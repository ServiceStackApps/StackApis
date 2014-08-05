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
using StackAPI.Questions.ServiceInterface;
using StackAPI.Questions.ServiceModel.Types;

namespace StackAPI.Questions.Tests
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
                    container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory("~/../../../StackAPI.Questions/App_Data/db.sqlite".MapServerPath(), SqliteDialect.Provider));
                    using (var db = container.Resolve<IDbConnectionFactory>().OpenDbConnection())
                    {
                        db.DropAndCreateTable<QuestionItem>();
                        db.DropAndCreateTable<AnswerItem>();
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
                var numberOfQuestions = db.Count<QuestionItem>();
                var numberOfAnswers = db.Count<AnswerItem>();
                Assert.That(numberOfQuestions > 0);
                Assert.That(numberOfAnswers > 0);
            }
        }

        private void SeedStackOverflowData(IDbConnectionFactory dbConnectionFactory)
        {
            JsonServiceClient client = new JsonServiceClient();
            int numberOfPages = 5;
            int pageSize = 100;
            var dbQuestions = new List<QuestionItem>();
            var dbAnswers= new List<AnswerItem>();
            for (int i = 1; i < numberOfPages + 1; i++)
            {
                //Throttle queries
                Thread.Sleep(500);
                var questionsResponse =
                    client.Get(
                        "https://api.stackexchange.com/2.2/questions?page={0}&pagesize={1}&site={2}&tagged=servicestack"
                            .Fmt(i, pageSize, "stackoverflow"));
                //There is an extension method I'm forgetting...
                var qResponseBytes = questionsResponse.GetResponseStream().ReadFully();
                var qResponseString = UTF8Encoding.UTF8.GetString(qResponseBytes);
                QuestionResponse questionsResponseDto;
                using (var scope = new ConfigScope())
                {
                    questionsResponseDto = JsonSerializer.DeserializeFromString<QuestionResponse>(qResponseString);
                    dbQuestions.AddRange(questionsResponseDto.Items.Select(stackOverflowQuestion => stackOverflowQuestion.ConvertTo<QuestionItem>()).ToList());
                }
                var acceptedAnswers =
                    questionsResponseDto.Items.Where(x => x.AcceptedAnswerId != null).Select(x => x.AcceptedAnswerId).ToList();

                var answersResponse = client.Get("https://api.stackexchange.com/2.2/answers/{0}?sort=activity&site=stackoverflow".Fmt(acceptedAnswers.Join(";")));
                var aResponseBytes = answersResponse.GetResponseStream().ReadFully();
                var aResponseString = UTF8Encoding.UTF8.GetString(aResponseBytes);
                AnswerResponse answersResponseDto;
                using (var scope = new ConfigScope())
                {
                    answersResponseDto = JsonSerializer.DeserializeFromString<AnswerResponse>(aResponseString);
                    dbAnswers.AddRange(answersResponseDto.Items.Select(stackOverflowAnswer => stackOverflowAnswer.ConvertTo<AnswerItem>()).ToList());
                }
            }

            //Filter duplicates
            dbQuestions = dbQuestions.GroupBy(q => q.QuestionId).Select(q => q.First()).ToList();
            dbAnswers = dbAnswers.GroupBy(a => a.AnswerId).Select(a => a.First()).ToList();
            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                db.InsertAll(dbQuestions);
                db.InsertAll(dbAnswers);
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
