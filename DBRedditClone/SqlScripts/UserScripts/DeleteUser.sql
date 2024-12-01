CREATE OR REPLACE FUNCTION DeleteUser(_UserId UUID) RETURNS void AS $$
BEGIN
    DELETE FROM Users WHERE UserId = _UserId;
     IF NOT FOUND THEN
        RAISE EXCEPTION 'No user found with UserId %', _UserId;
    END IF;
END;
$$ LANGUAGE plpgsql;