CREATE OR REPLACE FUNCTION UpdateUser(
    _UserId UUID,
    _UserName VARCHAR,
    _PasswordHash TEXT
) RETURNS void AS $$
BEGIN
    UPDATE Users
    SET UserName = _UserName,
        PasswordHash = _PasswordHash
    WHERE UserId = _UserId;
END;
$$ LANGUAGE plpgsql;