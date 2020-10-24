namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesClientProduit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Clients", "Civilite", c => c.Int(nullable: false));
            AddColumn("dbo.Produits", "RefProd", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produits", "RefProd");
            DropColumn("dbo.Clients", "Civilite");
        }
    }
}
