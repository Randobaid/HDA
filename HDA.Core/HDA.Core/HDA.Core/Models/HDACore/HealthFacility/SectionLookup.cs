using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class SectionLookup
    {
        public int SectionLookupID { get; set; }
        public int? ParentSectionID { get; set; }
        public string SectionNameEn { get; set; }
        public string SectionNameAr { get; set; }
    }
}