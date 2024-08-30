CREATE OR REPLACE FUNCTION "NotifyUserConnectionToggled"()
	RETURNS TRIGGER AS $$
    BEGIN
    PERFORM pg_notify('UserConnectionToggled',
        json_build_object(
            'Username', NEW."Username",
            'IsActive', NEW."ConnectionId" IS NOT NULL)
        ::TEXT);
	RETURN NEW;
    END
    $$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS "UserConnectionToggled" ON "Users";

CREATE TRIGGER "UserConnectionToggled"
    AFTER UPDATE OF "ConnectionId" ON "Users"
    FOR EACH ROW
    WHEN ((OLD."ConnectionId" IS NOT NULL AND NEW."ConnectionId" IS NULL)
        OR (OLD."ConnectionId" IS NULL AND NEW."ConnectionId" IS NOT NULL))
    EXECUTE PROCEDURE "NotifyUserConnectionToggled"()