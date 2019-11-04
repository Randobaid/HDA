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
        public ActionResult RunProcedure(string[] procedureIDs)
        {

            //var totolRecordsToMove = _coreContext.Database.SqlQuery<int>(@"SELECT count(PatientPrescriptionId) FROM patientPrescriptions");

            //var prescriptionTotals = _coreContext.Database.SqlQuery<PrescriptionTotal>(@"CALL `hdacore`.`sp_prescriptiontotals`();");

            //return Json(prescriptionTotals, JsonRequestBehavior.AllowGet);

            foreach (string procedureID in procedureIDs)
            {

                int procedureIdInt = System.Convert.ToInt32(procedureID);

                DataRefreshProcedureStatus obj = _coreContext.DataRefreshProcedures.Find(procedureIdInt);
                
                //Set the start time of the procedure now
                obj.ProcedureStartime = DateTime.Now;

                ///Get the procudure name
                ///


                var procedureName = obj.ProcedureName;

                ///Execute the procedure
                ///
                _coreContext.Database.ExecuteSqlCommand(procedureName);

                obj.ProcedureEndDate = DateTime.Now;
                obj.ProcedureStatus = "Completed";
                

            }
            _coreContext.SaveChanges();
            _coreContext.DataRefreshProcedures.ToList();
            return Json("All Procedures Updated !");

        }

        public string GetProcedureName(int procedureId) {
            DataRefreshProcedureStatus obj = _coreContext.DataRefreshProcedures.Find(procedureId);
            string ProcedureName = obj.ProcedureName;
            return ProcedureName;
        }
    }
}
