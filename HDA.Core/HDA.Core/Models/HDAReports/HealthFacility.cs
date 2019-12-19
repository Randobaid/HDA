using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class HealthFacility
    {
        public string HealthFacilityID { get; set; }
        [Index]
        public string FacilityShortName { get; set; }
        public string HealthFacilityNameEn { get; set; }
        public string HealthFacilityNameAr { get; set; }
        public int GovernorateID { get; set; }
        public virtual Governorate Governorate { get; set; }
        public int? DomainID { get; set; }
        public virtual Domain Domain { get; set; }
        public int? HealthFacilityTypeID { get; set; }
        public virtual HealthFacilityType HealthFacilityType { get; set; }
        public int EstimatedClinics { get; set; }
        public int EstimatedBeds { get; set; }
        public int EstimatedOperatingRooms { get; set; }
        public int DirectorateID { get; set; }
        public virtual Directorate Directorate { get; set; }
        public int EHRActivationYear { get; set; }
    }
}