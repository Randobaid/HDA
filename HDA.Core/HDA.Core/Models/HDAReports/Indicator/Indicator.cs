using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class Indicator
    {
        public int IndicatorID { get; set; }
        public string IndicatorShortName { get; set; }
        public string IndicatorNameEn { get; set; }
        public string IndicatorNameAr { get; set; }
    }
}