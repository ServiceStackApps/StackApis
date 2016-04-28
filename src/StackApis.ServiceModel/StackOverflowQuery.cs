using ServiceStack;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceModel
{
    [Route("/questions")]
    [AutoQueryViewer(
        Title = "Explore StackOverflow Questions", Description = "Find ServiceStack Questions on StackOverflow", 
        IconUrl = "material-icons:cast",
        DefaultSearchField = "Title", DefaultSearchType = "Contains", DefaultSearchText = "ServiceStack")]
    public class StackOverflowQuery : QueryDb<Question>
    {
        public int? ScoreGreaterThan { get; set; }
    }
}