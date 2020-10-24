ALTER TABLE "dbo"."Ventes" ADD "MontantClient" float4 NOT NULL DEFAULT 0
;

ALTER TABLE "dbo"."Ventes" ADD "MontantAssurance" float4 NOT NULL DEFAULT 0
;

ALTER TABLE "dbo"."Ventes" ADD "PayeAssurance" boolean NOT NULL DEFAULT FALSE
;

ALTER TABLE "dbo"."Ventes" ADD "PayeClient" boolean NOT NULL DEFAULT FALSE
;

ALTER TABLE "dbo"."Ventes" ADD "AssuranceId" int4
;

ALTER TABLE "dbo"."GammeVerres" ALTER COLUMN "Libelle" TYPE text
;

ALTER TABLE "dbo"."GammeVerres" ALTER COLUMN "Libelle" SET NOT NULL
;

ALTER TABLE "dbo"."GammeVerres" ALTER COLUMN "Libelle" DROP DEFAULT
;

CREATE INDEX "Ventes_IX_AssuranceId" ON "dbo"."Ventes" ("AssuranceId")
;

ALTER TABLE "dbo"."Ventes" ADD CONSTRAINT "FK_dbo.Ventes_dbo.Assurances_AssuranceId" FOREIGN KEY ("AssuranceId") REFERENCES "dbo"."Assurances" ("Id")
;

ALTER TABLE "dbo"."Ventes" DROP COLUMN "Paye"
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201512161153280_MultipleTableUpdateProperties',E'OpticaVL.Migrations.Configuration',decode('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', 'base64'),E'6.1.3-40302')