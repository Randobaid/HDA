using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDACore.RefreshData;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AuthorizeAttribute = System.Web.Http.AuthorizeAttribute;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class RefreshDataController : Controller
    {
        //private HDACoreContext coreContext = new HDACoreContext();
        //// GET: Reports
        //public ActionResult RefreshPrescriptionTotals(string spName) {
        //    int noOfrowsInserted = coreContext.Database.ExecuteSqlCommand(spName);
        //   return View(noOfrowsInserted);
        //}
        private HDACoreContext _coreContext = new HDACoreContext();
        public ActionResult Index()
        {
            var proceduresToExecute = _coreContext.DataRefreshProcedures.ToList();
            return View(proceduresToExecute);
        }

    }
}
