namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Assurances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Nom = c.String(nullable: false),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Commandes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefCmd = c.String(nullable: false),
                        DateCmd = c.DateTime(nullable: false),
                        DateLvrCmd = c.DateTime(nullable: false),
                        DateRLvrCmd = c.DateTime(),
                        StatutCmd = c.Boolean(nullable: false),
                        Commentaire = c.String(),
                        MontantAssur = c.Single(nullable: false),
                        MontantClient = c.Single(nullable: false),
                        PayeAssur = c.Boolean(nullable: false),
                        PayeClient = c.Boolean(nullable: false),
                        TypeCmd = c.Int(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                        DateDel = c.DateTime(),
                        MotifDel = c.String(),
                        AssuranceId = c.Int(),
                        ClientId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assurances", t => t.AssuranceId)
                .ForeignKey("dbo.Clients", t => t.ClientId, cascadeDelete: true)
                .Index(t => t.AssuranceId)
                .Index(t => t.ClientId);
            
            CreateTable(
                "dbo.Clients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(nullable: false),
                        Prenom = c.String(nullable: false),
                        Contact = c.String(),
                        Contact2 = c.String(),
                        Adresse = c.String(),
                        Profession = c.String(),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LigneCommandes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QteCmd = c.Int(nullable: false),
                        Rem = c.Single(nullable: false),
                        RemDG = c.Single(nullable: false),
                        PrixCmd = c.Single(nullable: false),
                        Del = c.Boolean(nullable: false),
                        CommandeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commandes", t => t.CommandeId, cascadeDelete: true)
                .Index(t => t.CommandeId);
            
            CreateTable(
                "dbo.Payements",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MontantPaye = c.Single(nullable: false),
                        ModePaye = c.Int(nullable: false),
                        SourcePaye = c.Int(nullable: false),
                        DatePaye = c.DateTime(nullable: false),
                        MontantEncaisse = c.Boolean(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                        DateDel = c.DateTime(),
                        MotifDel = c.String(),
                        RefCmd = c.Int(),
                        RefVente = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commandes", t => t.RefCmd)
                .ForeignKey("dbo.Ventes", t => t.RefVente)
                .Index(t => t.RefCmd)
                .Index(t => t.RefVente);
            
            CreateTable(
                "dbo.Ventes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefVente = c.String(),
                        DateVente = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        IdentiteClient = c.String(),
                        Del = c.Boolean(nullable: false),
                        DateDel = c.DateTime(),
                        MotifDel = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.LigneVentes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QteVente = c.Int(nullable: false),
                        PrixVente = c.Single(nullable: false),
                        Del = c.Boolean(nullable: false),
                        VenteId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ventes", t => t.VenteId, cascadeDelete: true)
                .Index(t => t.VenteId);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Libelle = c.String(nullable: false),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Produits",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RefProd = c.String(nullable: false),
                        Designation = c.String(nullable: false),
                        Prix = c.Single(nullable: false),
                        QteSeuil = c.Int(nullable: false),
                        QteStock = c.Int(nullable: false),
                        Del = c.Boolean(nullable: false),
                        MarqueId = c.Int(),
                        CategorieId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.CategorieId, cascadeDelete: true)
                .ForeignKey("dbo.Marques", t => t.MarqueId)
                .Index(t => t.MarqueId)
                .Index(t => t.CategorieId);
            
            CreateTable(
                "dbo.Marques",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Libelle = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EntreeStocks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qte = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        DateEnreg = c.DateTime(nullable: false),
                        Del = c.Boolean(nullable: false),
                        DateDel = c.DateTime(),
                        MotifDel = c.String(),
                        FournisseurId = c.Int(nullable: false),
                        ProduitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Fournisseurs", t => t.FournisseurId, cascadeDelete: true)
                .ForeignKey("dbo.Produits", t => t.ProduitId, cascadeDelete: true)
                .Index(t => t.FournisseurId)
                .Index(t => t.ProduitId);
            
            CreateTable(
                "dbo.Fournisseurs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false),
                        Nom = c.String(nullable: false),
                        Contact = c.String(),
                        Contact2 = c.String(),
                        Email = c.String(),
                        Adresse = c.String(),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReceptionCommandes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DateReception = c.DateTime(nullable: false),
                        IdentiteReceptionnaire = c.String(),
                        Commentaire = c.String(),
                        DateEnreg = c.DateTime(nullable: false),
                        CommandeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commandes", t => t.CommandeId, cascadeDelete: true)
                .Index(t => t.CommandeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReceptionCommandes", "CommandeId", "dbo.Commandes");
            DropForeignKey("dbo.EntreeStocks", "ProduitId", "dbo.Produits");
            DropForeignKey("dbo.EntreeStocks", "FournisseurId", "dbo.Fournisseurs");
            DropForeignKey("dbo.Produits", "MarqueId", "dbo.Marques");
            DropForeignKey("dbo.Produits", "CategorieId", "dbo.Categories");
            DropForeignKey("dbo.Payements", "RefVente", "dbo.Ventes");
            DropForeignKey("dbo.LigneVentes", "VenteId", "dbo.Ventes");
            DropForeignKey("dbo.Payements", "RefCmd", "dbo.Commandes");
            DropForeignKey("dbo.LigneCommandes", "CommandeId", "dbo.Commandes");
            DropForeignKey("dbo.Commandes", "ClientId", "dbo.Clients");
            DropForeignKey("dbo.Commandes", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.ReceptionCommandes", new[] { "CommandeId" });
            DropIndex("dbo.EntreeStocks", new[] { "ProduitId" });
            DropIndex("dbo.EntreeStocks", new[] { "FournisseurId" });
            DropIndex("dbo.Produits", new[] { "CategorieId" });
            DropIndex("dbo.Produits", new[] { "MarqueId" });
            DropIndex("dbo.LigneVentes", new[] { "VenteId" });
            DropIndex("dbo.Payements", new[] { "RefVente" });
            DropIndex("dbo.Payements", new[] { "RefCmd" });
            DropIndex("dbo.LigneCommandes", new[] { "CommandeId" });
            DropIndex("dbo.Commandes", new[] { "ClientId" });
            DropIndex("dbo.Commandes", new[] { "AssuranceId" });
            DropTable("dbo.ReceptionCommandes");
            DropTable("dbo.Fournisseurs");
            DropTable("dbo.EntreeStocks");
            DropTable("dbo.Marques");
            DropTable("dbo.Produits");
            DropTable("dbo.Categories");
            DropTable("dbo.LigneVentes");
            DropTable("dbo.Ventes");
            DropTable("dbo.Payements");
            DropTable("dbo.LigneCommandes");
            DropTable("dbo.Clients");
            DropTable("dbo.Commandes");
            DropTable("dbo.Assurances");
        }
    }
}
