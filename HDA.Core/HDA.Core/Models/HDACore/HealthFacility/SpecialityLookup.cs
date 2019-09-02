using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class SpecialityLookup
    {
        public int SpecialityLookupID { get; set; }
        public int SectionLookupID { get; set; }
        public virtual SectionLookup SectionLookup { get; set; }
        public string SpecialityNameEn { get; set; }
        public string SpecialityNameAr { get; set; }
    }
}