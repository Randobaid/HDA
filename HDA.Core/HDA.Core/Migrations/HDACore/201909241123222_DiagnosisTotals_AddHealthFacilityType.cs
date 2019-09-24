namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiagnosisTotals_AddHealthFacilityType : DbMigration
    {
        public override void Up()
        {
            AddColumn("DiagnosisTotals", "HealthFacilityTypeID", c => c.Int(nullable: false));
            CreateIndex("DiagnosisTotals", "HealthFacilityTypeID");
            AddForeignKey("DiagnosisTotals", "HealthFacilityTypeID", "HealthFacilityTypes", "HealthFacilityTypeID", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("DiagnosisTotals", "HealthFacilityTypeID", "HealthFacilityTypes");
            DropIndex("DiagnosisTotals", new[] { "HealthFacilityTypeID" });
            DropColumn("DiagnosisTotals", "HealthFacilityTypeID");
        }
    }
}
