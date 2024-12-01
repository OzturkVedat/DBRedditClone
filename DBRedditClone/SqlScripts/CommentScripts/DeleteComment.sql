CREATE OR REPLACE FUNCTION DeleteComment(
    _CommentId UUID
) RETURNS void AS $$
BEGIN
    DELETE FROM Comments
    WHERE CommentId = _CommentId;
END;
$$ LANGUAGE plpgsql;
