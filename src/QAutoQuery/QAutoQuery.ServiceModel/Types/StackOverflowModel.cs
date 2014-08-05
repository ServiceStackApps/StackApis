using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace QAutoQuery.ServiceModel.Types
{
    public class StackOverflowUser
    {
        public int reputation { get; set; }
        public int user_id { get; set; }
        public string user_type { get; set; }
        public int accept_rate { get; set; }
        public string profile_image { get; set; }
        public string display_name { get; set; }
        public string link { get; set; }
    }

    public class QuestionItem
    {
        [PrimaryKey]
        [Index(Unique = true)]
        public int question_id { get; set; }

        public string[] tags { get; set; }
        public StackOverflowUser owner { get; set; }
        public bool is_answered { get; set; }
        public int view_count { get; set; }
        public int answer_count { get; set; }
        public int score { get; set; }
        public int last_activity_date { get; set; }
        public int creation_date { get; set; }
        public int last_edit_date { get; set; }
        public string link { get; set; }
        public string title { get; set; }
        public int? accepted_answer_id { get; set; }
    }
}
