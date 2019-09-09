using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class TargetViewModel
    {
        public int TargetID { get; set; }
        public int IndicatorID { get; set; }
        public int? HealthFacilityID { get; set; }
        public int? ProviderID { get; set; }
        public int? DomainLookupID { get; set; }
        public int? DirectorateLookupID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string Value { get; set; }
    }
}