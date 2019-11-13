using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class DiagnosisCodingSystem
    {
        public int DiagnosisCodingSystemID { get; set; }
        public string CodingSystemName { get; set; }
        public string CodingSystemVersion { get; set; }
        public DateTime CodingSystemEffectiveDate { get; set; }
    }
}