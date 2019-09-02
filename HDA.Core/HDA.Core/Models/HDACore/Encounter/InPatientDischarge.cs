using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class InPatientDischarge
    {
        public int InPatientDischargeID { get; set; }
        [Index]
        public int SourceID { get; set; }
        [Index]
        public int PatientSourceID { get; set; }
        public DateTime DischargeDate { get; set; }
        public int MovementLookupID { get; set; }
        public virtual MovementLookup MovementLookup { get; set; }
        [Index]
        public int InPatientAdmissionSourceID { get; set; }
        public double LengthOfStaySourceDays { get; set; }
        public double LengthOfStayCalculatedDays { get; set; }
        public double LengthOfStayCalculatedHours { get; set; }
        [Index]
        public int WardLocationSourceID { get; set; }
        [Index]
        public int HealthFacilitySourceID { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }

    }
}