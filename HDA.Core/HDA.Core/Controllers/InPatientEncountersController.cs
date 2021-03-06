﻿using HDA.Core.Utilities;
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
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<InPatientEncounterTotal>();
                foreach (string healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                List<InPatientLOSTotal> monthlyTotals = new List<InPatientLOSTotal>();
                List<Target> targets = db.Targets.Where(t =>
                    t.Indicator.IndicatorNameEn == "Inpatient Transfers"
                ).OrderByDescending(tg => tg.EffectiveDate).ToList();


                /* Search Predicates */
                var baseInPatientEncounterTotalSP = PredicateBuilder.New<InPatientEncounterTotal>();
                baseInPatientEncounterTotalSP = baseInPatientEncounterTotalSP.And(a => a.Total > 0);

                var baseHealthFacilitySP = PredicateBuilder.New<HealthFacility>();
                baseHealthFacilitySP = baseHealthFacilitySP.And(a => a.HealthFacilityID.Length > 0);

                var healthFacilitySP_healthFacilityType = PredicateBuilder.New<HealthFacility>();
                var healthFacilitySP_healthFacility = PredicateBuilder.New<HealthFacility>();


                var inpatientEncounterTotalSP = PredicateBuilder.New<InPatientEncounterTotal>();

                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    inpatientEncounterTotalSP = inpatientEncounterTotalSP.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    healthFacilitySP_healthFacilityType = healthFacilitySP_healthFacilityType.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var selectedHealthFacilitiesSP = PredicateBuilder.New<InPatientEncounterTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (string id in selectedFacilitiesPayload.HealthFacilities)
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

                //var NewFromDate = new DateTime(fromDate.Year, fromDate.Month, 1);
                //var NewToDate = new DateTime(toDate.Year, toDate.Month, 1);
                var g = from t in db.InPatientEncounterTotals.
                        Where(inpatientEncounterTotalSP).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseInPatientEncounterTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            //h.FirstMonthDate >= NewFromDate
                            //&& h.FirstMonthDate <= NewToDate
                            && ((payload.ProviderID.Length > 0) ? h.ProviderID == payload.ProviderID : true))
                        group t by new { t.Year, t.Month } into x
                        select new
                        {
                            x.Key.Year,
                            MonthId = x.Key.Month,
                            Total13 = x.Where(w => w.LOSGroup == "1-3").Sum(t => (int?)t.Total) ?? 0,
                            Total47 = x.Where(w => w.LOSGroup == "4-7").Sum(t => (int?)t.Total) ?? 0,
                            Total8Plus = x.Where(w => w.LOSGroup == "8+").Sum(t => (int?)t.Total) ?? 0,
                            TotalNotDischarged = x.Where(w => w.LOSGroup == "ND").Sum(t => (int?)t.Total) ?? 0

                        };
                try
                {
                    foreach (var total in g.OrderBy(d => new { d.Year, d.MonthId }))
                    {
                        Target target = new Target();
                        //to be filled only once not inside the data set
                        foreach (var tgt in targets)
                        {
                            if (tgt.EffectiveDate <= new DateTime(fromDate.Year, fromDate.Month, 1))
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
                }
                catch(Exception e)
                {
                    Console.WriteLine("Operation cancelled");
                }

                return Ok(monthlyTotals);
            }
            return BadRequest(ModelState);
        }
    }
}
