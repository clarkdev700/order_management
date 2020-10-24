namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProdPropertyAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Produits", "Numero", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Produits", "Numero");
        }
    }
}
