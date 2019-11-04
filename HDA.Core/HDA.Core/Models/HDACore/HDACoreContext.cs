using HDA.Core.Models.HDACore.RefreshData;
using MySql.Data.Entity;
using System.Data.Entity;

namespace HDA.Core.Models.HDACore
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class HDACoreContext : DbContext
    {
        public HDACoreContext() : base("hdacore") { }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<DomainLookup> DomainLookups { get; set; }
        public DbSet<DirectorateLookup> DirectorateLookups { get; set; }
        public DbSet<HealthFacilityType> HealthFacilityTypes { get; set; }
        public DbSet<HealthFacility> HealthFacilities { get; set; }
        //public DbSet<Service> Services { get; set; }
        //public DbSet<Location> Locations { get; set; }
        //public DbSet<HealthFacilityLocation> HealthFacilityLocations { get; set; }
        //public DbSet<HealthFacilityLocationService> HealthFacilityLocationServices { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<GenderLookup> GenderLookups { get; set; }
        public DbSet<MaritalStatusLookup> MaritalStatusLookups { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<OutPatientEncounter> OutPatientEncounters { get; set; }
        //public DbSet<InPatientEncounter> InPatientEncounters { get; set; }
        public DbSet<DiagnosisLookup> DiagnosisLookups { get; set; }
        public DbSet<OutPatientEncounterDiagnosis> OutPatientEncounterDiagnoses { get; set; }
        //public DbSet<InPatientEncounterDiagnosis> InPatientEncounterDiagnoses { get; set; }
        public DbSet<ProcedureLookup> ProcedureLookups { get; set; }
        public DbSet<SurgerySeverityLookup> SurgerySeverityLookups { get; set; }
        public DbSet<SurgeryEncounter> SurgeryEncounters { get; set; }
        public DbSet<DepartmentLookup> DepartmentLookups { get; set; }
        public DbSet<OutpatientLocation> OutpatientLocations { get; set; }
        public DbSet<AppointmentStatusLookup> AppointmentStatusLookups { get; set; }
        public DbSet<AppointmentTypeLookup> AppointmentTypeLookups { get; set; }
        public DbSet<SectionLookup> SectionLookups { get; set; }
        public DbSet<SpecialityLookup> SpecialityLookups { get; set; }
        public DbSet<WardLocation> WardLocations { get; set; }
        public DbSet<MovementLookup> MovementLookups { get; set; }
        public DbSet<InPatientAdmission> InPatientAdmissions { get; set; }
        public DbSet<InPatientTransfer> InPatientTransfers { get; set; }
        public DbSet<InPatientDischarge> InPatientDischarges { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<Target> Targets { get; set; }
        public DbSet<DrugClass> DrugClasses { get; set; }
        public DbSet<Drug> Drugs { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<OutPatientEncounterTotal> OutPatientEncounterTotals { get; set; }
        public DbSet<InPatientEncounterTotal> InPatientEncounterTotals { get; set; }
        public DbSet<SurgeryTotal> SurgeryTotals { get; set; }
        public DbSet<PrescriptionTotal> PrescriptionTotals { get; set; }
        public DbSet<DataRefreshProcedureStatus> DataRefreshProcedures { get; set; }
    }
    
}