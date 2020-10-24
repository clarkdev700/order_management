namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MissRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LigneCommandes", "ProduitId", c => c.Int(nullable: false));
            AddColumn("dbo.LigneVentes", "ProduitId", c => c.Int(nullable: false));
            CreateIndex("dbo.LigneCommandes", "ProduitId");
            CreateIndex("dbo.LigneVentes", "ProduitId");
            AddForeignKey("dbo.LigneCommandes", "ProduitId", "dbo.Produits", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LigneVentes", "ProduitId", "dbo.Produits", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LigneVentes", "ProduitId", "dbo.Produits");
            DropForeignKey("dbo.LigneCommandes", "ProduitId", "dbo.Produits");
            DropIndex("dbo.LigneVentes", new[] { "ProduitId" });
            DropIndex("dbo.LigneCommandes", new[] { "ProduitId" });
            DropColumn("dbo.LigneVentes", "ProduitId");
            DropColumn("dbo.LigneCommandes", "ProduitId");
        }
    }
}
