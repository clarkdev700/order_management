CREATE SCHEMA dbo
;

CREATE TABLE "dbo"."AspNetRoles"("Id" varchar(128) NOT NULL DEFAULT '',"Name" varchar(256) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetRoles" PRIMARY KEY ("Id"))
;

CREATE UNIQUE INDEX "AspNetRoles_RoleNameIndex" ON "dbo"."AspNetRoles" ("Name")
;

CREATE TABLE "dbo"."AspNetUserRoles"("UserId" varchar(128) NOT NULL DEFAULT '',"RoleId" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUserRoles" PRIMARY KEY ("UserId","RoleId"))
;

CREATE INDEX "AspNetUserRoles_IX_UserId" ON "dbo"."AspNetUserRoles" ("UserId")
;

CREATE INDEX "AspNetUserRoles_IX_RoleId" ON "dbo"."AspNetUserRoles" ("RoleId")
;

CREATE TABLE "dbo"."AspNetUsers"("Id" varchar(128) NOT NULL DEFAULT '',"Email" varchar(256),"EmailConfirmed" boolean NOT NULL DEFAULT FALSE,"PasswordHash" text,"SecurityStamp" text,"PhoneNumber" text,"PhoneNumberConfirmed" boolean NOT NULL DEFAULT FALSE,"TwoFactorEnabled" boolean NOT NULL DEFAULT FALSE,"LockoutEndDateUtc" timestamp,"LockoutEnabled" boolean NOT NULL DEFAULT FALSE,"AccessFailedCount" int4 NOT NULL DEFAULT 0,"UserName" varchar(256) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUsers" PRIMARY KEY ("Id"))
;

CREATE UNIQUE INDEX "AspNetUsers_UserNameIndex" ON "dbo"."AspNetUsers" ("UserName")
;

CREATE TABLE "dbo"."AspNetUserClaims"("Id" serial4 NOT NULL,"UserId" varchar(128) NOT NULL DEFAULT '',"ClaimType" text,"ClaimValue" text,CONSTRAINT "PK_dbo.AspNetUserClaims" PRIMARY KEY ("Id"))
;

CREATE INDEX "AspNetUserClaims_IX_UserId" ON "dbo"."AspNetUserClaims" ("UserId")
;

CREATE TABLE "dbo"."AspNetUserLogins"("LoginProvider" varchar(128) NOT NULL DEFAULT '',"ProviderKey" varchar(128) NOT NULL DEFAULT '',"UserId" varchar(128) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.AspNetUserLogins" PRIMARY KEY ("LoginProvider","ProviderKey","UserId"))
;

CREATE INDEX "AspNetUserLogins_IX_UserId" ON "dbo"."AspNetUserLogins" ("UserId")
;

ALTER TABLE "dbo"."AspNetUserRoles" ADD CONSTRAINT "FK_dbo.AspNetUserRoles_dbo.AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "dbo"."AspNetRoles" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AspNetUserRoles" ADD CONSTRAINT "FK_dbo.AspNetUserRoles_dbo.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "dbo"."AspNetUsers" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AspNetUserClaims" ADD CONSTRAINT "FK_dbo.AspNetUserClaims_dbo.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "dbo"."AspNetUsers" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."AspNetUserLogins" ADD CONSTRAINT "FK_dbo.AspNetUserLogins_dbo.AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "dbo"."AspNetUsers" ("Id") ON DELETE CASCADE
;

CREATE TABLE "dbo"."__MigrationHistory"("MigrationId" varchar(150) NOT NULL DEFAULT '',"ContextKey" varchar(300) NOT NULL DEFAULT '',"Model" bytea NOT NULL DEFAULT '',"ProductVersion" varchar(32) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.__MigrationHistory" PRIMARY KEY ("MigrationId","ContextKey"))
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201511251734386_Init',E'OpticaVL.AuthMigrations.Configuration',decode('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', 'base64'),E'6.1.3-40302')
