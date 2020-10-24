namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateTypeColumnModelVerre : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ModelVerres", "VLSph", c => c.String());
            AlterColumn("dbo.ModelVerres", "VLCyl", c => c.String());
            AlterColumn("dbo.ModelVerres", "VPSph", c => c.String());
            AlterColumn("dbo.ModelVerres", "VPCyl", c => c.String());
            AlterColumn("dbo.ModelVerres", "Add", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ModelVerres", "Add", c => c.Single(nullable: false));
            AlterColumn("dbo.ModelVerres", "VPCyl", c => c.Single(nullable: false));
            AlterColumn("dbo.ModelVerres", "VPSph", c => c.Single(nullable: false));
            AlterColumn("dbo.ModelVerres", "VLCyl", c => c.Single(nullable: false));
            AlterColumn("dbo.ModelVerres", "VLSph", c => c.Single(nullable: false));
        }
    }
}
