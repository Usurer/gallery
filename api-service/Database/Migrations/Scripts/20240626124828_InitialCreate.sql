﻿CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
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
    "UpdatedAtDate" INTEGER NOT NULL,
    CONSTRAINT "FK_FileSystemItems_FileSystemItems_ParentId" FOREIGN KEY ("ParentId") REFERENCES "FileSystemItems" ("Id") ON DELETE RESTRICT
);

CREATE TABLE "ScanTargets" (
    "Id" INTEGER NOT NULL CONSTRAINT "PK_ScanTargets" PRIMARY KEY AUTOINCREMENT,
    "Path" TEXT NOT NULL,
    "IsScanned" INTEGER NOT NULL,
    "IsInvalid" INTEGER NOT NULL
);

CREATE TABLE "Images" (
    "FileSystemItemId" INTEGER NOT NULL CONSTRAINT "PK_Images" PRIMARY KEY,
    "Extension" TEXT NOT NULL,
    "Width" INTEGER NOT NULL,
    "Height" INTEGER NOT NULL,
    CONSTRAINT "FK_Images_FileSystemItems_FileSystemItemId" FOREIGN KEY ("FileSystemItemId") REFERENCES "FileSystemItems" ("Id") ON DELETE RESTRICT
);

CREATE INDEX "IX_FileSystemItems_ParentId" ON "FileSystemItems" ("ParentId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240626124828_InitialCreate', '7.0.10');

COMMIT;

