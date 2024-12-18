﻿namespace DBRedditClone.Models
{
    public class CommentModel
    {
        public string? CommentId {  get; set; }
        public string PostId { get; set; }
        public string UserId {  get; set; }
        public string Content {  get; set; }
        public int VoteScore {  get; set; }
    }

    public class CommentEntity
    {
        public Guid? CommentId { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Content { get; set; }
        public int VoteScore { get; set; }
    }

    public class AddCommmentDto
    {
        public string PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
