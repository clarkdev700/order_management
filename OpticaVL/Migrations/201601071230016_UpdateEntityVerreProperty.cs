namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateEntityVerreProperty : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.MVerres", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.MVerres", new[] { "GammeVerreId" });
            AlterColumn("dbo.MVerres", "GammeVerreId", c => c.Int());
            CreateIndex("dbo.MVerres", "GammeVerreId");
            AddForeignKey("dbo.MVerres", "GammeVerreId", "dbo.GammeVerres", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MVerres", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.MVerres", new[] { "GammeVerreId" });
            AlterColumn("dbo.MVerres", "GammeVerreId", c => c.Int(nullable: false));
            CreateIndex("dbo.MVerres", "GammeVerreId");
            AddForeignKey("dbo.MVerres", "GammeVerreId", "dbo.GammeVerres", "Id", cascadeDelete: true);
        }
    }
}
