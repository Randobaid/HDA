using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class HealthFacilityVM
    {
        public string ID { get; set; }
        public string HealthFacilityName { get; set; }
        public int DomainID { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public int DirectorateID { get; set; }
    }

    public class PharmacyVM
    {
        public string ID { get; set; }
        public int HealthFacilityID { get; set; }
        public string PharmacyName { get; set; }
    }

    public class SelectedFacilityPayload
    {
        public List<SelectedFacilityType> HealthFacilityTypes { get; set; }
        public List<string> HealthFacilities { get; set; }
    }

    public class SelectedFacilityType
    {
        public int HealthFacilityTypeId { get; set; }
    }
}