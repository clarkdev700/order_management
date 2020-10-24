namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultipleTableUpdateProperties : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ventes", "MontantClient", c => c.Single(nullable: false));
            AddColumn("dbo.Ventes", "MontantAssurance", c => c.Single(nullable: false));
            AddColumn("dbo.Ventes", "PayeAssurance", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ventes", "PayeClient", c => c.Boolean(nullable: false));
            AddColumn("dbo.Ventes", "AssuranceId", c => c.Int());
            AlterColumn("dbo.GammeVerres", "Libelle", c => c.String(nullable: false));
            CreateIndex("dbo.Ventes", "AssuranceId");
            AddForeignKey("dbo.Ventes", "AssuranceId", "dbo.Assurances", "Id");
            DropColumn("dbo.Ventes", "Paye");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ventes", "Paye", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Ventes", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.Ventes", new[] { "AssuranceId" });
            AlterColumn("dbo.GammeVerres", "Libelle", c => c.String());
            DropColumn("dbo.Ventes", "AssuranceId");
            DropColumn("dbo.Ventes", "PayeClient");
            DropColumn("dbo.Ventes", "PayeAssurance");
            DropColumn("dbo.Ventes", "MontantAssurance");
            DropColumn("dbo.Ventes", "MontantClient");
        }
    }
}
