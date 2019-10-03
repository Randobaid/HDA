namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editPrescriptionTotal : DbMigration
    {
        public override void Up()
        {
            CreateIndex("PrescriptionTotals", "HealthFacilityID");
            AddForeignKey("PrescriptionTotals", "HealthFacilityID", "HealthFacilities", "HealthFacilityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("PrescriptionTotals", "HealthFacilityID", "HealthFacilities");
            DropIndex("PrescriptionTotals", new[] { "HealthFacilityID" });
        }
    }
}
