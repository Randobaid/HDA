﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class Drug
    {
        public string DrugID { get; set; }
        //public string DrugNameEn { get; set; }
        //public string DrugNameAr { get; set; }
        public string DrugGenericName { get; set; }
        public int NormalAmountToOrder { get; set; }
        public int DrugEstimatedPrice { get; set; }
        public int DrugClassID { get; set; }
        public virtual DrugClass DrugClass { get; set; }
        public int AntibioticSusceptibilityID { get; set; }
        public int DomainID { get; set; }
    }
}