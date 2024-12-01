CREATE OR REPLACE FUNCTION GetAllPosts() 
RETURNS TABLE (
    PostId UUID,
    SubredditId UUID,
    UserId UUID,
    Title VARCHAR,
    Content TEXT,
    VoteScore INT
) AS $$
BEGIN
    RETURN QUERY SELECT * FROM Posts;
END;
$$ LANGUAGE plpgsql;
