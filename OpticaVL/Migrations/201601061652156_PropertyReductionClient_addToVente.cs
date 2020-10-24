namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PropertyReductionClient_addToVente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ventes", "ReductionClient", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ventes", "ReductionClient");
        }
    }
}
