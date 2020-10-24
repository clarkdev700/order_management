namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MiseAjourBD : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LigneCommandes", "CommandeId", "dbo.Commandes");
            DropForeignKey("dbo.Produits", "ModelDiversId", "dbo.ModelDivers");
            DropForeignKey("dbo.Produits", "ModelMontureId", "dbo.ModelMontures");
            DropForeignKey("dbo.Produits", "ModelVerreId", "dbo.ModelVerres");
            DropForeignKey("dbo.LigneCommandes", "ProduitId", "dbo.Produits");
            DropIndex("dbo.LigneCommandes", new[] { "CommandeId" });
            DropIndex("dbo.LigneCommandes", new[] { "ProduitId" });
            DropIndex("dbo.Produits", new[] { "ModelDiversId" });
            DropIndex("dbo.Produits", new[] { "ModelMontureId" });
            DropIndex("dbo.Produits", new[] { "ModelVerreId" });
            AddColumn("dbo.Commandes", "Teinte", c => c.String());
            AddColumn("dbo.ModelVerres", "VLAxe", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "VLSph", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "VLCyl", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "VPAxe", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "VPSph", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "VPCyl", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "Add", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "Side", c => c.Int(nullable: false));
            AddColumn("dbo.ModelVerres", "Prix", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "Remise", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "Qte", c => c.Int(nullable: false));
            AddColumn("dbo.ModelVerres", "CommandeId", c => c.Int(nullable: false));
            CreateIndex("dbo.ModelVerres", "CommandeId");
            AddForeignKey("dbo.ModelVerres", "CommandeId", "dbo.Commandes", "Id", cascadeDelete: true);
            DropColumn("dbo.Commandes", "TypeCmd");
            DropColumn("dbo.Produits", "ModelDiversId");
            DropColumn("dbo.Produits", "ModelMontureId");
            DropColumn("dbo.Produits", "ModelVerreId");
            DropColumn("dbo.ModelVerres", "Format");
            DropColumn("dbo.ModelVerres", "Puissance");
            DropColumn("dbo.ModelVerres", "Angle");
            DropTable("dbo.LigneCommandes");
            DropTable("dbo.ModelDivers");
            DropTable("dbo.ModelMontures");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ModelMontures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        Del = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModelDivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        Numero = c.Int(nullable: false),
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
                        ProduitId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ModelVerres", "Angle", c => c.Int());
            AddColumn("dbo.ModelVerres", "Puissance", c => c.Single(nullable: false));
            AddColumn("dbo.ModelVerres", "Format", c => c.Int(nullable: false));
            AddColumn("dbo.Produits", "ModelVerreId", c => c.Int());
            AddColumn("dbo.Produits", "ModelMontureId", c => c.Int());
            AddColumn("dbo.Produits", "ModelDiversId", c => c.Int());
            AddColumn("dbo.Commandes", "TypeCmd", c => c.Int(nullable: false));
            DropForeignKey("dbo.ModelVerres", "CommandeId", "dbo.Commandes");
            DropIndex("dbo.ModelVerres", new[] { "CommandeId" });
            DropColumn("dbo.ModelVerres", "CommandeId");
            DropColumn("dbo.ModelVerres", "Qte");
            DropColumn("dbo.ModelVerres", "Remise");
            DropColumn("dbo.ModelVerres", "Prix");
            DropColumn("dbo.ModelVerres", "Side");
            DropColumn("dbo.ModelVerres", "Add");
            DropColumn("dbo.ModelVerres", "VPCyl");
            DropColumn("dbo.ModelVerres", "VPSph");
            DropColumn("dbo.ModelVerres", "VPAxe");
            DropColumn("dbo.ModelVerres", "VLCyl");
            DropColumn("dbo.ModelVerres", "VLSph");
            DropColumn("dbo.ModelVerres", "VLAxe");
            DropColumn("dbo.Commandes", "Teinte");
            CreateIndex("dbo.Produits", "ModelVerreId");
            CreateIndex("dbo.Produits", "ModelMontureId");
            CreateIndex("dbo.Produits", "ModelDiversId");
            CreateIndex("dbo.LigneCommandes", "ProduitId");
            CreateIndex("dbo.LigneCommandes", "CommandeId");
            AddForeignKey("dbo.LigneCommandes", "ProduitId", "dbo.Produits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Produits", "ModelVerreId", "dbo.ModelVerres", "Id");
            AddForeignKey("dbo.Produits", "ModelMontureId", "dbo.ModelMontures", "Id");
            AddForeignKey("dbo.Produits", "ModelDiversId", "dbo.ModelDivers", "Id");
            AddForeignKey("dbo.LigneCommandes", "CommandeId", "dbo.Commandes", "Id", cascadeDelete: true);
        }
    }
}
