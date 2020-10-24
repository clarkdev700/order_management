namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeGammeVerreContrainteOnModelVerres : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ModelVerres", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.ModelVerres", new[] { "GammeVerreId" });
            AlterColumn("dbo.ModelVerres", "GammeVerreId", c => c.Int());
            CreateIndex("dbo.ModelVerres", "GammeVerreId");
            AddForeignKey("dbo.ModelVerres", "GammeVerreId", "dbo.GammeVerres", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModelVerres", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.ModelVerres", new[] { "GammeVerreId" });
            AlterColumn("dbo.ModelVerres", "GammeVerreId", c => c.Int(nullable: false));
            CreateIndex("dbo.ModelVerres", "GammeVerreId");
            AddForeignKey("dbo.ModelVerres", "GammeVerreId", "dbo.GammeVerres", "Id", cascadeDelete: true);
        }
    }
}
