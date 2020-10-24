namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateFactureProformaTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FactureProformas", "NumeroProforma", c => c.String());
            AddColumn("dbo.FactureProformas", "Del", c => c.Boolean(nullable: false));
            AddColumn("dbo.FactureProformas", "Date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FactureProformas", "Date");
            DropColumn("dbo.FactureProformas", "Del");
            DropColumn("dbo.FactureProformas", "NumeroProforma");
        }
    }
}
