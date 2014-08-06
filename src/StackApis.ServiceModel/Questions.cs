using System.Collections.Generic;
using ServiceStack;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceModel
{
    [Route("/questions/search")]
    public class SearchQuestions : IReturn<SearchQuestionsResponse>
    {
        public string[] Tags { get; set; }
        public string UserId { get; set; }
    }

    public class SearchQuestionsResponse
    {
        public List<Question> Results { get; set; }
    }
}
