ALTER TABLE "dbo"."Clients" ADD "Civilite" int4 NOT NULL DEFAULT 0
;

ALTER TABLE "dbo"."Produits" ADD "RefProd" text
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201511021642534_AddPropertiesClientProduit',E'OpticaVL.Migrations.Configuration',decode('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', 'base64'),E'6.1.3-40302')
