namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GestiondesVerre : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GammeVerrePuissances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Qte = c.Int(nullable: false),
                        Del = c.Boolean(nullable: false),
                        GammeVerreId = c.Int(nullable: false),
                        PuissanceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GammeVerres", t => t.GammeVerreId, cascadeDelete: true)
                .ForeignKey("dbo.Puissances", t => t.PuissanceId, cascadeDelete: true)
                .Index(t => t.GammeVerreId)
                .Index(t => t.PuissanceId);
            
            CreateTable(
                "dbo.Puissances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sph = c.String(),
                        Cycl = c.String(),
                        Axe = c.String(),
                        TypeVerre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.ModelVerres", "VLAxe", c => c.String());
            AlterColumn("dbo.ModelVerres", "VPAxe", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GammeVerrePuissances", "PuissanceId", "dbo.Puissances");
            DropForeignKey("dbo.GammeVerrePuissances", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.GammeVerrePuissances", new[] { "PuissanceId" });
            DropIndex("dbo.GammeVerrePuissances", new[] { "GammeVerreId" });
            AlterColumn("dbo.ModelVerres", "VPAxe", c => c.Single(nullable: false));
            AlterColumn("dbo.ModelVerres", "VLAxe", c => c.Single(nullable: false));
            DropTable("dbo.Puissances");
            DropTable("dbo.GammeVerrePuissances");
        }
    }
}
