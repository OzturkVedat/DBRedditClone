Users(UserId: UUID, UserName: String, Email: String, PasswordHash: String, Karma: Integer)

Roles(RoleId: UUID, RoleName: String)

RoleAssignments(UserId: UUID, RoleId: UUID)

Subreddits(SubredditId: UUID, CreatedBy: UUID, SubName: String, SubDescription: String)

UserSubs(SubredditId: UUID, UserId: UUID)

Posts(PostId: UUID, SubredditId: UUID, UserId: UUID, Title: String, Content: String, CreatedAt: Timestamp)

PostReports(PostId: UUID, ReporterId: UUID, ReportDetails: String, IsResolved: Boolean)

Tags(TagId: UUID, TagName: String)

PostTags(PostId: UUID, TagId: UUID)

Comments(CommentId: UUID, PostId: UUID, UserId: UUID, ParentId: UUID, Content: String, CreatedAt: Timestamp)

CommentReactions(UserId: UUID, CommentId: UUID, Reaction: String)

Chats(ChatId: UUID, UserId: UUID, FriendId: UUID)

Messages(MessageId: UUID, ChatId: UUID, Content: String, SentAt: Timestamp)

Votes(VoteId: UUID, UserId: UUID, VoteValue: Integer)

PostVotes(VoteId: UUID, PostId: UUID)

CommentVotes(VoteId: UUID, CommentId: UUID)
