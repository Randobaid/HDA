using HDA.Core.Models.HDAReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class RefreshViewModel
    {
        public List <DataRefreshProcedure> DataRefreshProcedures { get; set; }
        public List <DataRefreshProcedureStatus> DataRefreshProcedureStatuss { get; set; }

    }
}