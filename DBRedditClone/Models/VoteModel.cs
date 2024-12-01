namespace DBRedditClone.Models
{
    public class VoteModel
    {
        public Guid VoteId { get; set; }
        public Guid UserId { get; set; }
        public int VoteValue { get; set; }
    }

    public class PostVote : VoteModel
    {
        public Guid PostId { get; set; }
    }
    public class CommentVote : VoteModel
    {
        public Guid CommentId { get; set; }
    }

}
