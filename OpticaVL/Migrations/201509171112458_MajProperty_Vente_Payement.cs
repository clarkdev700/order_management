namespace OpticaVL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MajProperty_Vente_Payement : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Payements", "RefPayement", c => c.String());
            AddColumn("dbo.Ventes", "Paye", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Ventes", "Paye");
            DropColumn("dbo.Payements", "RefPayement");
        }
    }
}
