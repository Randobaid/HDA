using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class DrugVM
    {
        public int ID { get; set; }
        public string DrugName { get; set; }
    }

    public class DrugClassVM
    {
        public int ID { get; set; }
        public string DrugClassName { get; set; }
    }

    public class OPRequest
    {
        public int HealthFacilityID { get; set; }
        public int ProviderID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int DrugClassId { get; set; }
        public int DrugId { get; set; }

    }
}