namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editoutpatientencountertotals_add_domainid_healthfacilitytype : DbMigration
    {
        public override void Up()
        {
            AddColumn("OutPatientEncounterTotals", "DomainID", c => c.Int(nullable: false));
            AddColumn("OutPatientEncounterTotals", "DirectorateID", c => c.Int(nullable: false));
            AddColumn("OutPatientEncounterTotals", "HealthFacilityTypeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("OutPatientEncounterTotals", "HealthFacilityTypeID");
            DropColumn("OutPatientEncounterTotals", "DirectorateID");
            DropColumn("OutPatientEncounterTotals", "DomainID");
        }
    }
}
