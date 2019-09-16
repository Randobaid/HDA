namespace HDA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPharmacyIDToPrescriptionTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("PrescriptionTotals", "PharmacyID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PrescriptionTotals", "PharmacyID");
        }
    }
}
