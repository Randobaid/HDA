using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;

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
            RefreshViewModel vm = new RefreshViewModel();
            vm.DataRefreshProcedures = _coreContext.DataRefreshProcedure.ToList();
            vm.DataRefreshProcedureStatuss = _coreContext.DataRefreshProcedures.ToList();
            return View(vm);
         
        }
        public ActionResult RunProcedure(string[] procedureIDs)
        {

            //var totolRecordsToMove = _coreContext.Database.SqlQuery<int>(@"SELECT count(PatientPrescriptionId) FROM patientPrescriptions");

            //var prescriptionTotals = _coreContext.Database.SqlQuery<PrescriptionTotal>(@"CALL `hdacore`.`sp_prescriptiontotals`();");

            //return Json(prescriptionTotals, JsonRequestBehavior.AllowGet);
            try
            {
                foreach (string procedureID in procedureIDs)
                {

                    int procedureIdInt = System.Convert.ToInt32(procedureID);
                    DateTime runStatTime = DateTime.MinValue;
                    DateTime runEndTime = DateTime.MinValue;

                    DataRefreshProcedure obj = _coreContext.DataRefreshProcedure.Find(procedureIdInt);

                    DataRefreshProcedureStatus procStatus = new DataRefreshProcedureStatus();

                    var procedureName = obj.ProcedureName;



                    procStatus.ProcedureName = procedureName;
                    procStatus.ProcedureStartime = DateTime.Now;

                    try
                    {

                        _coreContext.Database.ExecuteSqlCommand(procedureName);
                        procStatus.ProcedureStatus = " Succesfully Executed Procedure(s)" ;

                    }
                    catch (Exception ex)
                    {

                        procStatus.ProcedureStatus = "Error:" + ex.Message;
                    }

                    procStatus.ProcedureEndDate = DateTime.Now;
                   

                    _coreContext.DataRefreshProcedures.Add(procStatus);

                }
                _coreContext.SaveChanges();
                RefreshViewModel vm = new RefreshViewModel();
                vm.DataRefreshProcedures = _coreContext.DataRefreshProcedure.ToList();
                vm.DataRefreshProcedureStatuss = _coreContext.DataRefreshProcedures.ToList();
          
                return Json("Completed, Check log for execution status");

            }
            catch (Exception)
            {

                return Json("Ensure Procedures to execute are selected.");
            }
           
           

        
          

            

        }

    }
}