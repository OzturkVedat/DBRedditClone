CREATE OR REPLACE FUNCTION PreventDuplicateSubscriptions() RETURNS TRIGGER AS $$
BEGIN
	IF EXISTS(
	SELECT 1
	FROM UserSubs
	WHERE SubredditId= NEW.SubredditId AND UserId= NEW.UserId
	)THEN
		RAISE EXCEPTION 'User is already subscribed to this subreddit.';
	END IF;
	RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE TRIGGER TriggerDuplicateSubs
BEFORE INSERT ON UserSubs
FOR EACH ROW EXECUTE FUNCTION PreventDuplicateSubscriptions();