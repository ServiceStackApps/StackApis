using ServiceStack.DataAnnotations;

namespace StackApis.ServiceModel.Types
{
    public class User
    {
        public int Reputation { get; set; }
        public int Userid { get; set; }
        public string UserType { get; set; }
        public int AcceptRate { get; set; }
        public string ProfileImage { get; set; }
        public string DisplayName { get; set; }
        public string Link { get; set; }
    }

    public class Question
    {
        [PrimaryKey]
        [Alias("Id")]
        public int QuestionId { get; set; }

        public string Title { get; set; }
        public int Score { get; set; }
        public int ViewCount { get; set; }
        public bool IsAnswered { get; set; }
        public int AnswerCount { get; set; }
        public string Link { get; set; }
        public string[] Tags { get; set; }
        public User Owner { get; set; }
        public int LastActivityDate { get; set; }
        public int CreationDate { get; set; }
        public int LastEditDate { get; set; }
        public int? AcceptedAnswerId { get; set; }
    }

    public class QuestionTag
    {
        [AutoIncrement]
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public string Tag { get; set; }
    }

    public class Answer
    {
        [PrimaryKey]
        [Alias("Id")]
        public int AnswerId { get; set; }

        public User Owner { get; set; }
        public bool IsAccepted { get; set; }
        public int Score { get; set; }
        public int LastActivityDate { get; set; }
        public int LastEditDate { get; set; }
        public int CreationDate { get; set; }
        public int QuestionId { get; set; }
    }
}
