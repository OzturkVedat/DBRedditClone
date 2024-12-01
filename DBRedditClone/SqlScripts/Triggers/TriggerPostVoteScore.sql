CREATE OR REPLACE FUNCTION UpdatePostVoteScoreAndUserKarma()
RETURNS TRIGGER AS $$ 
BEGIN 
    IF TG_OP = 'INSERT' THEN
        -- update post VoteScore when a vote is inserted
        UPDATE Posts
        SET VoteScore = VoteScore + (SELECT VoteValue FROM Votes WHERE VoteId = NEW.VoteId)
        WHERE PostId = NEW.PostId;

        -- update the user's karma when a vote is inserted
        UPDATE Users
        SET Karma = Karma + (SELECT VoteValue FROM Votes WHERE VoteId = NEW.VoteId)
        WHERE UserId = (SELECT UserId FROM Votes WHERE VoteId = NEW.VoteId);

    ELSIF TG_OP = 'DELETE' THEN
        UPDATE Posts
        SET VoteScore = VoteScore - (SELECT VoteValue FROM Votes WHERE VoteId = OLD.VoteId)
        WHERE PostId = OLD.PostId;

        UPDATE Users
        SET Karma = Karma - (SELECT VoteValue FROM Votes WHERE VoteId = OLD.VoteId)
        WHERE UserId = (SELECT UserId FROM Votes WHERE VoteId = OLD.VoteId);
    END IF;

    RETURN NULL;  
END; 
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER TriggerPostVoteScore
AFTER INSERT OR DELETE ON PostVotes
FOR EACH ROW EXECUTE FUNCTION UpdatePostVoteScoreAndUserKarma();
