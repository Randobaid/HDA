using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class Workload
    {
        
    }
    
    public class MonthlyTotal
    {
        public int Year { get; set; }
        public int MonthId { get; set; }
        public string MonthName { get; set; }
        public int Total { get; set; }
        public int TotalPreviousYear { get; set; }
        public int Target { get; set; }
        public int TargetPreviousYear { get; set; }
    }

    public class WorkloadRequest
    {
        public string HealthFacilityID { get; set; }
        public string ProviderID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int PreviousYear { get; set; }
    }

    public class InPatientEncountersRequest
    {
        public string HealthFacilityID { get; set; }
        public string ProviderID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }

    public class InPatientLOSTotal
    {
        public int Year { get; set; }
        public int MonthId { get; set; }
        public int LOS13Total { get; set; }
        public int LOS47Total { get; set; }
        public int LOS8Total { get; set; }
        public int LOSNDTotal { get; set; }
        public double Target { get; set; }
    }

    public class SurgeryBySeverityTotal
    {
        public int Year { get; set; }
        public int MonthId { get; set; }
        public int MinorSeverityTotal { get; set; }
        public int MajorSeverityTotal { get; set; }
        public string Target { get; set; }
    }


    
}