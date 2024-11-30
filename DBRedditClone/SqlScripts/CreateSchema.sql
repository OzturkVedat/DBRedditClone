CREATE PROCEDURE CreateSchema
AS
BEGIN

	CREATE TABLE Users(
		UserId VARCHAR(36) PRIMARY KEY,
		UserName VARCHAR(100) NOT NULL UNIQUE,
		Email VARCHAR(100) NOT NULL UNIQUE,
		PasswordHash NVARCHAR(256) NOT NULL,
		Karma INT NOT NULL
	);

	CREATE TABLE Roles(
		RoleId VARCHAR(36) PRIMARY KEY,
		RoleName VARCHAR(50) NOT NULL UNIQUE
	);

	CREATE TABLE RoleAssignments(
		UserId VARCHAR(36) NOT NULL,
		RoleId VARCHAR(36) NOT NULL,

		FOREIGN KEY (UserId) REFERENCES Users(UserId)
			ON DELETE CASCADE,	-- delete the role assignments if user with the ID is removed
		FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
			ON DELETE CASCADE,
	);

	CREATE TABLE Subreddits(
		SubredditId VARCHAR(36) PRIMARY KEY,
		CreatedBy NVARCHAR(36) NULL,
		SubName NVARCHAR(50) NOT NULL UNIQUE,
		SubDescription NVARCHAR(200) NULL,

		FOREIGN KEY (CreatedBy) REFERENCES Users(UserId)
			ON DELETE SET NULL		-- set the creator id to null
	);

	CREATE TABLE UserSubs(
		SubredditId VARCHAR(36) NOT NULL,
		UserId VARCHAR(36) NOT NULL,

		PRIMARY KEY (SubredditId, UserId),

		FOREIGN KEY	(SubredditId) REFERENCES Subreddits(SubredditId)
			ON DELETE CASCADE,	-- remove all the sub records of the deleted subreddit
		FOREIGN KEY (UserId) REFERENCES Users(UserId)
			ON DELETE CASCADE	-- remove all the subs of removed user
	);


	CREATE TABLE Posts (
		PostId VARCHAR(36) PRIMARY KEY,
		SubredditId VARCHAR(36) NULL,  
		UserId VARCHAR(36) NULL,      

		Title NVARCHAR(100) NOT NULL,
		Content NVARCHAR(MAX),
		CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

		-- FK to a composite key
		FOREIGN KEY (SubredditId, UserId) REFERENCES UserSubs(SubredditId, UserId)
		    ON DELETE NO ACTION -- no need to remove composite FK if user unsubscribes
	);


	CREATE TABLE PostReports(
		PostId VARCHAR(36) NOT NULL,
		ReporterId VARCHAR(36) NOT NULL,

		ReportDetails NVARCHAR(500) NOT NULL,
		IsResolved BIT NOT NULL DEFAULT 0,

		PRIMARY KEY (PostId, ReporterId),

		FOREIGN KEY (PostId) REFERENCES Posts(PostId)
			ON DELETE CASCADE,
		FOREIGN KEY (ReporterId) REFERENCES Users(UserId)
			ON DELETE CASCADE
	);
	
	CREATE TABLE Tags(
		TagId VARCHAR(36) NOT NULL,
		TagName NVARCHAR(50) NOT NULL
	);

	CREATE TABLE PostTags(
		PostId VARCHAR(36) NOT NULL,
		TagId VARCHAR (36) NOT NULL,

		PRIMARY KEY (PostId, TagId),

		FOREIGN KEY (PostId) REFERENCES Posts(PostId)
			ON DELETE CASCADE,
		FOREIGN KEY (TagId) REFERENCES Tags(TagId)
			ON DELETE CASCADE
	);

	CREATE TABLE Comments(
		CommentId VARCHAR(36) PRIMARY KEY,
		PostId VARCHAR(36) NOT NULL,
		UserId VARCHAR(36) NOT NULL,
		ParentId VARCHAR(36) NULL,

		Content NVARCHAR(MAX) NOT NULL,
		CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),

		FOREIGN KEY (PostId) REFERENCES Posts(PostId)
			ON DELETE CASCADE,
		FOREIGN KEY (UserId) REFERENCES Users(UserId)
			ON DELETE CASCADE,
		FOREIGN KEY (ParentId) REFERENCES Comments(CommentId)
			ON DELETE SET NULL
	);

	CREATE TABLE CommentReactions(
		UserId VARCHAR(36) NOT NULL,
		CommentId VARCHAR(36) NOT NULL,
		Reaction NVARCHAR(50) NOT NULL,

		CONSTRAINT CHK_Reaction CHECK (Reaction IN ('love', 'funny', 'sad', 'congrats', 'thanks')),

		PRIMARY KEY (UserId, CommentId),

		FOREIGN KEY (UserId) REFERENCES Users(UserId)
			ON DELETE CASCADE,
		FOREIGN KEY (CommentId) REFERENCES Comments(CommentId)
			ON DELETE CASCADE
	);
	
	CREATE TABLE Chats(
	ChatId VARCHAR(36) PRIMARY KEY,
	UserId VARCHAR(36) NOT NULL,
	FriendId VARCHAR(36),

	FOREIGN KEY (UserId) REFERENCES Users(UserId)
		ON DELETE CASCADE
	);

	CREATE TABLE Messages(
	MessageId VARCHAR(36) PRIMARY KEY,
	ChatId VARCHAR(36) NOT NULL,

	Content NVARCHAR(MAX) NOT NULL,
	SentAt DATETIME NOT NULL DEFAULT GETDATE(),

	FOREIGN KEY (ChatId) REFERENCES Chats(ChatId)
		ON DELETE CASCADE	-- remove the messages if the chat is deleted
	);

	CREATE TABLE Votes(
		VoteId VARCHAR(36) PRIMARY KEY,
		UserId VARCHAR(36) NOT NULL,
		VoteValue INT NOT NULL CHECK (VoteValue IN (1, -1)),	-- vote can either be -1 or 1 point
		
		FOREIGN KEY (UserId) REFERENCES Users(UserID)
			ON DELETE CASCADE
	);

	CREATE TABLE PostVotes(
		VoteId VARCHAR(36) PRIMARY KEY,
		PostId VARCHAR(36) NOT NULL,

		FOREIGN KEY (VoteId) REFERENCES Votes(VoteId) ON DELETE CASCADE,
		FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE
	);

	CREATE TABLE CommentVotes (
		VoteId VARCHAR(36) PRIMARY KEY,
		CommentId VARCHAR(36) NOT NULL,
    
		FOREIGN KEY (VoteId) REFERENCES Votes(VoteId) ON DELETE CASCADE,
		FOREIGN KEY (CommentId) REFERENCES Comments(CommentId) ON DELETE CASCADE
	);

END;