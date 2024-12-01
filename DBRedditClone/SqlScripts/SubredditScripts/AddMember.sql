CREATE OR REPLACE FUNCTION AddMember(
    _SubredditId UUID,
    _UserId UUID
) RETURNS void AS $$
BEGIN
    INSERT INTO UserSubs (SubredditId, UserId)
    VALUES (_SubredditId, _UserId);
END;
$$ LANGUAGE plpgsql;
