using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class SurgeryEncounter
    {
        public int SurgeryEncounterID { get; set; }
        public int SourceID { get; set; }
        public int HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int ProcedureLookupID { get; set; }
        public virtual ProcedureLookup ProcedureLookup { get; set; }
        public int SurgerySeverityLookupID { get; set; }
        public virtual SurgerySeverityLookup SurgerySeverityLookup { get; set; }
    }
}