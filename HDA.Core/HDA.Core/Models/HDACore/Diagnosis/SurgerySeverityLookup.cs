using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class SurgerySeverityLookup
    {
        public int SurgerySeverityLookupID { get; set; }
        public string SeverityEn { get; set; }
        public string SeverityAr { get; set; }


        public List<SurgeryEncounter> SurgeryEncounters { get; set; }
    }
}