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
    public class InPatientEncountersController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        [HttpPost]
        public IHttpActionResult GetMonthlyTotals([FromUri] InPatientEncountersRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<InPatientLOSTotal> monthlyTotals = new List<InPatientLOSTotal>();
                List<Target> targets = db.Targets.Where(t =>
                    t.Indicator.IndicatorNameEn == "Inpatient Transfers"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();

                var healthfacilityTypes = PredicateBuilder.New<InPatientEncounterTotal>();
                var healthfacilitiesSearchPredicate = PredicateBuilder.New<HealthFacility>();
                foreach (SelectedFacilityType s in selectedFacilityTypes)
                {
                    healthfacilityTypes = healthfacilityTypes.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    healthfacilitiesSearchPredicate = healthfacilitiesSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }


                List<HealthFacility> healthFacilities = db.HealthFacilities.
                    Where(healthfacilitiesSearchPredicate).
                    Where(h => (payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true).ToList();
                int NumberOfBeds = healthFacilities.Select(t => t.EstimatedBeds).Sum();

                var g = from t in db.InPatientEncounterTotals.
                        Where(healthfacilityTypes).
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
                            Total13 = x.Where(w => w.LOSGroup == "1-3").Sum(t => t.Total),
                            Total47 = x.Where(w => w.LOSGroup == "4-7").Sum(t => t.Total),
                            Total8Plus = x.Where(w => w.LOSGroup == "8+").Sum(t => t.Total),
                            TotalNotDischarged = x.Where(w => w.LOSGroup == "ND").Sum(t => t.Total)
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
