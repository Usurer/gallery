CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);

BEGIN TRANSACTION;

CREATE TABLE "FileSystemItems" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_FileSystemItems" PRIMARY KEY AUTOINCREMENT,
    "ParentId" INTEGER NULL,
    "IsFolder" INTEGER NOT NULL,
    "Path" TEXT NOT NULL,
    "Name" TEXT NOT NULL,
    "CreationDate" INTEGER NOT NULL,
    "Extension" TEXT NULL,
    "Width" INTEGER NULL,
    "Height" INTEGER NULL,
    "UpdatedAtDate" INTEGER NOT NULL
);

CREATE TABLE "ScanTargets" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ScanTargets" PRIMARY KEY AUTOINCREMENT,
    "Path" TEXT NOT NULL,
    "IsScanned" INTEGER NOT NULL,
    "IsInvalid" INTEGER NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240621083418_InitialCreate', '7.0.10');

COMMIT;

