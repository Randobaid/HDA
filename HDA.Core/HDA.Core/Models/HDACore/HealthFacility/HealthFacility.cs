using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class HealthFacility
    {
        public int HealthFacilityID { get; set; }
        [Index]
        public int SourceID { get; set; }
        public string FacilityShortName { get; set; }
        public string HealthFacilityNameEn { get; set; }
        public string HealthFacilityNameAr { get; set; }
        public int GovernorateID { get; set; }
        public virtual Governorate Governorate { get; set; }
        public int? DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }
        public int? HealthFacilityTypeID { get; set; }
        public virtual HealthFacilityType HealthFacilityType { get; set; }
        public int EstimatedClinics { get; set; }
        public int EstimatedBeds { get; set; }
        public int EstimatedOperatingRooms { get; set; }
        public int DirectorateLookupID { get; set; }
        public virtual DirectorateLookup DirectorateLookup { get; set; }
        public int EHRActivationYear { get; set; }
    }
}