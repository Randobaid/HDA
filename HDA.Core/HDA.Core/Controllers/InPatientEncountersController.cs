using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDAReports;
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
    [Authorize]
    public class InPatientEncountersController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        public IHttpActionResult GetMonthlyTotals([FromUri] WorkloadRequest payload)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<InPatientLOSTotal> monthlyTotals = new List<InPatientLOSTotal>();
                List<Target> targets = db.Targets.Where(t => 
                    t.Indicator.IndicatorNameEn == "Inpatient Transfers"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();

                if(payload.HealthFacilityID == 0)
                {
                    var g = from t in db.InPatientEncounterTotals.Where(h => 
                        h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
                        )
                            group t by new { t.Month } into x
                            select new
                            {
                                MonthId = x.Key.Month
                            ,
                                Total13 = x.Where(w => w.LOSGroup == "1-3").Sum(t => t.Total)
                            ,
                                Total47 = x.Where(w => w.LOSGroup == "4-7").Sum(t => t.Total)
                            ,
                                Total8Plus = x.Where(w => w.LOSGroup == "8+").Sum(t => t.Total)
                            ,
                                TotalNotDischarged = x.Where(w => w.LOSGroup == "ND").Sum(t => t.Total)
                            };
                    foreach (var total in g)
                    {
                        Target target = new Target();
                        foreach(var tgt in targets)
                        {
                            if(tgt.EffectiveDate <= new DateTime(fromDate.Year, total.MonthId, 1))
                            {
                                target = tgt;
                                break;
                            }
                        }
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        InPatientLOSTotal m = new InPatientLOSTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            LOS13Total = total.Total13,
                            LOS47Total = total.Total47,
                            LOS8Total = total.Total8Plus,
                            LOSNDTotal = total.TotalNotDischarged,
                            Target = target.Value
                        };
                        monthlyTotals.Add(m);
                    }
                    return Ok(monthlyTotals);

                }

                if (payload.ProviderID > 0 && payload.HealthFacilityID > 0)
                {
                    var g = from t in db.InPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
                            && h.ProviderID >= payload.ProviderID
                            && h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                        )
                            group t by new { t.HealthFacilityID, t.Month } into x
                            select new
                            {
                                MonthId = x.Key.Month
                            ,
                                Total13 = x.Where(w => w.LOSGroup == "1-3").Sum(t => t.Total)
                            ,
                                Total47 = x.Where(w => w.LOSGroup == "4-7").Sum(t => t.Total)
                            ,
                                Total8Plus = x.Where(w => w.LOSGroup == "8+").Sum(t => t.Total)
                            };
                    foreach (var total in g)
                    {
                        Target target = new Target();
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
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        InPatientLOSTotal m = new InPatientLOSTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            LOS13Total = total.Total13,
                            LOS47Total = total.Total47,
                            LOS8Total = total.Total8Plus,
                            Target = target.Value
                        };
                        monthlyTotals.Add(m);
                    }
                    return Ok(monthlyTotals);
                }

                if (payload.ProviderID == 0 && payload.HealthFacilityID > 0)
                {
                    var g = from t in db.InPatientEncounterTotals.Where(h => h.HealthFacilityID == payload.HealthFacilityID
                        && h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
                        )
                            group t by new { t.HealthFacilityID, t.Month } into x
                            select new { MonthId = x.Key.Month
                            , Total13 = x.Where(w=> w.LOSGroup == "1-3").Sum(t => t.Total)
                            , Total47 = x.Where(w => w.LOSGroup == "4-7").Sum(t => t.Total)
                            , Total8Plus = x.Where(w => w.LOSGroup == "8+").Sum(t => t.Total)
                            };
                    foreach (var total in g)
                    {
                        Target target = new Target();
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
                        DateTimeFormatInfo d = new DateTimeFormatInfo();
                        InPatientLOSTotal m = new InPatientLOSTotal
                        {
                            MonthId = total.MonthId,
                            MonthName = d.GetMonthName(total.MonthId),
                            LOS13Total = total.Total13,
                            LOS47Total = total.Total47,
                            LOS8Total = total.Total8Plus,
                            Target = target.Value
                        };
                        monthlyTotals.Add(m);
                    }
                    return Ok(monthlyTotals);
                }

                return BadRequest("No directive");
            }
            return BadRequest(ModelState);

        }
    }
}
