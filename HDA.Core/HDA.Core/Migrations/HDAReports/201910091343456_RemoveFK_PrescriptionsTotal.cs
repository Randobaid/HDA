namespace HDA.Core.Migrations.HDAReports
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveFK_PrescriptionsTotal : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("PrescriptionTotals", "HealthFacilityID", "HealthFacilities");
            DropIndex("PrescriptionTotals", new[] { "HealthFacilityID" });
        }
        
        public override void Down()
        {
            CreateIndex("PrescriptionTotals", "HealthFacilityID");
            AddForeignKey("PrescriptionTotals", "HealthFacilityID", "HealthFacilities", "HealthFacilityID", cascadeDelete: true);
        }
    }
}
