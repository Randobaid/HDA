using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.App_Code;
using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using LinqKit;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Controllers
{
    public class OutPatientEncountersController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        [HttpPost]
        public IHttpActionResult GetMonthlyTotals([FromUri] WorkloadRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<MonthlyTotal> monthlyTotals = new List<MonthlyTotal>();
                List<Target> targets = db.Targets.Where(t =>
                    t.Indicator.IndicatorNameEn == "Outpatient Encounters"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();

                var searchPredicate = PredicateBuilder.New<OutPatientEncounterTotal>();
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                //searchPredicate = searchPredicate.And(f => allowedHealthFacilityIDs.Contains(f.HealthFacilityID));
                var anotherSP = PredicateBuilder.New<HealthFacility>();
                foreach (SelectedFacilityType s in selectedFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    anotherSP =
                      anotherSP.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }
                List<HealthFacility> healthFacilities = db.HealthFacilities.
                    Where(anotherSP).
                    Where(h => (payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true).ToList();
                int NumberOfClinics = healthFacilities.Select(t => t.EstimatedClinics).Sum();
                var g = from t in db.OutPatientEncounterTotals.
                        Where(searchPredicate).
                        Where(h =>
                        h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
                        && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                        && ((payload.ProviderID > 0) ? h.ProviderID == payload.ProviderID : true))

                        group t by new { t.Year, t.Month } into x
                        select new
                        {
                            x.Key.Year,
                            MonthId = x.Key.Month,
                            Total = x.Sum(t => t.Total),
                        };
                foreach (var total in g)
                {
                    Target target = new Target();
                    Target targetPrevYear = new Target();
                    foreach (var tgt in targets)
                    {
                        if (tgt.EffectiveDate <= new DateTime(fromDate.Year, total.MonthId, 1))
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
                                         && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                                         && ((payload.ProviderID > 0) ? h.ProviderID == payload.ProviderID : true)
                                         )
                                group t by new { t.Year, t.Month } into x
                                select new
                                {
                                    x.Key.Year,
                                    MonthId = x.Key.Month,
                                    Total = x.Sum(t => t.Total),
                                    
                                };
                        foreach (var t in y)
                        {
                            totalPrevYear = t.Total;
                        }
                        foreach (var tgt in targets)
                        {
                            if (tgt.EffectiveDate <= new DateTime(fromDate.Year - 1, total.MonthId, 1))
                            {
                                targetPrevYear = tgt;
                                break;
                            }
                        }
                    }
                    DateTimeFormatInfo d = new DateTimeFormatInfo();
                    MonthlyTotal m = new MonthlyTotal
                    {
                        Year = total.Year,
                        MonthId = total.MonthId,
                        MonthName = d.GetMonthName(total.MonthId),
                        Total = total.Total,
                        TotalPreviousYear = totalPrevYear,
                        Target = Convert.ToInt32(target.Value) * NumberOfClinics ,
                        TargetPreviousYear = Convert.ToInt32(targetPrevYear.Value) * NumberOfClinics
                    };
                    monthlyTotals.Add(m);
                }
                return Ok(monthlyTotals);
            }

            return BadRequest(ModelState);
        }

    }
}
