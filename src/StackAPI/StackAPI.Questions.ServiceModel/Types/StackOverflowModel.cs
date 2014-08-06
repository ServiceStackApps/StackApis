using System.Collections.Generic;
using ServiceStack.DataAnnotations;

namespace StackApis.ServiceModel.Types
{
    public class StackOverflowUser
    {
        public int Reputation { get; set; }
        public int Userid { get; set; }
        public string UserType { get; set; }
        public int AcceptRate { get; set; }
        public string ProfileImage { get; set; }
        public string DisplayName { get; set; }
        public string Link { get; set; }
    }

    public class QuestionItem
    {
        [PrimaryKey]
        [Index(Unique = true)]
        [Alias("Id")]
        public int QuestionId { get; set; }

        public string[] Tags { get; set; }
        public StackOverflowUser Owner { get; set; }
        public bool IsAnswered { get; set; }
        public int ViewCount { get; set; }
        public int AnswerCount { get; set; }
        public int Score { get; set; }
        public int LastActivityDate { get; set; }
        public int CreationDate { get; set; }
        public int LastEditDate { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public int? AcceptedAnswerId { get; set; }
    }


    public class AnswerItem
    {
        [PrimaryKey]
        [Index(Unique = true)]
        [Alias("Id")]
        public int AnswerId { get; set; }

        public StackOverflowUser Owner { get; set; }
        public bool IsAccepted { get; set; }
        public int Score { get; set; }
        public int LastActivityDate { get; set; }
        public int LastEditDate { get; set; }
        public int CreationDate { get; set; }
        public int QuestionId { get; set; }
    }

    public class AnswerResponse
    {
        public List<AnswerItem> Items { get; set; }
        public bool HasMore { get; set; }
        public int QuotaMax { get; set; }
        public int QuotaRemaining { get; set; }
    }

    public class QuestionResponse
    {
        public List<QuestionItem> Items { get; set; }
        public bool HasMore { get; set; }
        public int QuotaMax { get; set; }
        public int QuotaRemaining { get; set; }
    }
}
