using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class HDAReportsContext : DbContext
    {
        public HDAReportsContext() : base("hdareports") { }

        
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<Directorate> Directorates { get; set; }
        public DbSet<HealthFacilityType> HealthFacilityTypes { get; set; }
        public DbSet<HealthFacility> HealthFacilities { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<DiagnosisCodingSystem> DiagnosisCodingSystems { get; set; }
        public DbSet<DiagnosisCode> DiagnosisCodes { get; set; }
        public DbSet<DrugClass> DrugClasses { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<OutPatientEncounterTotal> OutPatientEncounterTotals { get; set; }
        public DbSet<InPatientEncounterTotal> InPatientEncounterTotals { get; set; }
        public DbSet<SurgeryTotal> SurgeryTotals { get; set; }
        public DbSet<PrescriptionTotal> PrescriptionTotals { get; set; }
        public DbSet<DiagnosisTotal> DiagnosisTotals { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<Report> Reports { get; set; }

    }
    
}