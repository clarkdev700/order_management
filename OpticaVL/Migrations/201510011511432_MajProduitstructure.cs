namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajProduitstructure : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ModelDivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                        Numero = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModelMontures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Reference = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ModelVerres",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nature = c.Int(nullable: false),
                        Format = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Puissance = c.Int(nullable: false),
                        Angle = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Commandes", "User", c => c.String());
            AddColumn("dbo.Produits", "ModelDiversId", c => c.Int());
            AddColumn("dbo.Produits", "ModelMontureId", c => c.Int());
            AddColumn("dbo.Produits", "ModelVerreId", c => c.Int());
            AddColumn("dbo.Payements", "User", c => c.String());
            CreateIndex("dbo.Produits", "ModelDiversId");
            CreateIndex("dbo.Produits", "ModelMontureId");
            CreateIndex("dbo.Produits", "ModelVerreId");
            AddForeignKey("dbo.Produits", "ModelDiversId", "dbo.ModelDivers", "Id");
            AddForeignKey("dbo.Produits", "ModelMontureId", "dbo.ModelMontures", "Id");
            AddForeignKey("dbo.Produits", "ModelVerreId", "dbo.ModelVerres", "Id");
            DropColumn("dbo.Produits", "RefProd");
            DropColumn("dbo.Produits", "Numero");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Produits", "Numero", c => c.Int());
            AddColumn("dbo.Produits", "RefProd", c => c.String(nullable: false));
            DropForeignKey("dbo.Produits", "ModelVerreId", "dbo.ModelVerres");
            DropForeignKey("dbo.Produits", "ModelMontureId", "dbo.ModelMontures");
            DropForeignKey("dbo.Produits", "ModelDiversId", "dbo.ModelDivers");
            DropIndex("dbo.Produits", new[] { "ModelVerreId" });
            DropIndex("dbo.Produits", new[] { "ModelMontureId" });
            DropIndex("dbo.Produits", new[] { "ModelDiversId" });
            DropColumn("dbo.Payements", "User");
            DropColumn("dbo.Produits", "ModelVerreId");
            DropColumn("dbo.Produits", "ModelMontureId");
            DropColumn("dbo.Produits", "ModelDiversId");
            DropColumn("dbo.Commandes", "User");
            DropTable("dbo.ModelVerres");
            DropTable("dbo.ModelMontures");
            DropTable("dbo.ModelDivers");
        }
    }
}
