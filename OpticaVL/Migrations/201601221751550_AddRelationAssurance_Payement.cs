namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationAssurance_Payement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payements", "AssuranceId", c => c.Int());
            CreateIndex("dbo.Payements", "AssuranceId");
            AddForeignKey("dbo.Payements", "AssuranceId", "dbo.Assurances", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Payements", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.Payements", new[] { "AssuranceId" });
            DropColumn("dbo.Payements", "AssuranceId");
        }
    }
}
