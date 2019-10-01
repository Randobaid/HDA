namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_fk_healthfacilititytotals : DbMigration
    {
        public override void Up()
        {
            CreateIndex("OutPatientEncounterTotals", "HealthFacilityID");
            AddForeignKey("OutPatientEncounterTotals", "HealthFacilityID", "HealthFacilities", "HealthFacilityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("OutPatientEncounterTotals", "HealthFacilityID", "HealthFacilities");
            DropIndex("OutPatientEncounterTotals", new[] { "HealthFacilityID" });
        }
    }
}
