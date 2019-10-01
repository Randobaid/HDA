namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_InPatientEncounterTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("InPatientEncounterTotals", "DomainID", c => c.Int(nullable: false));
            AddColumn("InPatientEncounterTotals", "DirectorateID", c => c.Int(nullable: false));
            AddColumn("InPatientEncounterTotals", "HealthFacilityTypeID", c => c.Int(nullable: false));
            CreateIndex("InPatientEncounterTotals", "HealthFacilityID");
            AddForeignKey("InPatientEncounterTotals", "HealthFacilityID", "HealthFacilities", "HealthFacilityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("InPatientEncounterTotals", "HealthFacilityID", "HealthFacilities");
            DropIndex("InPatientEncounterTotals", new[] { "HealthFacilityID" });
            DropColumn("InPatientEncounterTotals", "HealthFacilityTypeID");
            DropColumn("InPatientEncounterTotals", "DirectorateID");
            DropColumn("InPatientEncounterTotals", "DomainID");
        }
    }
}
