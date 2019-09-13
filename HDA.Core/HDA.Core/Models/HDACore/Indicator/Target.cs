using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class Target
    {
        public int TargetID { get; set; }
        public int IndicatorID { get; set; }
        public virtual Indicator Indicator { get; set; }
        public int? HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public int? ProviderID { get; set; }
        public virtual Provider Provider { get; set; }
        public int? DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set;}
        public int? DirectorateLookupID { get; set; }
        public virtual DirectorateLookup DirectorateLookup { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Value { get; set; }
    }
}