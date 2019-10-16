using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class GovernorateViewModel
    {
        public int GovernorateID { get; set; }
        public int GovernorateCode { get; set; }
        public string GovernorateNameEn { get; set; }
        public string GovernorateNameAr { get; set; }
        public int RegionID { get; set; }
        public string ShapeFile { get; set; }
    }
}