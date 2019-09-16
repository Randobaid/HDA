using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Workload()
        {
            return View();
        }
        public ActionResult OutpatientPrescriptions()
        {
            return View();
        }
    }
}