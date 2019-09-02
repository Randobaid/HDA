using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class WardLocation
    {
        public int WardLocationID { get; set; }
        public int SourceID { get; set; }
        public string WardLocationName { get; set; }
        public int OperatingBeds { get; set; }
        public int DepartmentLookupID { get; set; }
        public virtual DepartmentLookup DepartmentLookup { get; set; }
        public int SpecialityLookupID { get; set; }
        public virtual SpecialityLookup SpecialityLookup { get; set; }
        public int HealthFacilitySourceID { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }


    }
}