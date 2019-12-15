using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class SelectedStartCode
    {
        public int DiagnosisCodeID { get; set; }
        public string DiagnosisCode { get; set; }
    }

    public class DiagnosisCodeVM
    {
        public int DiagnosisCodeID { get; set; }
        public string Code { get; set; }
        public string CodeName { get; set; }
    }

    public class DiseaseManagementRequestPayload
    {
        public string HealthFacilityID { get; set; }
        public int StartCodeID { get; set; }
        public int EndCodeID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class NewCasesByAgeGroup
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public string AgeGroup { get; set; }
        public int Total { get; set; }
    }
    public class NewCasesByGender
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int FemaleTotal { get; set; }
        public int MaleTotal { get; set; }
    }

    public class TotalsByAgeGroup
    {
        public int Total01 { get; set; }
        public int Total25 { get; set; }
        public int Total614 { get; set; }
        public int Total1535 { get; set; }
        public int Total3660 { get; set; }
        public int Total60Plus { get; set; }
    }
    public class TotalsByGender
    {
        public int FemaleTotal { get; set; }
        public int MaleTotal { get; set; }
    }

    public class DiagnosisSummary
    {
        public int AverageNewCasesPerMonth { get; set; }
        public int TotalNewCases { get; set; }
    }
}