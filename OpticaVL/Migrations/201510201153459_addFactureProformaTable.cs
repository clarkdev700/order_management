namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFactureProformaTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FactureProformas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nom = c.String(),
                        Prenom = c.String(),
                        ReferenceRecu = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FactureProformas");
        }
    }
}
