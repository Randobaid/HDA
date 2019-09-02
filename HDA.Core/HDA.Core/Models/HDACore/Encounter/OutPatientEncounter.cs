using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDA.Core.Models.HDACore
{
    public class OutPatientEncounter
    {
        public int OutPatientEncounterID { get; set; }
        public int SourceID { get; set; }
        public int? VisitSourceID { get; set; }
        [Index]
        public int PatientSourceID { get; set; }
        public DateTime EncounterDate { get; set; }
        public int AppointmentStatusLookupID { get; set; }
        public virtual AppointmentStatusLookup AppointmentStatusLookup { get; set; }
        public int AppointmentTypeLookupID { get; set; }
        public virtual AppointmentTypeLookup AppointmentTypeLookup { get; set; }
        public int ProviderSourceID { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set;}
        public int OutpatientLocationSourceID { get; set; }
        public int? HealthFacilitySourceID { get; set; }
    }
}