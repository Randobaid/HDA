namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PrescriptionTotal_AddQuantityTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("PrescriptionTotals", "TotalPrescriptions", c => c.Int(nullable: false));
            AddColumn("PrescriptionTotals", "TotalQuantity", c => c.Int(nullable: false));
            DropColumn("PrescriptionTotals", "Total");
        }
        
        public override void Down()
        {
            AddColumn("PrescriptionTotals", "Total", c => c.Int(nullable: false));
            DropColumn("PrescriptionTotals", "TotalQuantity");
            DropColumn("PrescriptionTotals", "TotalPrescriptions");
        }
    }
}
