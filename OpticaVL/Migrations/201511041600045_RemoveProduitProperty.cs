namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveProduitProperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Commandes", "Produit_Id", "dbo.Produits");
            DropIndex("dbo.Commandes", new[] { "Produit_Id" });
            DropColumn("dbo.Commandes", "Produit_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Commandes", "Produit_Id", c => c.Int());
            CreateIndex("dbo.Commandes", "Produit_Id");
            AddForeignKey("dbo.Commandes", "Produit_Id", "dbo.Produits", "Id");
        }
    }
}
