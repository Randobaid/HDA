using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class DiagnosisCode
    {
        public int DiagnosisCodeID { get; set; }
        public string Code { get; set; }
        public int DiagnosisCodingSystemID { get; set; }
        public virtual DiagnosisCodingSystem DiagnosisCodingSystem { get; set; }
        public string DiagnosisNameEn { get; set; }
        public string DiagnosisNameAr { get; set; }
    }
}