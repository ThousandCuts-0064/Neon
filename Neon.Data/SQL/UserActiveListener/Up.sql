CREATE OR REPLACE FUNCTION "NotifyActiveConnectionToggle"()
	RETURNS TRIGGER AS $$
    BEGIN
    PERFORM pg_notify('ActiveConnectionToggle',
        json_build_object(
            'UserName', NEW."UserName",
            'IsActive', NEW."ActiveConnectionId" IS NOT NULL)
        ::TEXT);
	RETURN NEW;
    END
    $$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "AfterActiveConnectionToggle" ON "AspNetUsers";

CREATE TRIGGER "AfterActiveConnectionToggle"
    AFTER UPDATE OF "ActiveConnectionId" ON "AspNetUsers"
    FOR EACH ROW
    WHEN ((OLD."ActiveConnectionId" IS NOT NULL AND NEW."ActiveConnectionId" IS NULL)
        OR (OLD."ActiveConnectionId" IS NULL AND NEW."ActiveConnectionId" IS NOT NULL))
    EXECUTE PROCEDURE "NotifyActiveConnectionToggle"()