CREATE OR REPLACE FUNCTION "NotifyConnectionToggle"()
	RETURNS TRIGGER AS $$
    BEGIN
    PERFORM pg_notify('ConnectionToggle',
        json_build_object(
            'UserName', NEW."UserName",
            'IsActive', NEW."ConnectionId" IS NOT NULL)
        ::TEXT);
	RETURN NEW;
    END
    $$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "AfterConnectionToggle" ON "AspNetUsers";

CREATE TRIGGER "AfterConnectionToggle"
    AFTER UPDATE OF "ConnectionId" ON "AspNetUsers"
    FOR EACH ROW
    WHEN ((OLD."ConnectionId" IS NOT NULL AND NEW."ConnectionId" IS NULL)
        OR (OLD."ConnectionId" IS NULL AND NEW."ConnectionId" IS NOT NULL))
    EXECUTE PROCEDURE "NotifyConnectionToggle"()