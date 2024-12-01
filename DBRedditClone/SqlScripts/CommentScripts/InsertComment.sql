CREATE OR REPLACE FUNCTION InsertComment(
    _CommentId UUID,
    _PostId UUID,
    _UserId UUID,
    _ParentId UUID,
    _Content TEXT
) RETURNS void AS $$
BEGIN
    INSERT INTO Comments (CommentId, PostId, UserId, ParentId, Content)
    VALUES (_CommentId, _PostId, _UserId, _ParentId, _Content);
END;
$$ LANGUAGE plpgsql;
