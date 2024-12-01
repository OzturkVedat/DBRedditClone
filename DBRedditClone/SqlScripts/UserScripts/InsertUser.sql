CREATE OR REPLACE FUNCTION InsertUser(
	_UserId UUID,
	_UserName VARCHAR,
	_Email VARCHAR,
	_PasswordHash TEXT,
	_Karma INT
)RETURNS void AS $$
BEGIN 
	INSERT INTO Users (UserId, UserName, Email, PasswordHash, Karma)
	VALUES (_UserId, _UserName, _Email, _PasswordHash, _Karma);
END;
$$ LANGUAGE plpgsql;