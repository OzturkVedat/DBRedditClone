CREATE OR REPLACE FUNCTION InsertPostVote(
    _VoteId UUID,
    _UserId UUID,
    _VoteValue INT,
    _PostId UUID
) RETURNS void AS $$
BEGIN
    IF _VoteValue NOT IN (1, -1) THEN
        RAISE EXCEPTION 'Invalid vote value, must be 1 or -1';
    END IF;

    IF NOT EXISTS (SELECT 1 FROM Posts WHERE PostId = _PostId) THEN
        RAISE EXCEPTION 'Post does not exist';
    END IF;  

    INSERT INTO Votes (VoteId, UserId, VoteValue)
    VALUES (_VoteId, _UserId, _VoteValue);

    INSERT INTO PostVotes (VoteId, PostId)
    VALUES (_VoteId, _PostId);
END;
$$ LANGUAGE plpgsql;
