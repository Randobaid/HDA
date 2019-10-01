namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_SurgeryTotal : DbMigration
    {
        public override void Up()
        {
            AddColumn("SurgeryTotals", "DomainID", c => c.Int(nullable: false));
            AddColumn("SurgeryTotals", "DirectorateID", c => c.Int(nullable: false));
            AddColumn("SurgeryTotals", "HealthFacilityTypeID", c => c.Int(nullable: false));
            CreateIndex("SurgeryTotals", "HealthFacilityID");
            AddForeignKey("SurgeryTotals", "HealthFacilityID", "HealthFacilities", "HealthFacilityID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("SurgeryTotals", "HealthFacilityID", "HealthFacilities");
            DropIndex("SurgeryTotals", new[] { "HealthFacilityID" });
            DropColumn("SurgeryTotals", "HealthFacilityTypeID");
            DropColumn("SurgeryTotals", "DirectorateID");
            DropColumn("SurgeryTotals", "DomainID");
        }
    }
}
