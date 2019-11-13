﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HDA.Core.Models.HDACore;

namespace HDA.Core.Models.HDAReports
{
    public class OutPatientEncounterTotal
    {
        public int OutPatientEncounterTotalID { get; set; }
        public int DomainID { get; set; }
        public int DirectorateID { get; set; }
        public int HealthFacilityTypeID { get; set; }
        public int HealthFacilityID { get; set; }
        public virtual HealthFacility HealthFacility { get; set; }
        public int ProviderID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Total { get; set; }
    }
}