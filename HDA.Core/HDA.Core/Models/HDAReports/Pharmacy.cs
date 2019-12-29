using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class Pharmacy
    {
        public string PharmacyID { get; set; }
        public string PharmacyName { get; set; }
        public int DepartmentID { get; set; }
        public int SpecialtyID { get; set; }
        public string HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
    }
}