namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajPropertyVenteLigneVente : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Ventes", "NomClient", c => c.String(nullable: false));
            AddColumn("dbo.Ventes", "PrenomClient", c => c.String());
            AddColumn("dbo.LigneVentes", "Rem", c => c.Single(nullable: false));
            AddColumn("dbo.LigneVentes", "Remdg", c => c.Single(nullable: false));
            DropColumn("dbo.Ventes", "IdentiteClient");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Ventes", "IdentiteClient", c => c.String());
            DropColumn("dbo.LigneVentes", "Remdg");
            DropColumn("dbo.LigneVentes", "Rem");
            DropColumn("dbo.Ventes", "PrenomClient");
            DropColumn("dbo.Ventes", "NomClient");
        }
    }
}
