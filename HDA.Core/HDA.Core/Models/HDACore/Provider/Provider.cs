using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class Provider
    {
        public int ProviderID { get; set; }
        [Index]
        public int SourceID { get; set; }
        public int? MasterProviderIndex { get; set; }
        public string JobID { get; set; }
        public string ProviderType { get; set; }
        public int? NationalID { get; set; }
        public string ProviderNameEn { get; set; }
        public string ProviderNameAr { get; set; }
        public int SpecialityLookupID { get; set; }
        public virtual SpecialityLookup SpecialityLookup { get; set; }
        public int DomainLookupID { get; set; }
        public virtual DomainLookup DomainLookup { get; set; }

        //public List<OutPatientEncounter> OutPatientEncounters { get; set; }
        //public List<InPatientEncounter> InPatientEncounters { get; set; }

    }
}