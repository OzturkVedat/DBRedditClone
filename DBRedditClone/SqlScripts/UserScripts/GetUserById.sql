﻿CREATE OR REPLACE FUNCTION GetUserById(
	_UserId UUID
) RETURNS TABLE(UserID UUID, UserName VARCHAR, Email VARCHAR, Karma INT) AS $$
BEGIN
	RETURN QUERY
	SELECT UserId, UserName, Email, Karma
	FROM Users
	WHERE UserId = _UserId;
END;
$$ LANGUAGE plpgsql;