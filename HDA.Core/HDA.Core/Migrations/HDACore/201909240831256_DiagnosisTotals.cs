namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiagnosisTotals : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DiagnosisTotals",
                c => new
                    {
                        DiagnosisTotalID = c.Int(nullable: false, identity: true),
                        DomainLookupID = c.Int(nullable: false),
                        DirectorateLookupID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        DiagnosisCodeID = c.Int(nullable: false),
                        AgeGroup = c.String(unicode: false),
                        GenderLookupID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiagnosisTotalID)
                .ForeignKey("DiagnosisCodes", t => t.DiagnosisCodeID, cascadeDelete: true)
                .ForeignKey("DirectorateLookups", t => t.DirectorateLookupID, cascadeDelete: true)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("GenderLookups", t => t.GenderLookupID, cascadeDelete: true)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .Index(t => t.DomainLookupID)
                .Index(t => t.DirectorateLookupID)
                .Index(t => t.HealthFacilityID)
                .Index(t => t.DiagnosisCodeID)
                .Index(t => t.GenderLookupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DiagnosisTotals", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("DiagnosisTotals", "GenderLookupID", "GenderLookups");
            DropForeignKey("DiagnosisTotals", "DomainLookupID", "DomainLookups");
            DropForeignKey("DiagnosisTotals", "DirectorateLookupID", "DirectorateLookups");
            DropForeignKey("DiagnosisTotals", "DiagnosisCodeID", "DiagnosisCodes");
            DropIndex("DiagnosisTotals", new[] { "GenderLookupID" });
            DropIndex("DiagnosisTotals", new[] { "DiagnosisCodeID" });
            DropIndex("DiagnosisTotals", new[] { "HealthFacilityID" });
            DropIndex("DiagnosisTotals", new[] { "DirectorateLookupID" });
            DropIndex("DiagnosisTotals", new[] { "DomainLookupID" });
            DropTable("DiagnosisTotals");
        }
    }
}
