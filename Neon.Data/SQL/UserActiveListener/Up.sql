CREATE OR REPLACE FUNCTION "NotifyActiveConnectionToggle"()
	RETURNS TRIGGER AS $$
    BEGIN
    PERFORM pg_notify('ActiveConnectionToggle',
        json_build_object(
            'UserName', NEW."UserName",
            'IsActive', NEW."ConnectionId" IS NOT NULL)
        ::TEXT);
	RETURN NEW;
    END
    $$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "AfterActiveConnectionToggle" ON "AspNetUsers";

CREATE TRIGGER "AfterActiveConnectionToggle"
    AFTER UPDATE OF "ConnectionId" ON "AspNetUsers"
    FOR EACH ROW
    WHEN ((OLD."ConnectionId" IS NOT NULL AND NEW."ConnectionId" IS NULL)
        OR (OLD."ConnectionId" IS NULL AND NEW."ConnectionId" IS NOT NULL))
    EXECUTE PROCEDURE "NotifyActiveConnectionToggle"()