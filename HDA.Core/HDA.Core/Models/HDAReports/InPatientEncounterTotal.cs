﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class InPatientEncounterTotal
    {
        public int InPatientEncounterTotalID { get; set; }
        public int DomainID { get; set; }
        public int DirectorateID { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public string HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public string ProviderID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string LOSGroup { get; set; }
        public int Total { get; set; }

    }
}