CREATE OR REPLACE FUNCTION InsertPost(
    _PostId UUID,
    _SubredditId UUID,
    _UserId UUID,
    _Title VARCHAR,
    _Content TEXT,
    _VoteScore INT
) RETURNS void AS $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM UserSubs WHERE SubredditId = _SubredditId AND UserId = _UserId
    ) THEN
        RAISE EXCEPTION 'SubredditId and UserId pair does not exist in UserSubs';
    END IF;

    INSERT INTO Posts (PostId, SubredditId, UserId, Title, Content, VoteScore)
    VALUES (_PostId, _SubredditId, _UserId, _Title, _Content, _VoteScore);
END;
$$ LANGUAGE plpgsql;
