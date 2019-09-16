using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class SurgeryTotal
    {
        public int SurgeryTotalID { get; set; }
        public int HealthFacilityID { get; set; }
        public int ProviderID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int SurgerySeverityID { get; set; }
        public int Total { get; set; }
    }
}