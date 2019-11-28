using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class ProcedureLookup
    {
        public int ProcedureLookupID { get; set; }
        public string ProcedureCode { get; set; }

        public List<SurgeryEncounter> SurgeryEncounters { get; set; }
    }
}