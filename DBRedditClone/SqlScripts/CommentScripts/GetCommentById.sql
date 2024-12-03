CREATE OR REPLACE FUNCTION GetCommentById(
    _CommentId UUID
) 
RETURNS TABLE (
    CommentId UUID, 
    PostId UUID, 
    UserId UUID, 
    Content TEXT, 
    VoteScore INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT c.CommentId, c.PostId, c.UserId, c.Content, c.VoteScore
    FROM Comments c
    WHERE c.CommentId = _CommentId;
END;
$$ LANGUAGE plpgsql;
