using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HDA.Core.Models.HDACore;

namespace HDA.Core.ViewModels
{
    public class TargetViewModel
    {
        public int TargetID { get; set; }
        public int IndicatorID { get; set; }
        public IndicatorViewModel Indicator { get; set; }
        public int? HealthFacilityID { get; set; }
        public HealthFacilityVM HealthFacility { get; set; }
        public int? ProviderID { get; set; }
        public ProviderVM Provider { get; set; }
        public int? DomainLookupID { get; set; }
        public DomainLookupViewModel DomainLookup { get; set;}
        public int? DirectorateLookupID { get; set; }
        public DirectorateLookupViewModel DirectorateLookup { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Value { get; set; }
    }
}