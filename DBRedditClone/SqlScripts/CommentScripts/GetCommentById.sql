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
    SELECT CommentId, PostId, UserId, Content, VoteScore
    FROM Comments
    WHERE CommentId = _CommentId;
END;
$$ LANGUAGE plpgsql;
