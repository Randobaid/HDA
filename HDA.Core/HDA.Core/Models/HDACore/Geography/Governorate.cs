using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class Governorate
    {
        public int GovernorateID { get; set; }
        public int GovernorateCode { get; set; }
        public string GovernorateNameEn { get; set; }
        public string GovernorateNameAr { get; set; }
        public int RegionID { get; set; }
        public virtual Region Region { get; set; }
        public string ShapeFile { get; set; }
    }
}