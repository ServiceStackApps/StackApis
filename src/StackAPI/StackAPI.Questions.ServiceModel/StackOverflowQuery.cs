using ServiceStack;
using StackAPI.Questions.ServiceModel.Types;

namespace StackAPI.Questions.ServiceModel
{
    [Route("/questions")]
    public class StackOverflowQuery : QueryBase<QuestionItem>
    {

    }
}
