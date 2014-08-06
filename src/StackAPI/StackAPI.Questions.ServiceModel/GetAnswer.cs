using ServiceStack;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceModel
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
