﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class DrugClass
    {
        public int DrugClassID { get; set; }
        public string DrugClassCode { get; set; }
        public int DrugParentClassID { get; set; }
        public string DrugClassNameEn { get; set; }
        public string DrugClassNameAr { get; set; }
        public string DrugClassCategory { get; set; }
    }
}