using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class PrescriptionTotal
    {
        public int PrescriptionTotalID { get; set; }
        public int HealthFacilityID { get; set; }
        public int ProviderID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int DrugClassID { get; set; }
        public int Total { get; set; }

    }
}