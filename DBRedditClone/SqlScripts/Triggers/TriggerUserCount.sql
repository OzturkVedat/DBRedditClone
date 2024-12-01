CREATE OR REPLACE FUNCTION UpdateSubredditUserCount() RETURNS TRIGGER AS $$
BEGIN
    IF TG_OP = 'INSERT' THEN
        UPDATE Subreddits
        SET UserCount = UserCount + 1
        WHERE SubredditId = NEW.SubredditId;
    ELSIF TG_OP = 'DELETE' THEN
        UPDATE Subreddits
        SET UserCount = UserCount - 1
        WHERE SubredditId = OLD.SubredditId;
    END IF;
    RETURN NULL;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE TRIGGER TriggerUserCount
AFTER INSERT OR DELETE ON UserSubs
FOR EACH ROW EXECUTE FUNCTION UpdateSubredditUserCount();
