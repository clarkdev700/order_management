namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajBDAddTableGammeVerre : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.GammeVerres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Libelle = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ModelVerres", "GammeVerreId", c => c.Int(nullable: false));
            AlterColumn("dbo.ModelVerres", "Puissance", c => c.Single(nullable: false));
            AlterColumn("dbo.ModelVerres", "Angle", c => c.Int());
            CreateIndex("dbo.ModelVerres", "GammeVerreId");
            AddForeignKey("dbo.ModelVerres", "GammeVerreId", "dbo.GammeVerres", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ModelVerres", "GammeVerreId", "dbo.GammeVerres");
            DropIndex("dbo.ModelVerres", new[] { "GammeVerreId" });
            AlterColumn("dbo.ModelVerres", "Angle", c => c.Int(nullable: false));
            AlterColumn("dbo.ModelVerres", "Puissance", c => c.Int(nullable: false));
            DropColumn("dbo.ModelVerres", "GammeVerreId");
            DropTable("dbo.GammeVerres");
        }
    }
}
