CREATE OR REPLACE FUNCTION DeletePost(
    _PostId UUID
) RETURNS void AS $$
BEGIN
    IF NOT EXISTS (
        SELECT 1 FROM Posts WHERE PostId = _PostId
    ) THEN
        RAISE EXCEPTION 'Post with the given PostId does not exist';
    END IF;

    DELETE FROM Posts WHERE PostId = _PostId;
END;
$$ LANGUAGE plpgsql;
