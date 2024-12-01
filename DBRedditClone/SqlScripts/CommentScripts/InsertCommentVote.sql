CREATE OR REPLACE FUNCTION InsertCommentVote(
    _VoteId UUID,
    _UserId UUID,
    _VoteValue INT,
    _CommentId UUID
) RETURNS void AS $$
BEGIN
    IF _VoteValue NOT IN (1, -1) THEN
        RAISE EXCEPTION 'Invalid vote value, must be 1 or -1';
    END IF;

    INSERT INTO Votes (VoteId, UserId, VoteValue)
    VALUES (_VoteId, _UserId, _VoteValue);

    INSERT INTO CommentVotes (VoteId, CommentId)
    VALUES (_VoteId, _CommentId);

    -- You can add additional checks if needed, like validating the CommentId exists
    IF NOT EXISTS (SELECT 1 FROM Comments WHERE CommentId = _CommentId) THEN
        RAISE EXCEPTION 'Comment does not exist';
    END IF;
    
END;
$$ LANGUAGE plpgsql;
