CREATE OR REPLACE FUNCTION InsertUser(
    _UserId UUID,
    _UserName VARCHAR,
    _Email VARCHAR,
    _PasswordHash TEXT,
    _Karma INT
) RETURNS void AS $$
BEGIN 
    -- Check for duplicate email
    IF EXISTS (SELECT 1 FROM Users WHERE Email = _Email) THEN
        RAISE EXCEPTION 'Email already exists';
    END IF;

    INSERT INTO Users (UserId, UserName, Email, PasswordHash, Karma)
    VALUES (_UserId, _UserName, _Email, _PasswordHash, _Karma);
END;
$$ LANGUAGE plpgsql;
