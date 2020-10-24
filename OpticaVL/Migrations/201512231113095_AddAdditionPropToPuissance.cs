namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdditionPropToPuissance : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Puissances", "Addition", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Puissances", "Addition");
        }
    }
}
