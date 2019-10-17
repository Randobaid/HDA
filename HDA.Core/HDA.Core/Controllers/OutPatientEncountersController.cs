using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HDA.Core.Utilities;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using LinqKit;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class OutPatientEncountersController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();
        [HttpPost]
        public IHttpActionResult GetMonthlyTotals([FromUri] WorkloadRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<MonthlyTotal> monthlyTotals = new List<MonthlyTotal>();
                List<Target> targets = db.Targets.Where(t =>
                    t.Indicator.IndicatorNameEn == "Outpatient Encounters"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();

                /* Search Predicates */
                var baseOutPatientEncounterTotalSP = PredicateBuilder.New<OutPatientEncounterTotal>();
                baseOutPatientEncounterTotalSP = baseOutPatientEncounterTotalSP.And(a => a.Total > 0);

                var baseHealthFacilitySP = PredicateBuilder.New<HealthFacility>();
                baseHealthFacilitySP = baseHealthFacilitySP.And(a => a.HealthFacilityID > 0);

                var healthFacilitySP_healthFacilityType = PredicateBuilder.New<HealthFacility>();
                var healthFacilitySP_healthFacility = PredicateBuilder.New<HealthFacility>();


                var outpatientEncounterTotalSP = PredicateBuilder.New<OutPatientEncounterTotal>();
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                
                
                
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    outpatientEncounterTotalSP = outpatientEncounterTotalSP.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    healthFacilitySP_healthFacilityType = healthFacilitySP_healthFacilityType.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var selectedHealthFacilitiesSP = PredicateBuilder.New<OutPatientEncounterTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                        healthFacilitySP_healthFacility = healthFacilitySP_healthFacility.Or(a => a.HealthFacilityID == id);
                    }
                }

                /* Search Predicates */

                List<HealthFacility> healthFacilities = db.HealthFacilities.
                    Where(healthFacilitySP_healthFacilityType).
                    Where((healthFacilitySP_healthFacility.IsStarted) ? healthFacilitySP_healthFacility : baseHealthFacilitySP).ToList();
                int NumberOfClinics = healthFacilities.Select(t => t.EstimatedClinics).Sum();


                var g = from t in db.OutPatientEncounterTotals.
                        Where(outpatientEncounterTotalSP).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP: baseOutPatientEncounterTotalSP).
                        Where(h =>
                        h.Month >= fromDate.Month
                        && h.Month <= toDate.Month
                        && h.Year >= fromDate.Year
                        && h.Year <= toDate.Year
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
                    HDAReportsContext newConnection = new HDAReportsContext();
                    if (payload.PreviousYear == 1)
                    {
                        var y = from t in newConnection.OutPatientEncounterTotals.
                                Where(outpatientEncounterTotalSP).
                                Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseOutPatientEncounterTotalSP).
                                Where(h =>
                                         h.Month == total.MonthId
                                         && h.Year == fromDate.Year - 1
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
                    
                    MonthlyTotal m = new MonthlyTotal
                    {
                        Year = total.Year,
                        MonthId = total.MonthId,
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
