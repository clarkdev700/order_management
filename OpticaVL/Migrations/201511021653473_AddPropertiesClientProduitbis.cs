namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPropertiesClientProduitbis : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Clients", "Nom", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Clients", "Nom", c => c.String());
        }
    }
}
