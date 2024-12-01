CREATE OR REPLACE FUNCTION UpdateCommentVoteScore()
RETURNS TRIGGER AS $$
BEGIN
	IF TG_OP = 'INSERT' THEN
		UPDATE Comments
		SET VoteScore = VoteScore + NEW.VoteValue
		WHERE CommentId = NEW.CommentId;
	
	ELSIF TG_OP = 'DELETE' THEN
		UPDATE Comments
		SET VoteScore = VoteScore - OLD.VoteValue
		WHERE CommentId = OLD.CommentId;
	
	ELSIF TG_OP= 'UPDATE' THEN
		UPDATE Comments
		SET VoteScore = VoteScore - OLD.VoteValue + NEW.VoteValue
		WHERE CommentId = NEW.CommentId;
	END IF;
	RETURN NULL;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE TRIGGER TriggerCommentVoteScore
AFTER INSERT OR DELETE OR UPDATE ON CommentVotes
FOR EACH ROW EXECUTE FUNCTION UpdateCommentVoteScore();