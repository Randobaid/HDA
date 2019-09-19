using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class PrescriptionTotal
    {
        public int PrescriptionTotalID { get; set; }
        public int DomainID { get; set; }
        public int DirectorateID { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public int HealthFacilityID { get; set; }
        public int PharmacyID { get; set; }
        public int ProviderID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int DrugClassID { get; set; }
        public int DrugId { get; set; }
        public int TotalPrescriptions { get; set; }
        public int TotalQuantity { get; set; }

    }
}