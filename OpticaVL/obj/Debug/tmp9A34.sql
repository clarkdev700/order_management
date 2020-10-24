CREATE SCHEMA dbo
;

CREATE TABLE "dbo"."Assurances"("Id" serial4 NOT NULL,"Code" text NOT NULL DEFAULT '',"Nom" text NOT NULL DEFAULT '',"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Assurances" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."Commandes"("Id" serial4 NOT NULL,"RefCmd" text NOT NULL DEFAULT '',"DateCmd" timestamp NOT NULL DEFAULT '-infinity',"DateLvrCmd" timestamp NOT NULL DEFAULT '-infinity',"DateRLvrCmd" timestamp,"StatutCmd" boolean NOT NULL DEFAULT FALSE,"Commentaire" text,"MontantAssur" float4 NOT NULL DEFAULT 0,"MontantClient" float4 NOT NULL DEFAULT 0,"PayeAssur" boolean NOT NULL DEFAULT FALSE,"PayeClient" boolean NOT NULL DEFAULT FALSE,"TypeCmd" int4 NOT NULL DEFAULT 0,"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,"DateDel" timestamp,"MotifDel" text,"AssuranceId" int4,"ClientId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.Commandes" PRIMARY KEY ("Id"))
;

CREATE INDEX "Commandes_IX_AssuranceId" ON "dbo"."Commandes" ("AssuranceId")
;

CREATE INDEX "Commandes_IX_ClientId" ON "dbo"."Commandes" ("ClientId")
;

CREATE TABLE "dbo"."Clients"("Id" serial4 NOT NULL,"Nom" text NOT NULL DEFAULT '',"Prenom" text NOT NULL DEFAULT '',"Contact" text,"Contact2" text,"Adresse" text,"Profession" text,"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Clients" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."LigneCommandes"("Id" serial4 NOT NULL,"QteCmd" int4 NOT NULL DEFAULT 0,"Rem" float4 NOT NULL DEFAULT 0,"RemDG" float4 NOT NULL DEFAULT 0,"PrixCmd" float4 NOT NULL DEFAULT 0,"Del" boolean NOT NULL DEFAULT FALSE,"CommandeId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.LigneCommandes" PRIMARY KEY ("Id"))
;

CREATE INDEX "LigneCommandes_IX_CommandeId" ON "dbo"."LigneCommandes" ("CommandeId")
;

CREATE TABLE "dbo"."Payements"("Id" serial4 NOT NULL,"MontantPaye" float4 NOT NULL DEFAULT 0,"ModePaye" int4 NOT NULL DEFAULT 0,"SourcePaye" int4 NOT NULL DEFAULT 0,"DatePaye" timestamp NOT NULL DEFAULT '-infinity',"MontantEncaisse" boolean NOT NULL DEFAULT FALSE,"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,"DateDel" timestamp,"MotifDel" text,"RefCmd" int4,"RefVente" int4,CONSTRAINT "PK_dbo.Payements" PRIMARY KEY ("Id"))
;

CREATE INDEX "Payements_IX_RefCmd" ON "dbo"."Payements" ("RefCmd")
;

CREATE INDEX "Payements_IX_RefVente" ON "dbo"."Payements" ("RefVente")
;

CREATE TABLE "dbo"."Ventes"("Id" serial4 NOT NULL,"RefVente" text,"DateVente" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"IdentiteClient" text,"Del" boolean NOT NULL DEFAULT FALSE,"DateDel" timestamp,"MotifDel" text,CONSTRAINT "PK_dbo.Ventes" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."LigneVentes"("Id" serial4 NOT NULL,"QteVente" int4 NOT NULL DEFAULT 0,"PrixVente" float4 NOT NULL DEFAULT 0,"Del" boolean NOT NULL DEFAULT FALSE,"VenteId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.LigneVentes" PRIMARY KEY ("Id"))
;

CREATE INDEX "LigneVentes_IX_VenteId" ON "dbo"."LigneVentes" ("VenteId")
;

CREATE TABLE "dbo"."Categories"("Id" serial4 NOT NULL,"Code" text NOT NULL DEFAULT '',"Libelle" text NOT NULL DEFAULT '',"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Categories" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."Produits"("Id" serial4 NOT NULL,"RefProd" text NOT NULL DEFAULT '',"Designation" text NOT NULL DEFAULT '',"Prix" float4 NOT NULL DEFAULT 0,"QteSeuil" int4 NOT NULL DEFAULT 0,"QteStock" int4 NOT NULL DEFAULT 0,"Del" boolean NOT NULL DEFAULT FALSE,"MarqueId" int4,"CategorieId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.Produits" PRIMARY KEY ("Id"))
;

CREATE INDEX "Produits_IX_MarqueId" ON "dbo"."Produits" ("MarqueId")
;

CREATE INDEX "Produits_IX_CategorieId" ON "dbo"."Produits" ("CategorieId")
;

CREATE TABLE "dbo"."Marques"("Id" serial4 NOT NULL,"Libelle" text NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.Marques" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."EntreeStocks"("Id" serial4 NOT NULL,"Qte" int4 NOT NULL DEFAULT 0,"Date" timestamp NOT NULL DEFAULT '-infinity',"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"Del" boolean NOT NULL DEFAULT FALSE,"DateDel" timestamp,"MotifDel" text,"FournisseurId" int4 NOT NULL DEFAULT 0,"ProduitId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.EntreeStocks" PRIMARY KEY ("Id"))
;

CREATE INDEX "EntreeStocks_IX_FournisseurId" ON "dbo"."EntreeStocks" ("FournisseurId")
;

CREATE INDEX "EntreeStocks_IX_ProduitId" ON "dbo"."EntreeStocks" ("ProduitId")
;

CREATE TABLE "dbo"."Fournisseurs"("Id" serial4 NOT NULL,"Code" text NOT NULL DEFAULT '',"Nom" text NOT NULL DEFAULT '',"Contact" text,"Contact2" text,"Email" text,"Adresse" text,"Del" boolean NOT NULL DEFAULT FALSE,CONSTRAINT "PK_dbo.Fournisseurs" PRIMARY KEY ("Id"))
;

CREATE TABLE "dbo"."ReceptionCommandes"("Id" serial4 NOT NULL,"DateReception" timestamp NOT NULL DEFAULT '-infinity',"IdentiteReceptionnaire" text,"Commentaire" text,"DateEnreg" timestamp NOT NULL DEFAULT '-infinity',"CommandeId" int4 NOT NULL DEFAULT 0,CONSTRAINT "PK_dbo.ReceptionCommandes" PRIMARY KEY ("Id"))
;

CREATE INDEX "ReceptionCommandes_IX_CommandeId" ON "dbo"."ReceptionCommandes" ("CommandeId")
;

ALTER TABLE "dbo"."Commandes" ADD CONSTRAINT "FK_dbo.Commandes_dbo.Assurances_AssuranceId" FOREIGN KEY ("AssuranceId") REFERENCES "dbo"."Assurances" ("Id")
;

ALTER TABLE "dbo"."Commandes" ADD CONSTRAINT "FK_dbo.Commandes_dbo.Clients_ClientId" FOREIGN KEY ("ClientId") REFERENCES "dbo"."Clients" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."LigneCommandes" ADD CONSTRAINT "FK_dbo.LigneCommandes_dbo.Commandes_CommandeId" FOREIGN KEY ("CommandeId") REFERENCES "dbo"."Commandes" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Payements" ADD CONSTRAINT "FK_dbo.Payements_dbo.Commandes_RefCmd" FOREIGN KEY ("RefCmd") REFERENCES "dbo"."Commandes" ("Id")
;

ALTER TABLE "dbo"."Payements" ADD CONSTRAINT "FK_dbo.Payements_dbo.Ventes_RefVente" FOREIGN KEY ("RefVente") REFERENCES "dbo"."Ventes" ("Id")
;

ALTER TABLE "dbo"."LigneVentes" ADD CONSTRAINT "FK_dbo.LigneVentes_dbo.Ventes_VenteId" FOREIGN KEY ("VenteId") REFERENCES "dbo"."Ventes" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Produits" ADD CONSTRAINT "FK_dbo.Produits_dbo.Categories_CategorieId" FOREIGN KEY ("CategorieId") REFERENCES "dbo"."Categories" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."Produits" ADD CONSTRAINT "FK_dbo.Produits_dbo.Marques_MarqueId" FOREIGN KEY ("MarqueId") REFERENCES "dbo"."Marques" ("Id")
;

ALTER TABLE "dbo"."EntreeStocks" ADD CONSTRAINT "FK_dbo.EntreeStocks_dbo.Fournisseurs_FournisseurId" FOREIGN KEY ("FournisseurId") REFERENCES "dbo"."Fournisseurs" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."EntreeStocks" ADD CONSTRAINT "FK_dbo.EntreeStocks_dbo.Produits_ProduitId" FOREIGN KEY ("ProduitId") REFERENCES "dbo"."Produits" ("Id") ON DELETE CASCADE
;

ALTER TABLE "dbo"."ReceptionCommandes" ADD CONSTRAINT "FK_dbo.ReceptionCommandes_dbo.Commandes_CommandeId" FOREIGN KEY ("CommandeId") REFERENCES "dbo"."Commandes" ("Id") ON DELETE CASCADE
;

CREATE TABLE "dbo"."__MigrationHistory"("MigrationId" varchar(150) NOT NULL DEFAULT '',"ContextKey" varchar(300) NOT NULL DEFAULT '',"Model" bytea NOT NULL DEFAULT '',"ProductVersion" varchar(32) NOT NULL DEFAULT '',CONSTRAINT "PK_dbo.__MigrationHistory" PRIMARY KEY ("MigrationId","ContextKey"))
;

INSERT INTO "dbo"."__MigrationHistory"("MigrationId","ContextKey","Model","ProductVersion") VALUES (E'201507301619171_InitialCreate',E'OpticaVL.Migrations.Configuration',decode('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', 'base64'),E'6.1.3-40302')
