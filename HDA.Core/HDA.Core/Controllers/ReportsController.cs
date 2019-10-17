using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using HDA.Core.Utilities;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Workload()
        {
            if (new PermissionCheck().IsAllowedOnReport("Provider Workload", User.Identity.GetUserId()) == true) {
                return View();
            } else {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
        public ActionResult OutpatientPrescriptions()
        {
            if (new PermissionCheck().IsAllowedOnReport("Outpatient Prescriptions", User.Identity.GetUserId()) == true) {
                return View();
            } else {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
        public ActionResult DiseaseManagement()
        {
            if (new PermissionCheck().IsAllowedOnReport("Disease Management", User.Identity.GetUserId()) == true) {
                return View();
            } else {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);
            }
        }
    }

    [System.Web.Http.Authorize]
    public class ApiReportsController : ApiController
    {
        [System.Web.Http.Route("api/reports")]
        [System.Web.Http.HttpGet]
        public IHttpActionResult Index()
        {
            HDAReportsContext db = new HDAReportsContext();
            List<ReportViewModel> reports = new List<ReportViewModel>();
            foreach (var report in db.Reports)
            {
                ReportViewModel i = new ReportViewModel {
                    ReportID = report.ReportID,
                    ReportCode = report.ReportCode,
                    ReportNameEn = report.ReportNameEn,
                    ReportNameAr = report.ReportNameAr,
                };
                reports.Add(i);
            }
            return Ok(reports);
        }
    }
}