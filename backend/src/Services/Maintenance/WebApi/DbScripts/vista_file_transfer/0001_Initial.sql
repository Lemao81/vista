DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'filetransfer') THEN
        CREATE SCHEMA filetransfer;
    END IF;
END $EF$;
CREATE TABLE IF NOT EXISTS filetransfer.__ef_migrations_history (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;
DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM pg_namespace WHERE nspname = 'filetransfer') THEN
        CREATE SCHEMA filetransfer;
    END IF;
END $EF$;

CREATE TABLE filetransfer.media_folders (
    id uuid NOT NULL,
    user_id uuid NOT NULL,
    storage_version smallint NOT NULL,
    original_name character varying(50) NOT NULL,
    created_utc timestamp with time zone NOT NULL,
    modified_utc timestamp with time zone,
    CONSTRAINT pk_media_folders PRIMARY KEY (id)
);

CREATE TABLE filetransfer.media_items (
    id uuid NOT NULL,
    media_folder_id uuid NOT NULL,
    user_id uuid NOT NULL,
    media_kind integer NOT NULL,
    media_size_kind integer NOT NULL,
    storage_version smallint NOT NULL,
    meta_data text NOT NULL,
    created_utc timestamp with time zone NOT NULL,
    modified_utc timestamp with time zone,
    CONSTRAINT pk_media_items PRIMARY KEY (id),
    CONSTRAINT fk_media_items_media_folders_media_folder_id FOREIGN KEY (media_folder_id) REFERENCES filetransfer.media_folders (id) ON DELETE CASCADE
);

CREATE INDEX ix_media_items_media_folder_id ON filetransfer.media_items (media_folder_id);

INSERT INTO filetransfer.__ef_migrations_history (migration_id, product_version)
VALUES ('20250128134342_Initial', '9.0.0');

COMMIT;

