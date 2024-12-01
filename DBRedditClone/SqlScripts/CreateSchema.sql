CREATE OR REPLACE FUNCTION CreateSchema() RETURNS void AS $$
BEGIN
    CREATE TABLE IF NOT EXISTS Users (
        UserId UUID PRIMARY KEY,
        UserName VARCHAR(100) NOT NULL UNIQUE,
        Email VARCHAR(100) NOT NULL UNIQUE,
        PasswordHash TEXT NOT NULL,
        Karma INT NOT NULL DEFAULT 0
    );

    CREATE TABLE IF NOT EXISTS Roles (
        RoleId UUID PRIMARY KEY,
        RoleName VARCHAR(50) NOT NULL UNIQUE
    );

    CREATE TABLE IF NOT EXISTS RoleAssignments (
        UserId UUID NOT NULL,
        RoleId UUID NOT NULL,
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Subreddits (
        SubredditId UUID PRIMARY KEY,
        CreatedBy UUID NULL,

        SubName VARCHAR(50) NOT NULL UNIQUE,
        SubDescription TEXT NULL,
        UserCount INT NOT NULL DEFAULT 0,

        FOREIGN KEY (CreatedBy) REFERENCES Users(UserId) ON DELETE SET NULL
    );

    CREATE TABLE IF NOT EXISTS UserSubs (
        SubredditId UUID NOT NULL,
        UserId UUID NOT NULL,
        PRIMARY KEY (SubredditId, UserId),
        FOREIGN KEY (SubredditId) REFERENCES Subreddits(SubredditId) ON DELETE CASCADE,
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Posts (
        PostId UUID PRIMARY KEY,
        SubredditId UUID NULL,
        UserId UUID NULL,

        Title VARCHAR(100) NOT NULL,
        Content TEXT,
        CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
        VoteScore INT NOT NULL DEFAULT 0,

        FOREIGN KEY (SubredditId, UserId) REFERENCES UserSubs(SubredditId, UserId) ON DELETE NO ACTION
    );

    CREATE TABLE IF NOT EXISTS PostReports (
        PostId UUID NOT NULL,
        ReporterId UUID NOT NULL,

        ReportDetails TEXT NOT NULL,
        IsResolved BOOLEAN NOT NULL DEFAULT FALSE,

        PRIMARY KEY (PostId, ReporterId),
        FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE,
        FOREIGN KEY (ReporterId) REFERENCES Users(UserId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Tags (
        TagId UUID PRIMARY KEY,
        TagName VARCHAR(50) NOT NULL
    );

    CREATE TABLE IF NOT EXISTS PostTags (
        PostId UUID NOT NULL,
        TagId UUID NOT NULL,
        PRIMARY KEY (PostId, TagId),
        FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE,
        FOREIGN KEY (TagId) REFERENCES Tags(TagId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Comments (
        CommentId UUID PRIMARY KEY,
        PostId UUID NOT NULL,
        UserId UUID NOT NULL,
        ParentId UUID NULL,

        Content TEXT NOT NULL,
        CreatedAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
        VoteScore INT NOT NULL DEFAULT 0,

        FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE,
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        FOREIGN KEY (ParentId) REFERENCES Comments(CommentId) ON DELETE SET NULL
    );

    CREATE TABLE IF NOT EXISTS CommentReactions (
        UserId UUID NOT NULL,
        CommentId UUID NOT NULL,
        Reaction VARCHAR(50) NOT NULL CHECK (Reaction IN ('love', 'funny', 'sad', 'congrats', 'thanks')),
        PRIMARY KEY (UserId, CommentId),
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
        FOREIGN KEY (CommentId) REFERENCES Comments(CommentId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Chats (
        ChatId UUID PRIMARY KEY,
        UserId UUID NOT NULL,
        FriendId UUID NULL,
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Messages (
        MessageId UUID PRIMARY KEY,
        ChatId UUID NOT NULL,
        Content TEXT NOT NULL,
        SentAt TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
        FOREIGN KEY (ChatId) REFERENCES Chats(ChatId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS Votes (
        VoteId UUID PRIMARY KEY,
        UserId UUID NOT NULL,
        VoteValue INT NOT NULL CHECK (VoteValue IN (1, -1)),
        FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS PostVotes (
        VoteId UUID PRIMARY KEY,
        PostId UUID NOT NULL,
        FOREIGN KEY (VoteId) REFERENCES Votes(VoteId) ON DELETE CASCADE,
        FOREIGN KEY (PostId) REFERENCES Posts(PostId) ON DELETE CASCADE
    );

    CREATE TABLE IF NOT EXISTS CommentVotes (
        VoteId UUID PRIMARY KEY,
        CommentId UUID NOT NULL,
        FOREIGN KEY (VoteId) REFERENCES Votes(VoteId) ON DELETE CASCADE,
        FOREIGN KEY (CommentId) REFERENCES Comments(CommentId) ON DELETE CASCADE
    );
END;
$$ LANGUAGE plpgsql;

SELECT CreateSchema();      -- execute the function