using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class PrescriptionRefill
    {
        public int PrescriptionRefillID { get; set; }
        public int SourceID { get; set; }
        public int PrescriptionID { get; set; }
        public virtual Prescription Prescription { get; set; }
        public int DrugID { get; set; }
        public virtual Drug Drug { get; set; }
        public int RefillSequence { get; set; }
        public int Quantity { get; set; }
        public int DaysSupply { get; set; }
        public DateTime FillDate { get; set; }
        public int PharmacyID { get; set; }
        public virtual Pharmacy Pharmacy { get; set; }

    }
}