using ServiceStack;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceModel
{
    [Route("/questions")]
    public class StackOverflowQuery : QueryBase<QuestionItem>
    {

    }
}
