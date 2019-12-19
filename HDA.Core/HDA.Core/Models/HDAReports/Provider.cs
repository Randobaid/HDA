using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class Provider
    {
        public string ProviderID { get; set; }
        [Index]
        public int? MasterProviderIndex { get; set; }
        public string JobID { get; set; }
        public string ProviderType { get; set; }
        public string NationalID { get; set; }
        public string ProviderNameEn { get; set; }
        public string ProviderNameAr { get; set; }
        public int SpecialtyID { get; set; }
        //public int SpecialityLookupID { get; set; }
        //public virtual SpecialityLookup SpecialityLookup { get; set; }
        public int DomainID { get; set; }
        public virtual Domain Domain { get; set; }
    }
}