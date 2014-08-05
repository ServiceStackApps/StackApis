using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;
using Funq;
using QAutoQuery.ServiceInterface;
using QAutoQuery.ServiceModel;
using QAutoQuery.ServiceModel.Types;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack;
using ServiceStack.Text;

namespace QAutoQuery
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("QAutoQuery", typeof(MyServices).Assembly)
        {

        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());

            Plugins.Add(new RazorFormat());
            Plugins.Add(new AutoQueryFeature { MaxLimit = 100});

            container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory("~/App_Data/db.sqlite".MapHostAbsolutePath(), SqliteDialect.Provider));
            //InitDatabaseSchema(container.Resolve<IDbConnectionFactory>());
            //SeedStackOverflowData(container.Resolve<IDbConnectionFactory>());

        }

        private void InitDatabaseSchema(IDbConnectionFactory dbConnectionFactory)
        {
            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                db.DropAndCreateTable<QuestionItem>();
            }
        }

        private void SeedStackOverflowData(IDbConnectionFactory dbConnectionFactory)
        {
            JsonServiceClient client = new JsonServiceClient();
            int numberOfPages = 5;
            int pageSize = 100;
            var dbQuestions = new List<QuestionItem>();
            for (int i = 1; i < numberOfPages + 1; i++)
            {
                //Throttle queries
                Thread.Sleep(500);
                var response =
                    client.Get(
                        "https://api.stackexchange.com/2.2/questions?page={0}&pagesize={1}&site={2}&tagged=servicestack"
                            .Fmt(i, pageSize, "stackoverflow"));
                //There is an extension method I'm forgetting...
                var responseBytes = response.GetResponseStream().ReadFully();
                var responseString = UTF8Encoding.UTF8.GetString(responseBytes);
                var responseDto = JsonSerializer.DeserializeFromString<StackOverflowResponse>(responseString);
                dbQuestions.AddRange(responseDto.items.Select(stackOverflowQuestion => stackOverflowQuestion.ConvertTo<QuestionItem>()).ToList());
            }

            //Filter duplicates
            dbQuestions = dbQuestions.GroupBy(q => q.question_id).Select(q => q.First()).ToList();
            
            using (var db = dbConnectionFactory.OpenDbConnection())
            {
                db.InsertAll(dbQuestions);
            }
        }
    }

    public class StackOverflowResponse
    {
        public List<QuestionItem> items { get; set; }
        public bool has_more { get; set; }
        public int quota_max { get; set; }
        public int quota_remaining { get; set; }
    }
}