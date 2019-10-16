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
    public class InPatientEncountersController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();
        [HttpPost]
        public IHttpActionResult GetMonthlyTotals([FromUri] InPatientEncountersRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<InPatientLOSTotal> monthlyTotals = new List<InPatientLOSTotal>();
                List<Target> targets = db.Targets.Where(t =>
                    t.Indicator.IndicatorNameEn == "Inpatient Transfers"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();


                /* Search Predicates */
                var baseOutPatientEncounterTotalSP = PredicateBuilder.New<InPatientEncounterTotal>();
                baseOutPatientEncounterTotalSP = baseOutPatientEncounterTotalSP.And(a => a.Total > 0);

                var baseHealthFacilitySP = PredicateBuilder.New<HealthFacility>();
                baseHealthFacilitySP = baseHealthFacilitySP.And(a => a.HealthFacilityID > 0);

                var healthFacilitySP_healthFacilityType = PredicateBuilder.New<HealthFacility>();
                var healthFacilitySP_healthFacility = PredicateBuilder.New<HealthFacility>();


                var outpatientEncounterTotalSP = PredicateBuilder.New<InPatientEncounterTotal>();
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());



                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    outpatientEncounterTotalSP = outpatientEncounterTotalSP.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    healthFacilitySP_healthFacilityType = healthFacilitySP_healthFacilityType.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var selectedHealthFacilitiesSP = PredicateBuilder.New<InPatientEncounterTotal>();
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

                int NumberOfBeds = healthFacilities.Select(t => t.EstimatedBeds).Sum();

                var g = from t in db.InPatientEncounterTotals.
                        Where(outpatientEncounterTotalSP).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseOutPatientEncounterTotalSP).
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
                            Total13 = x.Where(w => w.LOSGroup == "1-3").Sum(t => (int?)t.Total) ?? 0,
                            Total47 = x.Where(w => w.LOSGroup == "4-7").Sum(t => (int?)t.Total) ?? 0,
                            Total8Plus = x.Where(w => w.LOSGroup == "8+").Sum(t => (int?)t.Total ?? 0),
                            TotalNotDischarged = x.Where(w => w.LOSGroup == "ND").Sum(t => (int?)t.Total ?? 0)
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
                    InPatientLOSTotal m = new InPatientLOSTotal
                    {
                        Year = total.Year,
                        MonthId = total.MonthId,
                        LOS13Total = total.Total13,
                        LOS47Total = total.Total47,
                        LOS8Total = total.Total8Plus,
                        LOSNDTotal = total.TotalNotDischarged,
                        Target = Math.Ceiling(Convert.ToDouble(target.Value) * NumberOfBeds)
                    };
                    monthlyTotals.Add(m);
                }
                return Ok(monthlyTotals);
            }
            return BadRequest(ModelState);
        }
    }
}
