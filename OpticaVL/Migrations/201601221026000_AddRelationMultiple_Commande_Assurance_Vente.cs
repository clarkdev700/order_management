namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRelationMultiple_Commande_Assurance_Vente : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Commandes", "AssuranceId", "dbo.Assurances");
            DropForeignKey("dbo.Ventes", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.Commandes", new[] { "AssuranceId" });
            DropIndex("dbo.Ventes", new[] { "AssuranceId" });
            CreateTable(
                "dbo.AssuranceCommandes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MontantAsssur = c.Single(nullable: false),
                        PayeAssur = c.Boolean(nullable: false),
                        CommandeId = c.Int(nullable: false),
                        AssuranceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assurances", t => t.AssuranceId, cascadeDelete: true)
                .ForeignKey("dbo.Commandes", t => t.CommandeId, cascadeDelete: true)
                .Index(t => t.CommandeId)
                .Index(t => t.AssuranceId);
            
            CreateTable(
                "dbo.AssuranceVentes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MontantAssur = c.Single(nullable: false),
                        PayeAssur = c.Boolean(nullable: false),
                        VenteId = c.Int(nullable: false),
                        AssuranceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Assurances", t => t.AssuranceId, cascadeDelete: true)
                .ForeignKey("dbo.Ventes", t => t.VenteId, cascadeDelete: true)
                .Index(t => t.VenteId)
                .Index(t => t.AssuranceId);
            
            DropColumn("dbo.Commandes", "PayeAssur");
            DropColumn("dbo.Commandes", "AssuranceId");
            DropColumn("dbo.Ventes", "PayeAssurance");
            DropColumn("dbo.Ventes", "AssuranceId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ventes", "AssuranceId", c => c.Int());
            AddColumn("dbo.Ventes", "PayeAssurance", c => c.Boolean(nullable: false));
            AddColumn("dbo.Commandes", "AssuranceId", c => c.Int());
            AddColumn("dbo.Commandes", "PayeAssur", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.AssuranceVentes", "VenteId", "dbo.Ventes");
            DropForeignKey("dbo.AssuranceVentes", "AssuranceId", "dbo.Assurances");
            DropForeignKey("dbo.AssuranceCommandes", "CommandeId", "dbo.Commandes");
            DropForeignKey("dbo.AssuranceCommandes", "AssuranceId", "dbo.Assurances");
            DropIndex("dbo.AssuranceVentes", new[] { "AssuranceId" });
            DropIndex("dbo.AssuranceVentes", new[] { "VenteId" });
            DropIndex("dbo.AssuranceCommandes", new[] { "AssuranceId" });
            DropIndex("dbo.AssuranceCommandes", new[] { "CommandeId" });
            DropTable("dbo.AssuranceVentes");
            DropTable("dbo.AssuranceCommandes");
            CreateIndex("dbo.Ventes", "AssuranceId");
            CreateIndex("dbo.Commandes", "AssuranceId");
            AddForeignKey("dbo.Ventes", "AssuranceId", "dbo.Assurances", "Id");
            AddForeignKey("dbo.Commandes", "AssuranceId", "dbo.Assurances", "Id");
        }
    }
}
