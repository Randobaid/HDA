using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
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
        public int? DomainID { get; set; }
        public virtual Domain Domain { get; set; }
        public int? DirectorateID { get; set; }
        public virtual Directorate Directorate { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string Value { get; set; }
    }
}