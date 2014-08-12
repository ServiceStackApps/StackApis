using ServiceStack;
using ServiceStack.OrmLite;
using StackApis.ServiceModel;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceInterface
{
    public class MyServices : Service
    {
        public object Get(SearchQuestions request)
        {
            var query = Db.From<Question>();

            if (request.Tags != null && request.Tags.Count > 0)
            {
                query.Join<QuestionTag>((q, t) => q.QuestionId == t.QuestionId)
                    .Where<QuestionTag>(x => Sql.In(x.Tag, request.Tags));
            }

            var response = new SearchQuestionsResponse
            {
                Results = Db.Select(query)
            };

            return response;
        }

        public object Get(GetAnswers request)
        {
            return new GetAnswersResponse
            {
                Ansnwer = Db.Single<Answer>(x => x.QuestionId == request.QuestionId)
            };
        }

        public object Get(GetStats request)
        {
            return new GetStatsResponse
            {
                QuestionsCount = Db.Count<Question>(),
                AnswersCount = Db.Count<Answer>(),
                TagCounts = Db.Dictionary<string, long>("SELECT Tag, COUNT(*) FROM QuestionTag GROUP BY Tag HAVING COUNT(*) > 2 ORDER BY COUNT(*) DESC"),
                TopQuestionScore = Db.Scalar<Question, long>(x => Sql.Max(x.Score)),
                TopQuestionViews = Db.Scalar<Question, long>(x => Sql.Max(x.ViewCount)),
                TopAnswerScore = Db.Scalar<Answer, long>(x => Sql.Max(x.Score)),
            };
        }
    }
}