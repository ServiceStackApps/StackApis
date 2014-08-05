using System.Linq;
using ServiceStack;
using ServiceStack.OrmLite;
using StackAPI.Questions.ServiceModel;
using StackAPI.Questions.ServiceModel.Types;

namespace StackAPI.Questions.ServiceInterface
{
    public class MyServices : Service
    {
        public IAutoQuery AutoQuery { get; set; }

        public object Any(StackOverflowQuery request)
        {
            var q = AutoQuery.CreateQuery(request, Request.GetRequestParams());
            return AutoQuery.Execute(request, q);
        }

        public SearchQuestionResponse Get(SearchQuestion request)
        {
            var response = new SearchQuestionResponse
            {
                Results = Db.Select<QuestionItem>().Where(x => request.Tags.All(y => x.Tags.Contains(y))).ToList()
            };
            return response;
        }

        public GetAnswerResponse Get(GetAnswer request)
        {
            return new GetAnswerResponse
            {
                Ansnwer = Db.Single<AnswerItem>(x => x.QuestionId == request.QuestionId)
            };
        }
    }
}