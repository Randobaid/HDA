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
    public class SurgeriesController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        public IHttpActionResult GetMonthlyTotalsBySeverity([FromUri] WorkloadRequest payload)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                    DateTime toDate = Convert.ToDateTime(payload.ToDate);
                    List<SurgeryBySeverityTotal> result = new List<SurgeryBySeverityTotal>();
                    List<Target> targets = db.Targets.Where(t => 
                        t.Indicator.IndicatorNameEn == "Surgeries"
                    ).OrderByDescending(tg => tg.EffectiveDate).ToList();

                    if(payload.HealthFacilityID == 0)
                    {
                        var a = from b in db.SurgeryTotals.Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year)
                                group b by b.Month into c
                                select new
                                {
                                    MonthId = c.Key
                                ,
                                    MinorSeverityTotal = c.Where(d => d.SurgerySeverityID == 2).Sum(t => t.Total)
                                ,
                                    MajorSeverityTotal = c.Where(d => d.SurgerySeverityID == 1).Sum(t => t.Total)
                                //,
                                    //UndefinedSeverityTotal = c.Where(d => d.SurgerySeverityID == 3).Sum(t => t.Total)
                                };
                        foreach (var total in a)
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
                            SurgeryBySeverityTotal m = new SurgeryBySeverityTotal
                            {
                                MonthId = total.MonthId,
                                MonthName = d.GetMonthName(total.MonthId),
                                MinorSeverityTotal = total.MinorSeverityTotal,
                                MajorSeverityTotal = total.MajorSeverityTotal,
                                //UndefinedSeverityTotal = total.UndefinedSeverityTotal,
                                Target = target.Value
                            };
                            result.Add(m);
                        }
                        return Ok(result);
                    }

                    if (payload.ProviderID > 0 && payload.HealthFacilityID > 0)
                    {
                        var a = from b in db.SurgeryTotals.Where(
                            h => h.HealthFacilityID == payload.HealthFacilityID
                            && h.ProviderID == payload.ProviderID
                            && h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year)
                                group b by new { b.HealthFacilityID, b.Month } into c
                                select new
                                {
                                    MonthId = c.Key.Month
                                ,
                                    MinorSeverityTotal = c.Where(d => d.SurgerySeverityID == 2).Sum(t => t.Total)
                                ,
                                    MajorSeverityTotal = c.Where(d => d.SurgerySeverityID == 1).Sum(t => t.Total)
                                //,
                                    //UndefinedSeverityTotal = c.Where(d => d.SurgerySeverityID == 3).Sum(t => t.Total)
                                };
                        foreach (var total in a)
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
                            SurgeryBySeverityTotal m = new SurgeryBySeverityTotal
                            {
                                MonthId = total.MonthId,
                                MonthName = d.GetMonthName(total.MonthId),
                                MinorSeverityTotal = total.MinorSeverityTotal,
                                MajorSeverityTotal = total.MajorSeverityTotal,
                                //UndefinedSeverityTotal = total.UndefinedSeverityTotal,
                                Target = target.Value
                            };
                            result.Add(m);
                        }
                        return Ok(result);
                    }
                    if(payload.ProviderID == 0 && payload.HealthFacilityID > 0)
                    {
                        var a = from b in db.SurgeryTotals.Where(
                            h => h.HealthFacilityID == payload.HealthFacilityID
                            && h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year)
                                group b by new { b.HealthFacilityID, b.Month } into c
                                select new
                                {
                                    MonthId = c.Key.Month
                                ,
                                    MinorSeverityTotal = c.Where(d => d.SurgerySeverityID == 2).Sum(t => t.Total)
                                ,
                                    MajorSeverityTotal = c.Where(d => d.SurgerySeverityID == 1).Sum(t => t.Total)
                                //,
                                    //UndefinedSeverityTotal = c.Where(d => d.SurgerySeverityID == 3).Sum(t => t.Total)
                                };
                        foreach(var total in a)
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
                            SurgeryBySeverityTotal m = new SurgeryBySeverityTotal
                            {
                                MonthId = total.MonthId,
                                MonthName = d.GetMonthName(total.MonthId),
                                MinorSeverityTotal = total.MinorSeverityTotal,
                                MajorSeverityTotal = total.MajorSeverityTotal,
                                //UndefinedSeverityTotal = total.UndefinedSeverityTotal,
                                Target = target.Value
                            };
                            result.Add(m);
                        }
                        return Ok(result);
                    }

                    return BadRequest("No directive");
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest(ModelState);
        }
    }
}

