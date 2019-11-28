namespace HDA.Core.Migrations.HDAReports
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Countries",
                c => new
                    {
                        CountryID = c.Int(nullable: false, identity: true),
                        CountryNameEn = c.String(unicode: false),
                        CountryNameAr = c.String(unicode: false),
                        CountryCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CountryID);
            
            CreateTable(
                "DiagnosisCodes",
                c => new
                    {
                        DiagnosisCodeID = c.Int(nullable: false, identity: true),
                        Code = c.String(unicode: false),
                        DiagnosisCodingSystemID = c.Int(nullable: false),
                        DiagnosisNameEn = c.String(unicode: false),
                        DiagnosisNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DiagnosisCodeID)
                .ForeignKey("DiagnosisCodingSystems", t => t.DiagnosisCodingSystemID, cascadeDelete: true)
                .Index(t => t.DiagnosisCodingSystemID);
            
            CreateTable(
                "DiagnosisCodingSystems",
                c => new
                    {
                        DiagnosisCodingSystemID = c.Int(nullable: false, identity: true),
                        CodingSystemName = c.String(unicode: false),
                        CodingSystemVersion = c.String(unicode: false),
                        CodingSystemEffectiveDate = c.DateTime(nullable: false, precision: 0),
                    })
                .PrimaryKey(t => t.DiagnosisCodingSystemID);
            
            CreateTable(
                "DiagnosisTotals",
                c => new
                    {
                        DiagnosisTotalID = c.Int(nullable: false, identity: true),
                        DomainID = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                        HealthFacilityTypeID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        DiagnosisCodeID = c.Int(nullable: false),
                        AgeGroup = c.String(unicode: false),
                        GenderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DiagnosisTotalID)
                .ForeignKey("DiagnosisCodes", t => t.DiagnosisCodeID, cascadeDelete: true)
                .ForeignKey("Directorates", t => t.DirectorateID, cascadeDelete: true)
                .ForeignKey("Domains", t => t.DomainID, cascadeDelete: true)
                .ForeignKey("Genders", t => t.GenderID, cascadeDelete: true)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .ForeignKey("HealthFacilityTypes", t => t.HealthFacilityTypeID, cascadeDelete: true)
                .Index(t => t.DomainID)
                .Index(t => t.DirectorateID)
                .Index(t => t.HealthFacilityTypeID)
                .Index(t => t.HealthFacilityID)
                .Index(t => t.DiagnosisCodeID)
                .Index(t => t.GenderID);
            
            CreateTable(
                "Directorates",
                c => new
                    {
                        DirectorateID = c.Int(nullable: false, identity: true),
                        DirectorateNameEn = c.String(unicode: false),
                        DirectorateNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DirectorateID);
            
            CreateTable(
                "Domains",
                c => new
                    {
                        DomainID = c.Int(nullable: false, identity: true),
                        DomainCode = c.String(unicode: false),
                        DomainNameEn = c.String(unicode: false),
                        DomainNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DomainID);
            
            CreateTable(
                "Genders",
                c => new
                    {
                        GenderID = c.Int(nullable: false, identity: true),
                        GenderEn = c.String(unicode: false),
                        GenderAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.GenderID);
            
            CreateTable(
                "HealthFacilities",
                c => new
                    {
                        HealthFacilityID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        FacilityShortName = c.String(unicode: false),
                        HealthFacilityNameEn = c.String(unicode: false),
                        HealthFacilityNameAr = c.String(unicode: false),
                        GovernorateID = c.Int(nullable: false),
                        DomainID = c.Int(),
                        HealthFacilityTypeID = c.Int(),
                        EstimatedClinics = c.Int(nullable: false),
                        EstimatedBeds = c.Int(nullable: false),
                        EstimatedOperatingRooms = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                        EHRActivationYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HealthFacilityID)
                .ForeignKey("Directorates", t => t.DirectorateID, cascadeDelete: true)
                .ForeignKey("Domains", t => t.DomainID)
                .ForeignKey("Governorates", t => t.GovernorateID, cascadeDelete: true)
                .ForeignKey("HealthFacilityTypes", t => t.HealthFacilityTypeID)
                .Index(t => t.SourceID)
                .Index(t => t.GovernorateID)
                .Index(t => t.DomainID)
                .Index(t => t.HealthFacilityTypeID)
                .Index(t => t.DirectorateID);
            
            CreateTable(
                "Governorates",
                c => new
                    {
                        GovernorateID = c.Int(nullable: false, identity: true),
                        GovernorateCode = c.Int(nullable: false),
                        GovernorateNameEn = c.String(unicode: false),
                        GovernorateNameAr = c.String(unicode: false),
                        RegionID = c.Int(nullable: false),
                        ShapeFile = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.GovernorateID)
                .ForeignKey("Regions", t => t.RegionID, cascadeDelete: true)
                .Index(t => t.RegionID);
            
            CreateTable(
                "Regions",
                c => new
                    {
                        RegionID = c.Int(nullable: false, identity: true),
                        RegionCode = c.Int(nullable: false),
                        RegionNameEn = c.String(unicode: false),
                        RegionNameAr = c.String(unicode: false),
                        CountryID = c.Int(nullable: false),
                        ShapeFile = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.RegionID)
                .ForeignKey("Countries", t => t.CountryID, cascadeDelete: true)
                .Index(t => t.CountryID);
            
            CreateTable(
                "HealthFacilityTypes",
                c => new
                    {
                        HealthFacilityTypeID = c.Int(nullable: false, identity: true),
                        HealthFacilityTypeCode = c.String(unicode: false),
                        HealthFacilityTypeNameEn = c.String(unicode: false),
                        HealthFacilityTypeNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.HealthFacilityTypeID);
            
            CreateTable(
                "DrugClasses",
                c => new
                    {
                        DrugClassID = c.Int(nullable: false, identity: true),
                        DrugParentClassID = c.Int(),
                        DrugClassNameEn = c.String(unicode: false),
                        DrugClassNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DrugClassID);
            
            CreateTable(
                "Drugs",
                c => new
                    {
                        DrugID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        DrugNameEn = c.String(unicode: false),
                        DrugNameAr = c.String(unicode: false),
                        DrugGenericName = c.String(unicode: false),
                        NormalAmountToOrder = c.Int(),
                        DrugEstimatedPrice = c.Int(),
                        DrugClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DrugID)
                .ForeignKey("DrugClasses", t => t.DrugClassID, cascadeDelete: true)
                .Index(t => t.DrugClassID);
            
            CreateTable(
                "Indicators",
                c => new
                    {
                        IndicatorID = c.Int(nullable: false, identity: true),
                        IndicatorShortName = c.String(unicode: false),
                        IndicatorNameEn = c.String(unicode: false),
                        IndicatorNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.IndicatorID);
            
            CreateTable(
                "InPatientEncounterTotals",
                c => new
                    {
                        InPatientEncounterTotalID = c.Int(nullable: false, identity: true),
                        DomainID = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                        HealthFacilityTypeID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        LOSGroup = c.String(unicode: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InPatientEncounterTotalID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .Index(t => t.HealthFacilityID);
            
            CreateTable(
                "OutPatientEncounterTotals",
                c => new
                    {
                        OutPatientEncounterTotalID = c.Int(nullable: false, identity: true),
                        DomainID = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                        HealthFacilityTypeID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OutPatientEncounterTotalID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .Index(t => t.HealthFacilityID);
            
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
            
            CreateTable(
                "PrescriptionTotals",
                c => new
                    {
                        PrescriptionTotalID = c.Int(nullable: false, identity: true),
                        DomainID = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                        HealthFacilityTypeID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        PharmacyID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DrugClassID = c.Int(nullable: false),
                        DrugId = c.Int(nullable: false),
                        TotalPrescriptions = c.Int(nullable: false),
                        TotalQuantity = c.Int(nullable: false),
                        TotalRefillQuantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrescriptionTotalID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .Index(t => t.HealthFacilityID);
            
            CreateTable(
                "Providers",
                c => new
                    {
                        ProviderID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        MasterProviderIndex = c.Int(),
                        JobID = c.String(unicode: false),
                        ProviderType = c.String(unicode: false),
                        NationalID = c.Int(),
                        ProviderNameEn = c.String(unicode: false),
                        ProviderNameAr = c.String(unicode: false),
                        DomainID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProviderID)
                .ForeignKey("Domains", t => t.DomainID, cascadeDelete: true)
                .Index(t => t.SourceID)
                .Index(t => t.DomainID);
            
            CreateTable(
                "SurgeryTotals",
                c => new
                    {
                        SurgeryTotalID = c.Int(nullable: false, identity: true),
                        DomainID = c.Int(nullable: false),
                        DirectorateID = c.Int(nullable: false),
                        HealthFacilityTypeID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        SurgerySeverityID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurgeryTotalID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .Index(t => t.HealthFacilityID);
            
            CreateTable(
                "Targets",
                c => new
                    {
                        TargetID = c.Int(nullable: false, identity: true),
                        IndicatorID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(),
                        ProviderID = c.Int(),
                        DomainID = c.Int(),
                        DirectorateID = c.Int(),
                        EffectiveDate = c.DateTime(nullable: false, precision: 0),
                        Value = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.TargetID)
                .ForeignKey("Directorates", t => t.DirectorateID)
                .ForeignKey("Domains", t => t.DomainID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID)
                .ForeignKey("Indicators", t => t.IndicatorID, cascadeDelete: true)
                .ForeignKey("Providers", t => t.ProviderID)
                .Index(t => t.IndicatorID)
                .Index(t => t.HealthFacilityID)
                .Index(t => t.ProviderID)
                .Index(t => t.DomainID)
                .Index(t => t.DirectorateID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Targets", "ProviderID", "Providers");
            DropForeignKey("Targets", "IndicatorID", "Indicators");
            DropForeignKey("Targets", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Targets", "DomainID", "Domains");
            DropForeignKey("Targets", "DirectorateID", "Directorates");
            DropForeignKey("SurgeryTotals", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Providers", "DomainID", "Domains");
            DropForeignKey("PrescriptionTotals", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Pharmacies", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("OutPatientEncounterTotals", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("InPatientEncounterTotals", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Drugs", "DrugClassID", "DrugClasses");
            DropForeignKey("DiagnosisTotals", "HealthFacilityTypeID", "HealthFacilityTypes");
            DropForeignKey("DiagnosisTotals", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("HealthFacilities", "HealthFacilityTypeID", "HealthFacilityTypes");
            DropForeignKey("HealthFacilities", "GovernorateID", "Governorates");
            DropForeignKey("Governorates", "RegionID", "Regions");
            DropForeignKey("Regions", "CountryID", "Countries");
            DropForeignKey("HealthFacilities", "DomainID", "Domains");
            DropForeignKey("HealthFacilities", "DirectorateID", "Directorates");
            DropForeignKey("DiagnosisTotals", "GenderID", "Genders");
            DropForeignKey("DiagnosisTotals", "DomainID", "Domains");
            DropForeignKey("DiagnosisTotals", "DirectorateID", "Directorates");
            DropForeignKey("DiagnosisTotals", "DiagnosisCodeID", "DiagnosisCodes");
            DropForeignKey("DiagnosisCodes", "DiagnosisCodingSystemID", "DiagnosisCodingSystems");
            DropIndex("Targets", new[] { "DirectorateID" });
            DropIndex("Targets", new[] { "DomainID" });
            DropIndex("Targets", new[] { "ProviderID" });
            DropIndex("Targets", new[] { "HealthFacilityID" });
            DropIndex("Targets", new[] { "IndicatorID" });
            DropIndex("SurgeryTotals", new[] { "HealthFacilityID" });
            DropIndex("Providers", new[] { "DomainID" });
            DropIndex("Providers", new[] { "SourceID" });
            DropIndex("PrescriptionTotals", new[] { "HealthFacilityID" });
            DropIndex("Pharmacies", new[] { "HealthFacilityID" });
            DropIndex("OutPatientEncounterTotals", new[] { "HealthFacilityID" });
            DropIndex("InPatientEncounterTotals", new[] { "HealthFacilityID" });
            DropIndex("Drugs", new[] { "DrugClassID" });
            DropIndex("Regions", new[] { "CountryID" });
            DropIndex("Governorates", new[] { "RegionID" });
            DropIndex("HealthFacilities", new[] { "DirectorateID" });
            DropIndex("HealthFacilities", new[] { "HealthFacilityTypeID" });
            DropIndex("HealthFacilities", new[] { "DomainID" });
            DropIndex("HealthFacilities", new[] { "GovernorateID" });
            DropIndex("HealthFacilities", new[] { "SourceID" });
            DropIndex("DiagnosisTotals", new[] { "GenderID" });
            DropIndex("DiagnosisTotals", new[] { "DiagnosisCodeID" });
            DropIndex("DiagnosisTotals", new[] { "HealthFacilityID" });
            DropIndex("DiagnosisTotals", new[] { "HealthFacilityTypeID" });
            DropIndex("DiagnosisTotals", new[] { "DirectorateID" });
            DropIndex("DiagnosisTotals", new[] { "DomainID" });
            DropIndex("DiagnosisCodes", new[] { "DiagnosisCodingSystemID" });
            DropTable("Targets");
            DropTable("SurgeryTotals");
            DropTable("Providers");
            DropTable("PrescriptionTotals");
            DropTable("Pharmacies");
            DropTable("OutPatientEncounterTotals");
            DropTable("InPatientEncounterTotals");
            DropTable("Indicators");
            DropTable("Drugs");
            DropTable("DrugClasses");
            DropTable("HealthFacilityTypes");
            DropTable("Regions");
            DropTable("Governorates");
            DropTable("HealthFacilities");
            DropTable("Genders");
            DropTable("Domains");
            DropTable("Directorates");
            DropTable("DiagnosisTotals");
            DropTable("DiagnosisCodingSystems");
            DropTable("DiagnosisCodes");
            DropTable("Countries");
        }
    }
}
