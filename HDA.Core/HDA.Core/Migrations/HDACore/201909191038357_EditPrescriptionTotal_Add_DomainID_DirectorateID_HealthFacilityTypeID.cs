namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditPrescriptionTotal_Add_DomainID_DirectorateID_HealthFacilityTypeID : DbMigration
    {
        public override void Up()
        {
            AddColumn("PrescriptionTotals", "DomainID", c => c.Int(nullable: false));
            AddColumn("PrescriptionTotals", "DirectorateID", c => c.Int(nullable: false));
            AddColumn("PrescriptionTotals", "HealthFacilityTypeID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("PrescriptionTotals", "HealthFacilityTypeID");
            DropColumn("PrescriptionTotals", "DirectorateID");
            DropColumn("PrescriptionTotals", "DomainID");
        }
    }
}
