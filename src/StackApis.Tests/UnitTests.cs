using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Testing;
using ServiceStack.Text;
using ServiceStack.Web;
using StackApis.ServiceInterface;
using StackApis.ServiceModel.Types;

namespace StackApis.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(MyServices).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                    container.Register<IDbConnectionFactory>(
                        new OrmLiteConnectionFactory(
                            "~/../../../StackApis/App_Data/db.sqlite".MapServerPath(),
                            SqliteDialect.Provider));

                    using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
                    {
                        db.DropAndCreateTable<Question>();
                        db.DropAndCreateTable<Answer>();
                        db.DropAndCreateTable<QuestionTag>();
                    }

                    SeedStackOverflowData(container.Resolve<IDbConnectionFactory>());
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }


        [Test]
        public void TestImport()
        {
            var dbConnectionFactory = appHost.TryResolve<IDbConnectionFactory>();
            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                var numberOfQuestions = db.Count<Question>();
                var numberOfAnswers = db.Count<Answer>();
                Assert.That(numberOfQuestions > 0);
                Assert.That(numberOfAnswers > 0);
            }
        }

        private void SeedStackOverflowData(IDbConnectionFactory dbConnectionFactory)
        {
            var client = new JsonServiceClient();
            int numberOfPages = 5;
            int pageSize = 100;
            var dbQuestions = new List<Question>();
            var dbAnswers = new List<Answer>();
            for (int i = 1; i < numberOfPages + 1; i++)
            {
                //Throttle queries
                Thread.Sleep(500);
                var questionsResponse =
                    client.Get("https://api.stackexchange.com/2.2/questions?page={0}&pagesize={1}&site={2}&tagged=servicestack"
                            .Fmt(i, pageSize, "stackoverflow"));

                QuestionsResponse qResponse;
                using (new ConfigScope())
                {
                    var json = questionsResponse.ReadToEnd();
                    qResponse = json.FromJson<QuestionsResponse>();
                    dbQuestions.AddRange(qResponse.Items.Select(q => q.ConvertTo<Question>()));
                }

                var acceptedAnswers =
                    qResponse.Items
                    .Where(x => x.AcceptedAnswerId != null)
                    .Select(x => x.AcceptedAnswerId).ToList();

                var answersResponse = client.Get("https://api.stackexchange.com/2.2/answers/{0}?sort=activity&site=stackoverflow"
                    .Fmt(acceptedAnswers.Join(";")));

                using (new ConfigScope())
                {
                    var json = answersResponse.ReadToEnd();
                    var aResponse = JsonSerializer.DeserializeFromString<AnswersResponse>(json);
                    dbAnswers.AddRange(aResponse.Items.Select(a => a.ConvertTo<Answer>()));
                }
            }

            //Filter duplicates
            dbQuestions = dbQuestions.GroupBy(q => q.QuestionId).Select(q => q.First()).ToList();
            dbAnswers = dbAnswers.GroupBy(a => a.AnswerId).Select(a => a.First()).ToList();
            var questionTags = dbQuestions.SelectMany(q =>
                q.Tags.Select(t => new QuestionTag { QuestionId = q.QuestionId, Tag = t }));

            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                db.InsertAll(dbQuestions);
                db.InsertAll(dbAnswers);
                db.InsertAll(questionTags);
            }
        }
    }

    public class ConfigScope : IDisposable
    {
        private readonly WriteComplexTypeDelegate holdQsStrategy;
        private readonly JsConfigScope jsConfigScope;

        public ConfigScope()
        {
            jsConfigScope = JsConfig.With(dateHandler: DateHandler.UnixTime,
                                          propertyConvention: PropertyConvention.Lenient,
                                          emitLowercaseUnderscoreNames: true,
                                          emitCamelCaseNames: false);

            holdQsStrategy = QueryStringSerializer.ComplexTypeStrategy;
            QueryStringSerializer.ComplexTypeStrategy = QueryStringStrategy.FormUrlEncoded;
        }

        public void Dispose()
        {
            QueryStringSerializer.ComplexTypeStrategy = holdQsStrategy;
            jsConfigScope.Dispose();
        }
    }
}
