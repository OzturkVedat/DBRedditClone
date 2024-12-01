CREATE OR REPLACE FUNCTION GetAllComments() 
RETURNS TABLE (
    CommentId UUID, 
    PostId UUID, 
    UserId UUID, 
    Content TEXT, 
    VoteScore INT
) AS $$
BEGIN
    RETURN QUERY SELECT * FROM Comments;
END;
$$ LANGUAGE plpgsql;
