namespace HDA.Core.Models.HDACore
{
    public class OutPatientEncounterDiagnosis
    {
        public int OutPatientEncounterDiagnosisID { get; set; }
        public int OutPatientEncounterID { get; set; }
        public virtual OutPatientEncounter OutPatientEncounter { get; set; }
        public int DiagnosisLookupID { get; set; }
        public virtual DiagnosisLookup DiagnosisLookup { get; set; }

    }
}