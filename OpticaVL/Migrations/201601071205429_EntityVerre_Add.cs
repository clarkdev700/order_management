namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityVerre_Add : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.GammeVerrePuissances", "GammeVerreId", "dbo.GammeVerres");
            DropForeignKey("dbo.GammeVerrePuissances", "PuissanceId", "dbo.Puissances");
            DropIndex("dbo.GammeVerrePuissances", new[] { "GammeVerreId" });
            DropIndex("dbo.GammeVerrePuissances", new[] { "PuissanceId" });
            CreateTable(
                "dbo.MVerres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TypeVerre = c.Int(nullable: false),
                        Sph = c.String(),
                        Cyl = c.String(),
                        Add = c.String(),
                        Side = c.Int(),
                        Qte = c.Int(nullable: false),
                        GammeVerreId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GammeVerres", t => t.GammeVerreId, cascadeDelete: true)
                .Index(t => t.GammeVerreId);
            
            DropTable("dbo.GammeVerrePuissances");
            DropTable("dbo.Puissances");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Puissances",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Sph = c.String(),
                        Cycl = c.String(),
                        Axe = c.String(),
                        Addition = c.String(),
                        TypeVerre = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.MVerres", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.MVerres", new[] { "GammeVerreId" });
            DropTable("dbo.MVerres");
            CreateIndex("dbo.GammeVerrePuissances", "PuissanceId");
            CreateIndex("dbo.GammeVerrePuissances", "GammeVerreId");
            AddForeignKey("dbo.GammeVerrePuissances", "PuissanceId", "dbo.Puissances", "Id", cascadeDelete: true);
            AddForeignKey("dbo.GammeVerrePuissances", "GammeVerreId", "dbo.GammeVerres", "Id", cascadeDelete: true);
        }
    }
}
