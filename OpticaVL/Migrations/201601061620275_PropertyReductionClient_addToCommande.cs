namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyReductionClient_addToCommande : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Commandes", "ReductionClient", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Commandes", "ReductionClient");
        }
    }
}
