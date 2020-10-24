namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTableLigneCommande : DbMigration
    {
        public override void Up()
        {
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Commandes", t => t.CommandeId, cascadeDelete: true)
                .ForeignKey("dbo.Produits", t => t.ProduitId, cascadeDelete: true)
                .Index(t => t.CommandeId)
                .Index(t => t.ProduitId);
            
            AddColumn("dbo.Commandes", "Produit_Id", c => c.Int());
            AddColumn("dbo.ModelVerres", "RemiseDG", c => c.Single(nullable: false));
            CreateIndex("dbo.Commandes", "Produit_Id");
            AddForeignKey("dbo.Commandes", "Produit_Id", "dbo.Produits", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Commandes", "Produit_Id", "dbo.Produits");
            DropForeignKey("dbo.LigneCommandes", "ProduitId", "dbo.Produits");
            DropForeignKey("dbo.LigneCommandes", "CommandeId", "dbo.Commandes");
            DropIndex("dbo.LigneCommandes", new[] { "ProduitId" });
            DropIndex("dbo.LigneCommandes", new[] { "CommandeId" });
            DropIndex("dbo.Commandes", new[] { "Produit_Id" });
            DropColumn("dbo.ModelVerres", "RemiseDG");
            DropColumn("dbo.Commandes", "Produit_Id");
            DropTable("dbo.LigneCommandes");
        }
    }
}
