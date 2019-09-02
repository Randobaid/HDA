using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class HealthFacilityInventory
    {
        public int HealthFacilityID { get; set; }
        public int? NumberOfBeds { get; set; }
        public int? NumberOfClinics { get; set; }
        public int? NumberOfOperatingRooms { get; set; }
    }
}