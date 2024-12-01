CREATE OR REPLACE FUNCTION UpdatePostVoteScoreAndUserKarma()
RETURNS TRIGGER AS $$
BEGIN
	IF TG_OP = 'INSERT' THEN
		UPDATE Posts
		SET	VoteScore = VoteScore + NEW.VoteValue
		WHERE PostId = NEW.PostId;

		UPDATE Users
		SET Karma = Karma + NEW.VoteValue
		WHERE UserId = NEW.UserId;
	
	ELSIF TG_OP = 'DELETE' THEN
		UPDATE Posts
		SET VoteScore = VoteScore - OLD.VoteValue
		WHERE PostId = OLD.PostId;

		UPDATE Users
		SET Karma = Karma - OLD.VoteValue
		WHERE UserId = OLD.UserId;
	
	ELSIF TG_OP = 'UPDATE' THEN
		UPDATE Posts
		SET VoteScore = VoteScore - OLD.VoteValue + NEW.VoteValue
		WHERE PostId= OLD.PostId;

		UPDATE Users
        SET Karma = Karma - OLD.VoteValue + NEW.VoteValue
        WHERE UserId = NEW.UserId;
	END IF;
	RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER TriggerPostVoteScore
AFTER INSERT OR DELETE OR UPDATE ON PostVotes
FOR EACH ROW EXECUTE FUNCTION UpdatePostVoteScoreAndUserKarma();
