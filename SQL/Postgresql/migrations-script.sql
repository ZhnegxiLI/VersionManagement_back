﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);


DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE TABLE "Environment" (
        "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "CreatedBy" bigint NULL,
        "CreatedOn" timestamp without time zone NULL,
        "UpdatedBy" bigint NULL,
        "UpdatedOn" timestamp without time zone NULL,
        "Name" text NULL,
        "Order" integer NULL,
        CONSTRAINT "PK_Environment" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE TABLE "Projet" (
        "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "CreatedBy" bigint NULL,
        "CreatedOn" timestamp without time zone NULL,
        "UpdatedBy" bigint NULL,
        "UpdatedOn" timestamp without time zone NULL,
        "Name" text NULL,
        "Description" text NULL,
        "ParentId" bigint NULL,
        CONSTRAINT "PK_Projet" PRIMARY KEY ("Id")
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE TABLE "ProjetEnvironment" (
        "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "CreatedBy" bigint NULL,
        "CreatedOn" timestamp without time zone NULL,
        "UpdatedBy" bigint NULL,
        "UpdatedOn" timestamp without time zone NULL,
        "ProjetId" bigint NOT NULL,
        "EnvironmentId" bigint NOT NULL,
        "Description" text NULL,
        CONSTRAINT "PK_ProjetEnvironment" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ProjetEnvironment_Environment_EnvironmentId" FOREIGN KEY ("EnvironmentId") REFERENCES "Environment" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_ProjetEnvironment_Projet_ProjetId" FOREIGN KEY ("ProjetId") REFERENCES "Projet" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE TABLE "Version" (
        "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "CreatedBy" bigint NULL,
        "CreatedOn" timestamp without time zone NULL,
        "UpdatedBy" bigint NULL,
        "UpdatedOn" timestamp without time zone NULL,
        "VersionNumber" text NULL,
        "ProjetId" bigint NOT NULL,
        CONSTRAINT "PK_Version" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Version_Projet_ProjetId" FOREIGN KEY ("ProjetId") REFERENCES "Projet" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE TABLE "DeploimentHistory" (
        "Id" bigint NOT NULL GENERATED BY DEFAULT AS IDENTITY,
        "CreatedBy" bigint NULL,
        "CreatedOn" timestamp without time zone NULL,
        "UpdatedBy" bigint NULL,
        "UpdatedOn" timestamp without time zone NULL,
        "VersionId" bigint NOT NULL,
        "ProjetEnvironmentId" bigint NOT NULL,
        CONSTRAINT "PK_DeploimentHistory" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_DeploimentHistory_ProjetEnvironment_ProjetEnvironmentId" FOREIGN KEY ("ProjetEnvironmentId") REFERENCES "ProjetEnvironment" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_DeploimentHistory_Version_VersionId" FOREIGN KEY ("VersionId") REFERENCES "Version" ("Id") ON DELETE CASCADE
    );
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE INDEX "IX_DeploimentHistory_ProjetEnvironmentId" ON "DeploimentHistory" ("ProjetEnvironmentId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE INDEX "IX_DeploimentHistory_VersionId" ON "DeploimentHistory" ("VersionId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE INDEX "IX_ProjetEnvironment_EnvironmentId" ON "ProjetEnvironment" ("EnvironmentId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE INDEX "IX_ProjetEnvironment_ProjetId" ON "ProjetEnvironment" ("ProjetId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    CREATE INDEX "IX_Version_ProjetId" ON "Version" ("ProjetId");
    END IF;
END $$;

DO $$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "MigrationId" = '20211208161202_init') THEN
    INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
    VALUES ('20211208161202_init', '3.1.21');
    END IF;
END $$;
