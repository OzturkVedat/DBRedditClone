CREATE OR REPLACE FUNCTION GetUserById(_UserId UUID)
RETURNS TABLE(UserId UUID, UserName VARCHAR, Email VARCHAR, PasswordHash TEXT, Karma INT) AS $$
BEGIN
	RETURN QUERY
	SELECT u.UserId, u.UserName, u.Email, u.PasswordHash, u.Karma
	FROM Users u
	WHERE u.UserId = _UserId;
END;
$$ LANGUAGE plpgsql;