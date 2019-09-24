namespace HDA.Core.Migrations.HDACore
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PatientDiagnoses : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "PatientDiagnoses",
                c => new
                    {
                        PatientDiagnosisID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PatientID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        DiagnosisDate = c.DateTime(nullable: false, precision: 0),
                        DiagnosisCodeID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatientDiagnosisID)
                .ForeignKey("dbo.DiagnosisCodes", t => t.DiagnosisCodeID, cascadeDelete: true)
                .ForeignKey("dbo.HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .ForeignKey("dbo.Patients", t => t.PatientID, cascadeDelete: true)
                .ForeignKey("dbo.Providers", t => t.ProviderID, cascadeDelete: true)
                .Index(t => t.PatientID)
                .Index(t => t.HealthFacilityID)
                .Index(t => t.DiagnosisCodeID)
                .Index(t => t.ProviderID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("PatientDiagnoses", "ProviderID", "dbo.Providers");
            DropForeignKey("PatientDiagnoses", "PatientID", "dbo.Patients");
            DropForeignKey("PatientDiagnoses", "HealthFacilityID", "dbo.HealthFacilities");
            DropForeignKey("PatientDiagnoses", "DiagnosisCodeID", "dbo.DiagnosisCodes");
            DropIndex("PatientDiagnoses", new[] { "ProviderID" });
            DropIndex("PatientDiagnoses", new[] { "DiagnosisCodeID" });
            DropIndex("PatientDiagnoses", new[] { "HealthFacilityID" });
            DropIndex("PatientDiagnoses", new[] { "PatientID" });
            DropTable("PatientDiagnoses");
        }
    }
}
