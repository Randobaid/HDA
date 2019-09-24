using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class DiagnosisTotal
    {
        public int DiagnosisTotalID { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }
        public int DirectorateLookupID { get; set; }
        public virtual DirectorateLookup DirectorateLookup { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public virtual HealthFacilityType HealthFacilityType { get; set; }
        public int HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public int DiagnosisCodeID { get; set; }
        public virtual DiagnosisCode DiagnosisCode { get; set; }
        public string AgeGroup { get; set; }
        public int GenderLookupID { get; set; }
        public virtual GenderLookup GenderLookup { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Total { get; set; }
    }
}