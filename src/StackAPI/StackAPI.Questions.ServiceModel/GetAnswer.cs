using ServiceStack;
using StackAPI.Questions.ServiceModel.Types;

namespace StackAPI.Questions.ServiceModel
{
    [Route("/getanswer/{QuestionId}")]
    public class GetAnswer : IReturn<GetAnswerResponse>
    {
        public int QuestionId { get; set; }
    }

    public class GetAnswerResponse
    {
        public AnswerItem Ansnwer { get; set; }
    }
}
