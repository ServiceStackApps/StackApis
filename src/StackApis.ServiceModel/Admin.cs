using System.Collections.Generic;
using ServiceStack;

namespace StackApis.ServiceModel
{
    [Restrict(InternalOnly = true)]
    [Route("/admin/stats")]
    public class GetStats {}

    public class GetStatsResponse
    {
        public long QuestionsCount { get; set; }
        public long AnswersCount { get; set; }
        public Dictionary<string, long> TagCounts { get; set; }
        public long TopQuestionScore { get; set; }
        public long TopQuestionViews { get; set; }
        public long TopAnswerScore { get; set; }
    }
}