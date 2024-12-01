﻿CREATE OR REPLACE FUNCTION GetAllUsers()
RETURNS TABLE(UserId UUID, UserName VARCHAR, Karma INT) AS $$
BEGIN
	RETURN QUERY
	SELECT u.UserId, u.UserName, u.Karma
	FROM Users u
	ORDER BY u.Karma;
END;
$$ LANGUAGE plpgsql;