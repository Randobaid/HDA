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
        
        public IHttpActionResult GetMonthlyTotals([FromUri] WorkloadRequest payload)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<MonthlyTotal> monthlyTotals = new List<MonthlyTotal>();
                List<Target> targets = db.Targets.Where(t => 
                    t.Indicator.IndicatorNameEn == "Outpatient Encounters"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();

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
                        Target target = new Target();
                        Target targetPrevYear = new Target();
                        foreach(var tgt in targets)
                        {
                            if(tgt.EffectiveDate <= new DateTime(fromDate.Year, total.MonthId, 1))
                            {
                                target = tgt;
                                break;
                            }
                        }
                        int totalPrevYear = 0;
                        HDACoreContext newConnection = new HDACoreContext();
                        if (payload.PreviousYear == 1)
                        {
                            var y = from t in newConnection.OutPatientEncounterTotals.Where(h => 
                                             h.Month == total.MonthId
                                             && h.Year == fromDate.Year - 1
                                             )
                                    group t by new { t.Month } into x
                                    select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                            foreach (var t in y)
                            {
                                totalPrevYear = t.Total;
                            }
                            foreach(var tgt in targets)
                            {
                                if(tgt.EffectiveDate <= new DateTime(fromDate.Year - 1, total.MonthId, 1))
                                {
                                    targetPrevYear = tgt;
                                    break;
                                }
                            }
                        }
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        MonthlyTotal m = new MonthlyTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            Total = total.Total,
                            TotalPreviousYear = totalPrevYear,
                            Target = target.Value,
                            TargetPreviousYear = targetPrevYear.Value
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
                        Target target = new Target();
                        Target targetPrevYear = new Target();
                        foreach(var tgt in targets)
                        {
                            if(
                                tgt.EffectiveDate <= new DateTime(fromDate.Year, total.MonthId, 1)
                                && tgt.ProviderID == payload.ProviderID
                                && tgt.HealthFacilityID == payload.HealthFacilityID
                            )
                            {
                                target = tgt;
                                break;
                            }
                        }
                        int totalPrevYear = 0;
                        if (payload.PreviousYear == 1)
                        {
                            HDACoreContext newConnection = new HDACoreContext();
                            var y = from t in newConnection.OutPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
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
                            foreach(var tgt in targets)
                            {
                                if(
                                    tgt.EffectiveDate <= new DateTime(fromDate.Year - 1, total.MonthId, 1)
                                    && tgt.ProviderID == payload.ProviderID
                                    && tgt.HealthFacilityID == payload.HealthFacilityID
                                )
                                {
                                    targetPrevYear = tgt;
                                    break;
                                }
                            }
                        }

                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        MonthlyTotal m = new MonthlyTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            Total = total.Total,
                            TotalPreviousYear = totalPrevYear,
                            Target = target.Value,
                            TargetPreviousYear = targetPrevYear.Value,
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
                        Target target = new Target();
                        Target targetPrevYear = new Target();
                        foreach(var tgt in targets)
                        {
                            if(
                                tgt.EffectiveDate <= new DateTime(fromDate.Year, total.MonthId, 1)
                                && tgt.HealthFacilityID == payload.HealthFacilityID
                            )
                            {
                                target = tgt;
                                break;
                            }
                        }
                        int totalPrevYear = 0;
                        if (payload.PreviousYear == 1)
                        {
                            HDACoreContext newConnection = new HDACoreContext();
                            var y = from t in newConnection.OutPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
                                             && h.Month == total.MonthId
                                             && h.Year == fromDate.Year - 1
                                             )
                                            group t by new { t.HealthFacilityID, t.Month } into x
                                            select new { MonthId = x.Key.Month, Total = x.Sum(t => t.Total) };
                            foreach(var t in y)
                            {
                                totalPrevYear = t.Total;
                            }
                            foreach(var tgt in targets)
                            {
                                if(
                                    tgt.EffectiveDate <= new DateTime(fromDate.Year - 1, total.MonthId, 1)
                                    && tgt.HealthFacilityID == payload.HealthFacilityID
                                )
                                {
                                    targetPrevYear = tgt;
                                    break;
                                }
                            }
                        }
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        MonthlyTotal m = new MonthlyTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            Total = total.Total,
                            TotalPreviousYear = totalPrevYear,
                            Target = target.Value,
                            TargetPreviousYear = targetPrevYear.Value
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
