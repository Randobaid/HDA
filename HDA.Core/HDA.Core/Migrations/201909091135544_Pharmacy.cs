namespace HDA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Pharmacy : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Prescriptions", "HealthFacilityID", "HealthFacilities");
            DropIndex("Prescriptions", new[] { "HealthFacilityID" });
            CreateTable(
                "Pharmacies",
                c => new
                    {
                        PharmacyID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PharmacyName = c.String(unicode: false),
                        HealthFacilityID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PharmacyID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .Index(t => t.HealthFacilityID);
            
            AddColumn("Prescriptions", "PharmacyID", c => c.Int(nullable: false));
            AddColumn("Prescriptions", "RequestedRefills", c => c.Int());
            CreateIndex("Prescriptions", "PharmacyID");
            AddForeignKey("Prescriptions", "PharmacyID", "Pharmacies", "PharmacyID", cascadeDelete: true);
            DropColumn("Prescriptions", "HealthFacilityID");
        }
        
        public override void Down()
        {
            AddColumn("Prescriptions", "HealthFacilityID", c => c.Int(nullable: false));
            DropForeignKey("Prescriptions", "PharmacyID", "Pharmacies");
            DropForeignKey("Pharmacies", "HealthFacilityID", "HealthFacilities");
            DropIndex("Prescriptions", new[] { "PharmacyID" });
            DropIndex("Pharmacies", new[] { "HealthFacilityID" });
            DropColumn("Prescriptions", "RequestedRefills");
            DropColumn("Prescriptions", "PharmacyID");
            DropTable("Pharmacies");
            CreateIndex("Prescriptions", "HealthFacilityID");
            AddForeignKey("Prescriptions", "HealthFacilityID", "HealthFacilities", "HealthFacilityID", cascadeDelete: true);
        }
    }
}
