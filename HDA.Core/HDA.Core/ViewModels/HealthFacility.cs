using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class HealthFacilityVM
    {
        public int ID { get; set; }
        public string HealthFacilityName { get; set; }
    }

    public class PharmacyVM
    {
        public int ID { get; set; }
        public int HealthFacilityID { get; set; }
        public string PharmacyName { get; set; }
    }

    public class SelectedFacilityType
    {
        public int HealthFacilityTypeId { get; set; }
    }
}