CREATE OR REPLACE FUNCTION InsertSubreddit(
    _SubredditId UUID,
    _CreatedBy UUID,
    _SubName VARCHAR,
    _SubDescription TEXT,
    _UserCount INT
) RETURNS void AS $$
BEGIN
    INSERT INTO Subreddits (SubredditId, CreatedBy, SubName, SubDescription, UserCount)
    VALUES (_SubredditId, _CreatedBy, _SubName, _SubDescription, _UserCount);
END;
$$ LANGUAGE plpgsql;
