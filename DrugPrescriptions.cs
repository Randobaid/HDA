using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace HDA.Core.Models.HDACore
{
    public class DrugPrescriptions
    {
        public int Patientid { get; set; }
        public int IddrugPrescribed { get; set; }
        public int Providerid { get; set; }
        public DateTime PrescriptionDate { get; set; }

    }
}