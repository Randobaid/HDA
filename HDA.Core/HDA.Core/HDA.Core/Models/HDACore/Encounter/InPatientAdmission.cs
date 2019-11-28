using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class InPatientAdmission
    {
        public int InPatientAdmissionID { get; set; }
        [Index]
        public int SourceID { get; set; }
        [Index]
        public int PatientSourceID { get; set; }
        public DateTime AdmissionDate { get; set; }
        public int MovementLookupID { get; set; }
        public virtual MovementLookup MovementLookup { get; set; }
        public string PrimaryAdmissionReason { get; set; }
        [Index]
        public int WardLocationSourceID { get; set; }
        [Index]
        public int HealthFacilitySourceID { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }
    }
}