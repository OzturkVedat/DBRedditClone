namespace DBRedditClone.Models
{

    public class PostModel      // for fetch
    {
        public string PostId { get; set; }
        public string SubredditId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int VoteScore { get; set; }
    }

    public class PostEntity         // for storage
    {
        public Guid PostId {  get; set; }
        public Guid SubredditId {  get; set; }
        public Guid UserId { get; set; }
        public string Title {  get; set; }
        public string Content {  get; set; }
        public int VoteScore {  get; set; }
    }
    public class NewPostDto     // for register
    {
        public string SubredditId { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class PostVoteDto
    {
        public string UserId { get; set; }
        public string PostId { get; set; }

    }
}
