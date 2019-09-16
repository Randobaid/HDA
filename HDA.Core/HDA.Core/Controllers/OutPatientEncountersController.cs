using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;

namespace HDA.Core.Controllers
{
    public class OutPatientEncountersController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        //private HDAReportsContext dbb = new HDAReportsContext();
        
        public IHttpActionResult GetMonthlyTotals([FromUri] WorkloadRequest payload)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<MonthlyTotal> monthlyTotals = new List<MonthlyTotal>();
                

                if(payload.HealthFacilityID == 0)
                {
                    var g = from t in db.OutPatientEncounterTotals.Where(h => 
                        h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
                        )
                            group t by new { t.Month } into x
                            select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                    foreach (var total in g)
                    {
                        int totalPrevYear = 0;
                        if (payload.PreviousYear == 1)
                        {
                            var y = from t in db.OutPatientEncounterTotals.Where(h => 
                                             h.Month == total.MonthId
                                             && h.Year == fromDate.Year - 1
                                             )
                                    group t by new { t.Month } into x
                                    select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                            foreach (var t in y)
                            {
                                totalPrevYear = t.Total;
                            }
                        }
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        MonthlyTotal m = new MonthlyTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            Total = total.Total,
                            TotalPreviousYear = totalPrevYear,
                            MonthlyTarget = 100
                        };
                        monthlyTotals.Add(m);
                    }
                    return Ok(monthlyTotals);
                }

                if (payload.ProviderID > 0 && payload.HealthFacilityID > 0)

                {
                    var g = from t in db.OutPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID 
                        && h.ProviderID == payload.ProviderID
                        && h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
                        )
                            group t by new { t.HealthFacilityID, t.Month } into x
                            select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                    foreach (var total in g)
                    {
                        int totalPrevYear = 0;
                        if (payload.PreviousYear == 1)
                        {
                            var y = from t in db.OutPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
                                             && h.ProviderID == payload.ProviderID
                                             && h.Month == total.MonthId
                                             && h.Year == fromDate.Year - 1
                                             )
                                    group t by new { t.HealthFacilityID, t.Month } into x
                                    select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                            foreach (var t in y)
                            {
                                totalPrevYear = t.Total;
                            }
                        }


                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        MonthlyTotal m = new MonthlyTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            Total = total.Total,
                            TotalPreviousYear = totalPrevYear
                        };
                        monthlyTotals.Add(m);

                    }
                    return Ok(monthlyTotals);
                }

                if (payload.HealthFacilityID > 0 && payload.ProviderID == 0)
                {
                    var g = from t in db.OutPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
                        && h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
                        )
                            group t by new { t.HealthFacilityID, t.Month } into x
                        select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                    foreach (var total in g)
                    {
                        int totalPrevYear = 0;
                        if (payload.PreviousYear == 1)
                        {
                            var y = from t in db.OutPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
                                             && h.Month == total.MonthId
                                             && h.Year == fromDate.Year - 1
                                             )
                                            group t by new { t.HealthFacilityID, t.Month } into x
                                            select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                            foreach(var t in y)
                            {
                                totalPrevYear = t.Total;
                            }
                        }
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        MonthlyTotal m = new MonthlyTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            Total = total.Total,
                            TotalPreviousYear = totalPrevYear
                        };
                        monthlyTotals.Add(m);
                    }
                    return Ok(monthlyTotals);
                }

                return BadRequest("Nothing to do");
            }
            return BadRequest(ModelState);
            
        }

        
    }
}
