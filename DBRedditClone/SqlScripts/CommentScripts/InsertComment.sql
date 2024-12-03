CREATE OR REPLACE FUNCTION InsertComment(
    _CommentId UUID,
    _PostId UUID,
    _UserId UUID,
    _Content TEXT,
    _VoteScore INT
) RETURNS void AS $$
BEGIN
    INSERT INTO Comments (CommentId, PostId, UserId, Content, VoteScore)
    VALUES (_CommentId, _PostId, _UserId, _Content, _VoteScore);
END;
$$ LANGUAGE plpgsql;
