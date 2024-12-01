CREATE OR REPLACE FUNCTION DeleteCommentVote(_VoteId UUID)
RETURNS void AS $$
BEGIN
	IF NOT EXISTS (SELECT 1 FROM CommentVotes WHERE VoteId = _VoteId) THEN
        RAISE EXCEPTION 'Vote does not exist in CommentVotes';
    END IF;

    DELETE FROM CommentVotes WHERE VoteId = _VoteId;

    DELETE FROM Votes WHERE VoteId = _VoteId AND NOT EXISTS (
        SELECT 1 FROM PostVotes WHERE VoteId = _VoteId
    ) AND NOT EXISTS (
        SELECT 1 FROM CommentVotes WHERE VoteId = _VoteId
    );

END;
$$ LANGUAGE plpgsql;