using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HDA.Core.Models.HDAReports;

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
        private HDAReportsContext _coreContext = new HDAReportsContext();
        public ActionResult Index()
        {
            var proceduresToExecute = _coreContext.DataRefreshProcedures.ToList();
            return View(proceduresToExecute);
        }
    }
}