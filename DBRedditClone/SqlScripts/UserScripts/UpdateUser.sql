CREATE OR REPLACE FUNCTION UpdateUser(
	_UserId UUID,
	_UserName VARCHAR,
	_Email VARCHAR,
	_PasswordHash TEXT,
	_Karma INT
) RETURNS void AS $$
BEGIN
	UPDATE Users
	SET UserName= _UserName,
		Email= _Email,
		PasswordHash= _PasswordHash,
		Karma= _Karma
	WHERE UserId = _UserId;
END;
$$ LANGUAGE plpgsql;