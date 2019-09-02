using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class Patient
    {
        public int PatientID { get; set; }
        [Index]
        public int SourceID { get; set; }
        public DateTime DoB { get; set; }
        public string SSN { get; set; }
        public int GenderLookupID { get; set; }
        public virtual GenderLookup GenderLookup { get; set; }
        public int? MaritalStatusLookupID { get; set; }
        public virtual MaritalStatusLookup MaritalStatusLookup { get; set; }
        public int? CountryID { get; set; }
        public virtual Country Country { get; set; }

        //public List<OutPatientEncounter> OutPatientEncounters { get; set; }
        //public List<InPatientEncounter> InPatientEncounters { get; set; }

    }
}