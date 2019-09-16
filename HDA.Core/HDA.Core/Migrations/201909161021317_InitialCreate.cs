namespace HDA.Core.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "AppointmentStatusLookups",
                c => new
                    {
                        AppointmentStatusLookupID = c.Int(nullable: false, identity: true),
                        AppointmentStatusEn = c.String(unicode: false),
                        AppointmentStatusAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.AppointmentStatusLookupID);
            
            CreateTable(
                "AppointmentTypeLookups",
                c => new
                    {
                        AppointmentTypeLookupID = c.Int(nullable: false, identity: true),
                        AppointmentTypeNameEn = c.String(unicode: false),
                        AppointmentTypeNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.AppointmentTypeLookupID);
            
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
                "DepartmentLookups",
                c => new
                    {
                        DepartmentLookupID = c.Int(nullable: false, identity: true),
                        DepartmentCode = c.String(unicode: false),
                        DepartmentNameEn = c.String(unicode: false),
                        DepartmentNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DepartmentLookupID);
            
            CreateTable(
                "DiagnosisLookups",
                c => new
                    {
                        DiagnosisLookupID = c.Int(nullable: false, identity: true),
                        DiagnosisCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DiagnosisLookupID);
            
            CreateTable(
                "DirectorateLookups",
                c => new
                    {
                        DirectorateLookupID = c.Int(nullable: false, identity: true),
                        DirectorateNameEn = c.String(unicode: false),
                        DirectorateNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DirectorateLookupID);
            
            CreateTable(
                "DomainLookups",
                c => new
                    {
                        DomainLookupID = c.Int(nullable: false, identity: true),
                        DomainCode = c.String(unicode: false),
                        DomainNameEn = c.String(unicode: false),
                        DomainNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.DomainLookupID);
            
            CreateTable(
                "DrugClasses",
                c => new
                    {
                        DrugClassID = c.Int(nullable: false, identity: true),
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
                        DrugClassID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.DrugID)
                .ForeignKey("DrugClasses", t => t.DrugClassID, cascadeDelete: true)
                .Index(t => t.DrugClassID);
            
            CreateTable(
                "GenderLookups",
                c => new
                    {
                        GenderLookupID = c.Int(nullable: false, identity: true),
                        GenderEn = c.String(unicode: false),
                        GenderAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.GenderLookupID);
            
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
                "HealthFacilities",
                c => new
                    {
                        HealthFacilityID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        FacilityShortName = c.String(unicode: false),
                        HealthFacilityNameEn = c.String(unicode: false),
                        HealthFacilityNameAr = c.String(unicode: false),
                        GovernorateID = c.Int(nullable: false),
                        DomainLookupID = c.Int(),
                        HealthFacilityTypeID = c.Int(),
                        EstimatedClinics = c.Int(nullable: false),
                        EstimatedBeds = c.Int(nullable: false),
                        DirectorateLookupID = c.Int(nullable: false),
                        EHRActivationYear = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.HealthFacilityID)
                .ForeignKey("DirectorateLookups", t => t.DirectorateLookupID, cascadeDelete: true)
                .ForeignKey("DomainLookups", t => t.DomainLookupID)
                .ForeignKey("Governorates", t => t.GovernorateID, cascadeDelete: true)
                .ForeignKey("HealthFacilityTypes", t => t.HealthFacilityTypeID)
                .Index(t => t.SourceID)
                .Index(t => t.GovernorateID)
                .Index(t => t.DomainLookupID)
                .Index(t => t.HealthFacilityTypeID)
                .Index(t => t.DirectorateLookupID);
            
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
                "InPatientAdmissions",
                c => new
                    {
                        InPatientAdmissionID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PatientSourceID = c.Int(nullable: false),
                        AdmissionDate = c.DateTime(nullable: false, precision: 0),
                        MovementLookupID = c.Int(nullable: false),
                        PrimaryAdmissionReason = c.String(unicode: false),
                        WardLocationSourceID = c.Int(nullable: false),
                        HealthFacilitySourceID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InPatientAdmissionID)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("MovementLookups", t => t.MovementLookupID, cascadeDelete: true)
                .Index(t => t.SourceID)
                .Index(t => t.PatientSourceID)
                .Index(t => t.MovementLookupID)
                .Index(t => t.WardLocationSourceID)
                .Index(t => t.HealthFacilitySourceID)
                .Index(t => t.DomainLookupID);
            
            CreateTable(
                "MovementLookups",
                c => new
                    {
                        MovementLookupID = c.Int(nullable: false, identity: true),
                        MovementNameEn = c.String(unicode: false),
                        MovementNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.MovementLookupID);
            
            CreateTable(
                "InPatientDischarges",
                c => new
                    {
                        InPatientDischargeID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PatientSourceID = c.Int(nullable: false),
                        DischargeDate = c.DateTime(nullable: false, precision: 0),
                        MovementLookupID = c.Int(nullable: false),
                        InPatientAdmissionSourceID = c.Int(nullable: false),
                        LengthOfStaySourceDays = c.Double(nullable: false),
                        LengthOfStayCalculatedDays = c.Double(nullable: false),
                        LengthOfStayCalculatedHours = c.Double(nullable: false),
                        WardLocationSourceID = c.Int(nullable: false),
                        HealthFacilitySourceID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InPatientDischargeID)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("MovementLookups", t => t.MovementLookupID, cascadeDelete: true)
                .Index(t => t.SourceID)
                .Index(t => t.PatientSourceID)
                .Index(t => t.MovementLookupID)
                .Index(t => t.InPatientAdmissionSourceID)
                .Index(t => t.WardLocationSourceID)
                .Index(t => t.HealthFacilitySourceID)
                .Index(t => t.DomainLookupID);
            
            CreateTable(
                "InPatientEncounterTotals",
                c => new
                    {
                        InPatientEncounterTotalID = c.Int(nullable: false, identity: true),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        LOSGroup = c.String(unicode: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InPatientEncounterTotalID);
            
            CreateTable(
                "InPatientTransfers",
                c => new
                    {
                        InPatientTransferID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PatientSourceID = c.Int(nullable: false),
                        SpecialityLookupID = c.Int(nullable: false),
                        SpecialityTransferDate = c.DateTime(nullable: false, precision: 0),
                        AttendingProviderSourceID = c.Int(nullable: false),
                        PrimaryProviderSourceID = c.Int(nullable: false),
                        InPatientAdmissionSourceID = c.Int(nullable: false),
                        HealthFacilitySourceID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.InPatientTransferID)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("SpecialityLookups", t => t.SpecialityLookupID, cascadeDelete: true)
                .Index(t => t.SourceID)
                .Index(t => t.PatientSourceID)
                .Index(t => t.SpecialityLookupID)
                .Index(t => t.AttendingProviderSourceID)
                .Index(t => t.PrimaryProviderSourceID)
                .Index(t => t.InPatientAdmissionSourceID)
                .Index(t => t.HealthFacilitySourceID)
                .Index(t => t.DomainLookupID);
            
            CreateTable(
                "SpecialityLookups",
                c => new
                    {
                        SpecialityLookupID = c.Int(nullable: false, identity: true),
                        SectionLookupID = c.Int(nullable: false),
                        SpecialityNameEn = c.String(unicode: false),
                        SpecialityNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.SpecialityLookupID)
                .ForeignKey("SectionLookups", t => t.SectionLookupID, cascadeDelete: true)
                .Index(t => t.SectionLookupID);
            
            CreateTable(
                "SectionLookups",
                c => new
                    {
                        SectionLookupID = c.Int(nullable: false, identity: true),
                        ParentSectionID = c.Int(),
                        SectionNameEn = c.String(unicode: false),
                        SectionNameAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.SectionLookupID);
            
            CreateTable(
                "MaritalStatusLookups",
                c => new
                    {
                        MaritalStatusLookupID = c.Int(nullable: false, identity: true),
                        MaritalStatusEn = c.String(unicode: false),
                        MaritalStatusAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.MaritalStatusLookupID);
            
            CreateTable(
                "OutPatientEncounterDiagnosis",
                c => new
                    {
                        OutPatientEncounterDiagnosisID = c.Int(nullable: false, identity: true),
                        OutPatientEncounterID = c.Int(nullable: false),
                        DiagnosisLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OutPatientEncounterDiagnosisID)
                .ForeignKey("DiagnosisLookups", t => t.DiagnosisLookupID, cascadeDelete: true)
                .ForeignKey("OutPatientEncounters", t => t.OutPatientEncounterID, cascadeDelete: true)
                .Index(t => t.OutPatientEncounterID)
                .Index(t => t.DiagnosisLookupID);
            
            CreateTable(
                "OutPatientEncounters",
                c => new
                    {
                        OutPatientEncounterID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        VisitSourceID = c.Int(),
                        PatientSourceID = c.Int(nullable: false),
                        EncounterDate = c.DateTime(nullable: false, precision: 0),
                        AppointmentStatusLookupID = c.Int(nullable: false),
                        AppointmentTypeLookupID = c.Int(nullable: false),
                        ProviderSourceID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                        OutpatientLocationSourceID = c.Int(nullable: false),
                        HealthFacilitySourceID = c.Int(),
                    })
                .PrimaryKey(t => t.OutPatientEncounterID)
                .ForeignKey("AppointmentStatusLookups", t => t.AppointmentStatusLookupID, cascadeDelete: true)
                .ForeignKey("AppointmentTypeLookups", t => t.AppointmentTypeLookupID, cascadeDelete: true)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .Index(t => t.PatientSourceID)
                .Index(t => t.AppointmentStatusLookupID)
                .Index(t => t.AppointmentTypeLookupID)
                .Index(t => t.DomainLookupID);
            
            CreateTable(
                "OutPatientEncounterTotals",
                c => new
                    {
                        OutPatientEncounterTotalID = c.Int(nullable: false, identity: true),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OutPatientEncounterTotalID);
            
            CreateTable(
                "OutpatientLocations",
                c => new
                    {
                        OutpatientLocationID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        OutpatientLocationName = c.String(unicode: false),
                        DepartmentLookupID = c.Int(nullable: false),
                        SpecialityLookupID = c.Int(nullable: false),
                        HealthFacilitySourceID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.OutpatientLocationID)
                .ForeignKey("DepartmentLookups", t => t.DepartmentLookupID, cascadeDelete: true)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("SpecialityLookups", t => t.SpecialityLookupID, cascadeDelete: true)
                .Index(t => t.DepartmentLookupID)
                .Index(t => t.SpecialityLookupID)
                .Index(t => t.DomainLookupID);
            
            CreateTable(
                "Patients",
                c => new
                    {
                        PatientID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        DoB = c.DateTime(nullable: false, precision: 0),
                        SSN = c.String(unicode: false),
                        GenderLookupID = c.Int(nullable: false),
                        MaritalStatusLookupID = c.Int(),
                        CountryID = c.Int(),
                    })
                .PrimaryKey(t => t.PatientID)
                .ForeignKey("Countries", t => t.CountryID)
                .ForeignKey("GenderLookups", t => t.GenderLookupID, cascadeDelete: true)
                .ForeignKey("MaritalStatusLookups", t => t.MaritalStatusLookupID)
                .Index(t => t.SourceID)
                .Index(t => t.GenderLookupID)
                .Index(t => t.MaritalStatusLookupID)
                .Index(t => t.CountryID);
            
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
                "Prescriptions",
                c => new
                    {
                        PrescriptionID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        PharmacyID = c.Int(nullable: false),
                        PatientID = c.Int(nullable: false),
                        PrescriptionDate = c.DateTime(nullable: false, precision: 0),
                        ProviderID = c.Int(nullable: false),
                        DrugID = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Duration = c.Int(nullable: false),
                        RequestedRefills = c.Int(),
                    })
                .PrimaryKey(t => t.PrescriptionID)
                .ForeignKey("Drugs", t => t.DrugID, cascadeDelete: true)
                .ForeignKey("Patients", t => t.PatientID, cascadeDelete: true)
                .ForeignKey("Pharmacies", t => t.PharmacyID, cascadeDelete: true)
                .ForeignKey("Providers", t => t.ProviderID, cascadeDelete: true)
                .Index(t => t.PharmacyID)
                .Index(t => t.PatientID)
                .Index(t => t.ProviderID)
                .Index(t => t.DrugID);
            
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
                        SpecialityLookupID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProviderID)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("SpecialityLookups", t => t.SpecialityLookupID, cascadeDelete: true)
                .Index(t => t.SourceID)
                .Index(t => t.SpecialityLookupID)
                .Index(t => t.DomainLookupID);
            
            CreateTable(
                "PrescriptionTotals",
                c => new
                    {
                        PrescriptionTotalID = c.Int(nullable: false, identity: true),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        DrugClassID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrescriptionTotalID);
            
            CreateTable(
                "ProcedureLookups",
                c => new
                    {
                        ProcedureLookupID = c.Int(nullable: false, identity: true),
                        ProcedureCode = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.ProcedureLookupID);
            
            CreateTable(
                "SurgeryEncounters",
                c => new
                    {
                        SurgeryEncounterID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(nullable: false),
                        PatientID = c.Int(nullable: false),
                        StartDateTime = c.DateTime(nullable: false, precision: 0),
                        EndDateTime = c.DateTime(nullable: false, precision: 0),
                        ProcedureLookupID = c.Int(nullable: false),
                        SurgerySeverityLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurgeryEncounterID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID, cascadeDelete: true)
                .ForeignKey("Patients", t => t.PatientID, cascadeDelete: true)
                .ForeignKey("ProcedureLookups", t => t.ProcedureLookupID, cascadeDelete: true)
                .ForeignKey("SurgerySeverityLookups", t => t.SurgerySeverityLookupID, cascadeDelete: true)
                .Index(t => t.HealthFacilityID)
                .Index(t => t.PatientID)
                .Index(t => t.ProcedureLookupID)
                .Index(t => t.SurgerySeverityLookupID);
            
            CreateTable(
                "SurgerySeverityLookups",
                c => new
                    {
                        SurgerySeverityLookupID = c.Int(nullable: false, identity: true),
                        SeverityEn = c.String(unicode: false),
                        SeverityAr = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.SurgerySeverityLookupID);
            
            CreateTable(
                "SurgeryTotals",
                c => new
                    {
                        SurgeryTotalID = c.Int(nullable: false, identity: true),
                        HealthFacilityID = c.Int(nullable: false),
                        ProviderID = c.Int(nullable: false),
                        Year = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        SurgerySeverityID = c.Int(nullable: false),
                        Total = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SurgeryTotalID);
            
            CreateTable(
                "Targets",
                c => new
                    {
                        TargetID = c.Int(nullable: false, identity: true),
                        IndicatorID = c.Int(nullable: false),
                        HealthFacilityID = c.Int(),
                        ProviderID = c.Int(),
                        DomainLookupID = c.Int(),
                        DirectorateLookupID = c.Int(),
                        EffectiveDate = c.DateTime(precision: 0),
                        Year = c.Int(),
                        Month = c.Int(),
                        Value = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.TargetID)
                .ForeignKey("DirectorateLookups", t => t.DirectorateLookupID)
                .ForeignKey("DomainLookups", t => t.DomainLookupID)
                .ForeignKey("HealthFacilities", t => t.HealthFacilityID)
                .ForeignKey("Indicators", t => t.IndicatorID, cascadeDelete: true)
                .ForeignKey("Providers", t => t.ProviderID)
                .Index(t => t.IndicatorID)
                .Index(t => t.HealthFacilityID)
                .Index(t => t.ProviderID)
                .Index(t => t.DomainLookupID)
                .Index(t => t.DirectorateLookupID);
            
            CreateTable(
                "WardLocations",
                c => new
                    {
                        WardLocationID = c.Int(nullable: false, identity: true),
                        SourceID = c.Int(nullable: false),
                        WardLocationName = c.String(unicode: false),
                        OperatingBeds = c.Int(nullable: false),
                        DepartmentLookupID = c.Int(nullable: false),
                        SpecialityLookupID = c.Int(nullable: false),
                        HealthFacilitySourceID = c.Int(nullable: false),
                        DomainLookupID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WardLocationID)
                .ForeignKey("DepartmentLookups", t => t.DepartmentLookupID, cascadeDelete: true)
                .ForeignKey("DomainLookups", t => t.DomainLookupID, cascadeDelete: true)
                .ForeignKey("SpecialityLookups", t => t.SpecialityLookupID, cascadeDelete: true)
                .Index(t => t.DepartmentLookupID)
                .Index(t => t.SpecialityLookupID)
                .Index(t => t.DomainLookupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("WardLocations", "SpecialityLookupID", "SpecialityLookups");
            DropForeignKey("WardLocations", "DomainLookupID", "DomainLookups");
            DropForeignKey("WardLocations", "DepartmentLookupID", "DepartmentLookups");
            DropForeignKey("Targets", "ProviderID", "Providers");
            DropForeignKey("Targets", "IndicatorID", "Indicators");
            DropForeignKey("Targets", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Targets", "DomainLookupID", "DomainLookups");
            DropForeignKey("Targets", "DirectorateLookupID", "DirectorateLookups");
            DropForeignKey("SurgeryEncounters", "SurgerySeverityLookupID", "SurgerySeverityLookups");
            DropForeignKey("SurgeryEncounters", "ProcedureLookupID", "ProcedureLookups");
            DropForeignKey("SurgeryEncounters", "PatientID", "Patients");
            DropForeignKey("SurgeryEncounters", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Prescriptions", "ProviderID", "Providers");
            DropForeignKey("Providers", "SpecialityLookupID", "SpecialityLookups");
            DropForeignKey("Providers", "DomainLookupID", "DomainLookups");
            DropForeignKey("Prescriptions", "PharmacyID", "Pharmacies");
            DropForeignKey("Prescriptions", "PatientID", "Patients");
            DropForeignKey("Prescriptions", "DrugID", "Drugs");
            DropForeignKey("Pharmacies", "HealthFacilityID", "HealthFacilities");
            DropForeignKey("Patients", "MaritalStatusLookupID", "MaritalStatusLookups");
            DropForeignKey("Patients", "GenderLookupID", "GenderLookups");
            DropForeignKey("Patients", "CountryID", "Countries");
            DropForeignKey("OutpatientLocations", "SpecialityLookupID", "SpecialityLookups");
            DropForeignKey("OutpatientLocations", "DomainLookupID", "DomainLookups");
            DropForeignKey("OutpatientLocations", "DepartmentLookupID", "DepartmentLookups");
            DropForeignKey("OutPatientEncounterDiagnosis", "OutPatientEncounterID", "OutPatientEncounters");
            DropForeignKey("OutPatientEncounters", "DomainLookupID", "DomainLookups");
            DropForeignKey("OutPatientEncounters", "AppointmentTypeLookupID", "AppointmentTypeLookups");
            DropForeignKey("OutPatientEncounters", "AppointmentStatusLookupID", "AppointmentStatusLookups");
            DropForeignKey("OutPatientEncounterDiagnosis", "DiagnosisLookupID", "DiagnosisLookups");
            DropForeignKey("InPatientTransfers", "SpecialityLookupID", "SpecialityLookups");
            DropForeignKey("SpecialityLookups", "SectionLookupID", "SectionLookups");
            DropForeignKey("InPatientTransfers", "DomainLookupID", "DomainLookups");
            DropForeignKey("InPatientDischarges", "MovementLookupID", "MovementLookups");
            DropForeignKey("InPatientDischarges", "DomainLookupID", "DomainLookups");
            DropForeignKey("InPatientAdmissions", "MovementLookupID", "MovementLookups");
            DropForeignKey("InPatientAdmissions", "DomainLookupID", "DomainLookups");
            DropForeignKey("HealthFacilities", "HealthFacilityTypeID", "HealthFacilityTypes");
            DropForeignKey("HealthFacilities", "GovernorateID", "Governorates");
            DropForeignKey("HealthFacilities", "DomainLookupID", "DomainLookups");
            DropForeignKey("HealthFacilities", "DirectorateLookupID", "DirectorateLookups");
            DropForeignKey("Governorates", "RegionID", "Regions");
            DropForeignKey("Regions", "CountryID", "Countries");
            DropForeignKey("Drugs", "DrugClassID", "DrugClasses");
            DropIndex("WardLocations", new[] { "DomainLookupID" });
            DropIndex("WardLocations", new[] { "SpecialityLookupID" });
            DropIndex("WardLocations", new[] { "DepartmentLookupID" });
            DropIndex("Targets", new[] { "DirectorateLookupID" });
            DropIndex("Targets", new[] { "DomainLookupID" });
            DropIndex("Targets", new[] { "ProviderID" });
            DropIndex("Targets", new[] { "HealthFacilityID" });
            DropIndex("Targets", new[] { "IndicatorID" });
            DropIndex("SurgeryEncounters", new[] { "SurgerySeverityLookupID" });
            DropIndex("SurgeryEncounters", new[] { "ProcedureLookupID" });
            DropIndex("SurgeryEncounters", new[] { "PatientID" });
            DropIndex("SurgeryEncounters", new[] { "HealthFacilityID" });
            DropIndex("Providers", new[] { "DomainLookupID" });
            DropIndex("Providers", new[] { "SpecialityLookupID" });
            DropIndex("Providers", new[] { "SourceID" });
            DropIndex("Prescriptions", new[] { "DrugID" });
            DropIndex("Prescriptions", new[] { "ProviderID" });
            DropIndex("Prescriptions", new[] { "PatientID" });
            DropIndex("Prescriptions", new[] { "PharmacyID" });
            DropIndex("Pharmacies", new[] { "HealthFacilityID" });
            DropIndex("Patients", new[] { "CountryID" });
            DropIndex("Patients", new[] { "MaritalStatusLookupID" });
            DropIndex("Patients", new[] { "GenderLookupID" });
            DropIndex("Patients", new[] { "SourceID" });
            DropIndex("OutpatientLocations", new[] { "DomainLookupID" });
            DropIndex("OutpatientLocations", new[] { "SpecialityLookupID" });
            DropIndex("OutpatientLocations", new[] { "DepartmentLookupID" });
            DropIndex("OutPatientEncounters", new[] { "DomainLookupID" });
            DropIndex("OutPatientEncounters", new[] { "AppointmentTypeLookupID" });
            DropIndex("OutPatientEncounters", new[] { "AppointmentStatusLookupID" });
            DropIndex("OutPatientEncounters", new[] { "PatientSourceID" });
            DropIndex("OutPatientEncounterDiagnosis", new[] { "DiagnosisLookupID" });
            DropIndex("OutPatientEncounterDiagnosis", new[] { "OutPatientEncounterID" });
            DropIndex("SpecialityLookups", new[] { "SectionLookupID" });
            DropIndex("InPatientTransfers", new[] { "DomainLookupID" });
            DropIndex("InPatientTransfers", new[] { "HealthFacilitySourceID" });
            DropIndex("InPatientTransfers", new[] { "InPatientAdmissionSourceID" });
            DropIndex("InPatientTransfers", new[] { "PrimaryProviderSourceID" });
            DropIndex("InPatientTransfers", new[] { "AttendingProviderSourceID" });
            DropIndex("InPatientTransfers", new[] { "SpecialityLookupID" });
            DropIndex("InPatientTransfers", new[] { "PatientSourceID" });
            DropIndex("InPatientTransfers", new[] { "SourceID" });
            DropIndex("InPatientDischarges", new[] { "DomainLookupID" });
            DropIndex("InPatientDischarges", new[] { "HealthFacilitySourceID" });
            DropIndex("InPatientDischarges", new[] { "WardLocationSourceID" });
            DropIndex("InPatientDischarges", new[] { "InPatientAdmissionSourceID" });
            DropIndex("InPatientDischarges", new[] { "MovementLookupID" });
            DropIndex("InPatientDischarges", new[] { "PatientSourceID" });
            DropIndex("InPatientDischarges", new[] { "SourceID" });
            DropIndex("InPatientAdmissions", new[] { "DomainLookupID" });
            DropIndex("InPatientAdmissions", new[] { "HealthFacilitySourceID" });
            DropIndex("InPatientAdmissions", new[] { "WardLocationSourceID" });
            DropIndex("InPatientAdmissions", new[] { "MovementLookupID" });
            DropIndex("InPatientAdmissions", new[] { "PatientSourceID" });
            DropIndex("InPatientAdmissions", new[] { "SourceID" });
            DropIndex("HealthFacilities", new[] { "DirectorateLookupID" });
            DropIndex("HealthFacilities", new[] { "HealthFacilityTypeID" });
            DropIndex("HealthFacilities", new[] { "DomainLookupID" });
            DropIndex("HealthFacilities", new[] { "GovernorateID" });
            DropIndex("HealthFacilities", new[] { "SourceID" });
            DropIndex("Regions", new[] { "CountryID" });
            DropIndex("Governorates", new[] { "RegionID" });
            DropIndex("Drugs", new[] { "DrugClassID" });
            DropTable("WardLocations");
            DropTable("Targets");
            DropTable("SurgeryTotals");
            DropTable("SurgerySeverityLookups");
            DropTable("SurgeryEncounters");
            DropTable("ProcedureLookups");
            DropTable("PrescriptionTotals");
            DropTable("Providers");
            DropTable("Prescriptions");
            DropTable("Pharmacies");
            DropTable("Patients");
            DropTable("OutpatientLocations");
            DropTable("OutPatientEncounterTotals");
            DropTable("OutPatientEncounters");
            DropTable("OutPatientEncounterDiagnosis");
            DropTable("MaritalStatusLookups");
            DropTable("SectionLookups");
            DropTable("SpecialityLookups");
            DropTable("InPatientTransfers");
            DropTable("InPatientEncounterTotals");
            DropTable("InPatientDischarges");
            DropTable("MovementLookups");
            DropTable("InPatientAdmissions");
            DropTable("Indicators");
            DropTable("HealthFacilityTypes");
            DropTable("HealthFacilities");
            DropTable("Regions");
            DropTable("Governorates");
            DropTable("GenderLookups");
            DropTable("Drugs");
            DropTable("DrugClasses");
            DropTable("DomainLookups");
            DropTable("DirectorateLookups");
            DropTable("DiagnosisLookups");
            DropTable("DepartmentLookups");
            DropTable("Countries");
            DropTable("AppointmentTypeLookups");
            DropTable("AppointmentStatusLookups");
        }
    }
}
