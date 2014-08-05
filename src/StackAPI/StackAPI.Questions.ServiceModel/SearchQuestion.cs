using System.Collections.Generic;
using ServiceStack;
using StackAPI.Questions.ServiceModel.Types;

namespace StackAPI.Questions.ServiceModel
{
    [Route("/questions/search")]
    public class SearchQuestion : IReturn<SearchQuestionResponse>
    {
        public string[] Tags { get; set; }
        public string UserId { get; set; }
    }

    public class SearchQuestionResponse
    {
        public List<QuestionItem> Results { get; set; } 
    }
}
