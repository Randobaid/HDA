using HDA.Core.Models.HDACore;
using HDA.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HDA.Core.Controllers
{
    public class OutPatientPrescriptionsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        public IHttpActionResult GetPrescriptionsPerInstitution([FromUri] OPRequest payload)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();
                var g = from t in db.PrescriptionTotals.Where(h =>
                h.Month >= fromDate.Month
                && h.Month <= toDate.Month
                && h.Year >= fromDate.Year
                && h.Year <= toDate.Year)
                        group t by new { t.Month, t.Year } into x
                        select new
                        {
                            x.Key.Year,
                            MonthId = x.Key.Month,
                            Total = x.Sum(t => t.Total)
                        };
                foreach(var total in g)
                {
                    DateTimeFormatInfo d = new DateTimeFormatInfo();
                    PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                    {
                        Year = total.Year,
                        MonthId = total.MonthId,
                        MonthName = d.GetMonthName(total.MonthId),
                        DrugClass = 0,
                        DrugId = 0,
                        Total = total.Total
                    };
                    monthlyTotals.Add(p);
                }

                return Ok(monthlyTotals);
            }
            else
            {
                return BadRequest(ModelState);
            }

            
        }
    }
}
