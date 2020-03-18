using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class PrescriptionTotal
    {
        public int PrescriptionTotalID { get; set; }
        public int DomainID { get; set; }
        public int DirectorateID { get; set; }
        public int GovernorateID { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public string HealthFacilityID { get; set; }
        //public virtual HealthFacility HealthFacility { get; set; }
        public string PharmacyID { get; set; }
        public string ProviderID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int DrugClassID { get; set; }
        public string DrugId { get; set; }
        public int AgeRangeID { get; set; }
        public int GenderID { get; set; }
        public int NationalityCategoryID { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalQuantity { get; set; }
        public int TotalRefillQuantity { get; set; }

    }
}