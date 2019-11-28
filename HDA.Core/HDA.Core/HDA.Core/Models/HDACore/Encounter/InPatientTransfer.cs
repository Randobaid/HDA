using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class InPatientTransfer
    {
        public int InPatientTransferID { get; set; }
        [Index]
        public int SourceID { get; set; }
        [Index]
        public int PatientSourceID { get; set; }
        public int SpecialityLookupID { get; set; }
        public virtual SpecialityLookup SpecialityLookup { get; set; }
        public DateTime SpecialityTransferDate { get; set; }
        [Index]
        public int AttendingProviderSourceID { get; set; }
        [Index]
        public int PrimaryProviderSourceID { get; set; }
        [Index]
        public int InPatientAdmissionSourceID { get; set; }
        [Index]
        public int HealthFacilitySourceID { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }
    }
}