using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class DiagnosisLookup
    {
        public int DiagnosisLookupID { get; set; }
        public string DiagnosisCode { get; set; }

        //public List<OutPatientEncounter> OutPatientEncounters { get; set; }
        //public List<InPatientEncounter> InPatientEncounters { get; set; }
    }
}