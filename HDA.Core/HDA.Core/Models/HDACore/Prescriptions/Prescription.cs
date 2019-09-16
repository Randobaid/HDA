using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class Prescription
    {
        public int PrescriptionID { get; set; }
        public int SourceID { get; set; }
        public int PharmacyID { get; set; }
        public virtual Pharmacy Pharmacy {get; set; }
        public int PatientID { get; set; }
        public virtual Patient Patient { get; set; }
        public DateTime PrescriptionDate { get; set; }
        public int ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        public int DrugID { get; set; }
        public virtual Drug Drug { get; set; }
        public int Quantity { get; set; }
        public int Duration { get; set; }
        public int? RequestedRefills { get; set; }
    }
}