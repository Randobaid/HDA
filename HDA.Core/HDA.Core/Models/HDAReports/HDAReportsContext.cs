using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class HDAReportsContext : DbContext
    {
        public HDAReportsContext() : base("hdareports") { }

        public DbSet<OutPatientEncounterTotal> OutPatientEncounterTotals { get; set; }
        public DbSet<InPatientEncounterTotal> InPatientEncounterTotals { get; set; }
        public DbSet<SurgeryTotal> SurgeryTotals { get; set; }
        public DbSet<PrescriptionTotal> PrescriptionTotals { get; set; }
    }
}