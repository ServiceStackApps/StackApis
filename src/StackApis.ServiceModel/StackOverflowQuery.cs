using ServiceStack;
using StackApis.ServiceModel.Types;

namespace StackApis.ServiceModel
{
    [Route("/questions")]
    [AutoQueryViewer(
        Title = "Explore StackOverflow Questions", Description = "Find ServiceStack Questions on StackOverflow", IconUrl = "/Content/app/stacks-white-75.png",
        DefaultSearchField = "Title", DefaultSearchType = "Contains", DefaultSearchText = "ServiceStack")]
    public class StackOverflowQuery : QueryBase<Question>
    {
        public int? ScoreGreaterThan { get; set; }
    }
}