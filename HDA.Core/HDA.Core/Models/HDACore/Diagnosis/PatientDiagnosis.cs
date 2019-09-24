using System;

namespace HDA.Core.Models.HDACore
{
    public class PatientDiagnosis
    {
        public int PatientDiagnosisID { get; set; }
        public int SourceID { get; set; }
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
        public int HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public DateTime DiagnosisDate { get; set; }
        public int DiagnosisCodeID { get; set; }
        public virtual DiagnosisCode DiagnosisCode { get; set; }
        public int ProviderID { get; set; }
        public virtual Provider Provider { get; set; }

    }
}