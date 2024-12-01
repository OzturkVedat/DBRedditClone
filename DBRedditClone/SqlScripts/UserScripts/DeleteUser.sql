CREATE OR REPLACE FUNCTION DeleteUser(
    _UserId UUID
) RETURNS void AS $$
BEGIN
    DELETE FROM Users WHERE UserId = _UserId;
END;
$$ LANGUAGE plpgsql;