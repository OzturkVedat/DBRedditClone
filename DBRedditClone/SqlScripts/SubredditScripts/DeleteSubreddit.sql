CREATE OR REPLACE FUNCTION DeleteSubreddit(
    _SubredditId UUID
) RETURNS void AS $$
BEGIN
    DELETE FROM Subreddits WHERE SubredditId = _SubredditId;
END;
$$ LANGUAGE plpgsql;
