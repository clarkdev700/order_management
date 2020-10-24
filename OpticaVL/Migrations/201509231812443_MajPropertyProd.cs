namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajPropertyProd : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Produits", "Numero", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Produits", "Numero", c => c.Int(nullable: false));
        }
    }
}
