using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using LinqKit;
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
        [HttpPost]
        public IHttpActionResult GetMonthlyTotalsBySeverity([FromUri] WorkloadRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
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

                    var healthfacilityTypes = PredicateBuilder.New<SurgeryTotal>();
                    
                    foreach (SelectedFacilityType s in selectedFacilityTypes)
                    {
                        healthfacilityTypes = healthfacilityTypes.Or(z => z.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    }

                    var a = from b in db.SurgeryTotals.
                            Where(healthfacilityTypes).
                            Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                            && ((payload.ProviderID > 0) ? h.ProviderID == payload.ProviderID : true))
                            group b by new { b.Year, b.Month } into c
                            select new
                            {
                                c.Key.Year,
                                MonthId = c.Key.Month,
                                MinorSeverityTotal = c.Where(d => d.SurgerySeverityID == 2).Sum(t => t.Total),
                                MajorSeverityTotal = c.Where(d => d.SurgerySeverityID == 1).Sum(t => t.Total)
                            };
                    foreach (var total in a.OrderBy(d => new { d.Year, d.MonthId }))
                    {
                        Target target = new Target();
                        foreach (var tgt in targets)
                        {
                            if (tgt.EffectiveDate <= new DateTime(fromDate.Year, total.MonthId, 1))
                            {
                                target = tgt;
                                break;
                            }
                        }
                        
                        SurgeryBySeverityTotal m = new SurgeryBySeverityTotal
                        {
                            Year = total.Year,
                            MonthId = total.MonthId,
                            MinorSeverityTotal = total.MinorSeverityTotal,
                            MajorSeverityTotal = total.MajorSeverityTotal,
                            Target = target.Value
                        };
                        result.Add(m);
                    }
                    return Ok(result);
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

