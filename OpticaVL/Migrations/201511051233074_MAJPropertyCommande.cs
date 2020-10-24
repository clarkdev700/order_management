namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MAJPropertyCommande : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commandes", "Nature", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commandes", "Nature");
        }
    }
}
