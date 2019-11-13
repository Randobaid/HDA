using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class DiagnosisTotal
    {
        public int DiagnosisTotalID { get; set; }
        public int DomainID { get; set; }
        public virtual Domain Domain { get; set; }
        public int DirectorateID { get; set; }
        public virtual Directorate Directorate { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public virtual HealthFacilityType HealthFacilityType { get; set; }
        public int HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public int DiagnosisCodeID { get; set; }
        public virtual DiagnosisCode DiagnosisCode { get; set; }
        public string AgeGroup { get; set; }
        public int GenderID { get; set; }
        public virtual Gender Gender { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Total { get; set; }
    }
}