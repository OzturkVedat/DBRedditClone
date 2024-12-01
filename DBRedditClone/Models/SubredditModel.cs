namespace DBRedditClone.Models
{
    public class SubredditModel
    {
        public string SubredditId {  get; set; }
        public string CreatedById {  get; set; }
        public string SubName { get; set; }
        public string SubDescription { get; set; }
        public int UserCount {  get; set; }

    }

    public class SubredditEntity
    {
        public Guid SubredditId { get; set; }
        public Guid CreatedById { get; set; }
        public string SubName { get; set; }
        public string SubDescription { get; set; }
        public int UserCount { get; set; }

    }


    public class NewSubredditDto
    {
        public string CreatedById { get; set; }
        public string SubName { get; set; }
        public string SubDescription { get; set; }

    }

    public class NewMemberDto
    {
        public string SubredditId { get; set; }
        public string UserId { get; set; }
    }

}
