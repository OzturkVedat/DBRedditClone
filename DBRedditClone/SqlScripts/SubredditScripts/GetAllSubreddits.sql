CREATE OR REPLACE FUNCTION GetAllSubreddits()
RETURNS TABLE (
    SubredditId UUID,
    CreatedBy UUID,
    SubName VARCHAR,
    SubDescription TEXT,
    UserCount INT
) AS $$
BEGIN
    RETURN QUERY SELECT * FROM Subreddits;
END;
$$ LANGUAGE plpgsql;
