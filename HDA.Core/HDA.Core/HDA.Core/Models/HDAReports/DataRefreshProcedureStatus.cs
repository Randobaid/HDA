using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAReports
{
    public class DataRefreshProcedureStatus
    {
        public int Id { get; set; }
        public string ProcedureName { get; set; }

        public DateTime ProcedureStartime { get; set; }

        public DateTime ProcedureEndDate { get; set; }

        public string ProcedureStatus { get; set; }
    }
}