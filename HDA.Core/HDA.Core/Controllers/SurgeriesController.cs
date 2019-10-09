using HDA.Core.App_Code;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using LinqKit;
using Microsoft.AspNet.Identity;
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
        private HDAReportsContext db = new HDAReportsContext();
        [HttpPost]
        public IHttpActionResult GetMonthlyTotalsBySeverity([FromUri] WorkloadRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
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

                    var selectedFacilitiesPayload = selectedFacilityPayload.First();

                    /* Search Predicates */
                    var baseSurgeriesTotalSP = PredicateBuilder.New<SurgeryTotal>();
                    baseSurgeriesTotalSP = baseSurgeriesTotalSP.And(a => a.Total > 0);

                    

                    var healthFacilitySP_healthFacilityType = PredicateBuilder.New<HealthFacility>();
                    var healthFacilitySP_healthFacility = PredicateBuilder.New<HealthFacility>();


                    var SurgeriesTotalSP = PredicateBuilder.New<SurgeryTotal>();
                    var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());



                    foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                    {
                        SurgeriesTotalSP = SurgeriesTotalSP.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                        healthFacilitySP_healthFacilityType = healthFacilitySP_healthFacilityType.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    }

                    var selectedHealthFacilitiesSP = PredicateBuilder.New<SurgeryTotal>();
                    if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                    {
                        foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                        {
                            selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                            healthFacilitySP_healthFacility = healthFacilitySP_healthFacility.Or(a => a.HealthFacilityID == id);
                        }
                    }

                    /* Search Predicates */

                    

                    var g = from b in db.SurgeryTotals.
                            Where(SurgeriesTotalSP).
                            Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseSurgeriesTotalSP).
                            Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            && ((payload.ProviderID > 0) ? h.ProviderID == payload.ProviderID : true))
                            group b by new { b.Year, b.Month } into c
                            select new
                            {
                                c.Key.Year,
                                MonthId = c.Key.Month,
                                MinorSeverityTotal = c.Where(d => d.SurgerySeverityID == 2).Sum(t => t.Total),
                                MajorSeverityTotal = c.Where(d => d.SurgerySeverityID == 1).Sum(t => t.Total)
                            };
                    foreach (var total in g.OrderBy(d => new { d.Year, d.MonthId }))
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

