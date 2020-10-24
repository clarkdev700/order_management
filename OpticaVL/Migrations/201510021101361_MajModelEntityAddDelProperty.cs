namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajModelEntityAddDelProperty : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ModelDivers", "Del", c => c.Boolean(nullable: false));
            AddColumn("dbo.ModelMontures", "Del", c => c.Boolean(nullable: false));
            AddColumn("dbo.ModelVerres", "Del", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ModelVerres", "Del");
            DropColumn("dbo.ModelMontures", "Del");
            DropColumn("dbo.ModelDivers", "Del");
        }
    }
}
