CREATE OR REPLACE FUNCTION GetAllComments() 
RETURNS TABLE (
    CommentId UUID, 
    PostId UUID, 
    UserId UUID, 
    Content TEXT, 
    VoteScore INT
) AS $$
BEGIN
    RETURN QUERY
    SELECT C.CommentId, c.PostId, c.UserId, c.Content, c.VoteScore
    FROM Comments c
    ORDER BY UserId;
END;
$$ LANGUAGE plpgsql;
