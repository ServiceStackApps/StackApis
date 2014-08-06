using System.Collections.Generic;
using ServiceStack;
using ServiceStack.DataAnnotations;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceModel
{
    [Description("Get a list of Answers for a Question")]
    [Route("/getanswer/{QuestionId}")]
    public class GetAnswers : IReturn<GetAnswersResponse>
    {
        public int QuestionId { get; set; }
    }

    public class GetAnswersResponse
    {
        public Answer Ansnwer { get; set; }
    }

    public class AnswersResponse
    {
        public List<Answer> Items { get; set; }
        public bool HasMore { get; set; }
        public int QuotaMax { get; set; }
        public int QuotaRemaining { get; set; }
    }

    public class QuestionsResponse
    {
        public List<Question> Items { get; set; }
        public bool HasMore { get; set; }
        public int QuotaMax { get; set; }
        public int QuotaRemaining { get; set; }
    }
}