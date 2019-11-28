using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class Pharmacy
    {
        public int PharmacyID { get; set; }
        public int SourceID { get; set; }
        public string PharmacyName { get; set; }
        public int HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
    }
}