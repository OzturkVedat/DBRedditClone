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
    RETURN QUERY
    SELECT p.PostId, p.SubredditId, p.UserId, p.Title, p.Content, p.VoteScore
    FROM Posts p
    ORDER BY p.Title;
END;
$$ LANGUAGE plpgsql;
