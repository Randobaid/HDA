using HDA.Core.App_Code;
using HDA.Core.Models.HDACore;
using HDA.Core.ViewModels;
using LinqKit;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class OutPatientPrescriptionsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        [HttpPost]
        public IHttpActionResult GetPrescriptionsPerInstitution([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                searchPredicate = searchPredicate.And(f => allowedHealthFacilityIDs.Contains(f.HealthFacilityID));

                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);

                    }
                }


                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();

                if (selectedFacilitiesPayload.HealthFacilities.Count == 0 || selectedFacilitiesPayload.HealthFacilities.Count > 5)
                {
                    var g = from t in db.PrescriptionTotals.
                            Where(searchPredicate).
                            Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                            Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                                && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true))
                            group t by new { t.Year, t.Month } into x
                            select new
                            {
                                x.Key.Year,
                                MonthId = x.Key.Month,
                                Total = x.Sum(t => (int?)t.TotalPrescriptions) ?? 0
                            };
                    foreach (var total in g.OrderBy(d => new { d.Year, d.MonthId }))
                    {
                        PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                        {
                            Year = total.Year,
                            MonthId = total.MonthId,
                            Total = total.Total
                        };
                        monthlyTotals.Add(p);
                    }
                }
                else
                {
                    var g = from t in db.PrescriptionTotals.
                            Where(searchPredicate).
                            Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                            Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                                && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true))
                            group t by new {t.HealthFacility.HealthFacilityNameEn, t.HealthFacilityID, t.Year, t.Month } into x
                            select new
                            {
                                HealthFacilityName = x.Key.HealthFacilityNameEn,
                                x.Key.HealthFacilityID,
                                x.Key.Year,
                                MonthId = x.Key.Month,
                                Total = x.Sum(t => (int?)t.TotalPrescriptions) ?? 0
                            };
                    foreach (var total in g.OrderBy(d => new { d.Year, d.MonthId }))
                    {
                        PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                        {
                            HealthFacilityName = total.HealthFacilityName,
                            HealthFacilityID = total.HealthFacilityID,
                            Year = total.Year,
                            MonthId = total.MonthId,
                            Total = total.Total
                        };
                        monthlyTotals.Add(p);
                    }
                }

                return Ok(monthlyTotals);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IHttpActionResult GetPrescriptionsPerPharmacy([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            HDACoreContext pharmDb = new HDACoreContext();
            if (ModelState.IsValid)
            {
                var selectedFacilitiesPayload = selectedFacilityPayload.First();


                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                searchPredicate = searchPredicate.And(f => allowedHealthFacilityIDs.Contains(f.HealthFacilityID));

                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                        
                    }
                }

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerPharmacy> monthlyTotals = new List<PrescriptionsPerPharmacy>();
                var g = from t in db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                h.Month >= fromDate.Month
                && h.Month <= toDate.Month
                && h.Year >= fromDate.Year
                && h.Year <= toDate.Year
                && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true))
                        group t by new { t.PharmacyID } into x
                        select new
                        {
                            x.Key.PharmacyID,
                            TotalPrescriptions = x.Sum(t => t.TotalPrescriptions),
                            TotalQuantity = x.Sum(t=>t.TotalQuantity),
                            TotalRefills = x.Sum(t=>t.TotalRefillQuantity),
                        };
                foreach (var total in g.OrderByDescending(d => new { d.TotalPrescriptions }))
                {
                    
                    PrescriptionsPerPharmacy p = new PrescriptionsPerPharmacy
                    {
                        PharmacyName = pharmDb.Pharmacies.Where(d => d.PharmacyID == total.PharmacyID).First().PharmacyName,
                        TotalPrescriptions = total.TotalPrescriptions,
                        TotalQuantity = total.TotalQuantity,
                        TotalRefillQuantity = total.TotalRefills,
                    };
                    monthlyTotals.Add(p);
                }

                return Ok(monthlyTotals);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IHttpActionResult GetPrescriptionsPerFacility([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            HDACoreContext providerQuery = new HDACoreContext();
            if (ModelState.IsValid)
            {
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                searchPredicate = searchPredicate.And(f => allowedHealthFacilityIDs.Contains(f.HealthFacilityID));

                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);

                    }
                }
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();
                var g = from t in db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                h.Month >= fromDate.Month
                && h.Month <= toDate.Month
                && h.Year >= fromDate.Year
                && h.Year <= toDate.Year
                && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true))
                        group t by new {t.HealthFacilityID, t.HealthFacility.HealthFacilityNameEn } into x
                        select new
                        {
                            x.Key.HealthFacilityID,
                            HealthFacilityName = x.Key.HealthFacilityNameEn,
                            Total = x.Sum(t => t.TotalPrescriptions),
                        };
                var result = g.OrderByDescending(d => new { d.Total });
                foreach (var total in result)
                {

                    PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                    {
                        HealthFacilityName = total.HealthFacilityName,
                        Total = total.Total,
                    };
                    monthlyTotals.Add(p);
                }

                return Ok(monthlyTotals);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
        [HttpPost]
        public IHttpActionResult GetSummaryCounts([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var selectedFacilitiesPayload = selectedFacilityPayload.First();

                    DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                    DateTime toDate = Convert.ToDateTime(payload.ToDate);


                    var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                    var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                    searchPredicate = searchPredicate.And(f => allowedHealthFacilityIDs.Contains(f.HealthFacilityID));

                    foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                    {
                        searchPredicate =
                          searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    }

                    var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                    basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                    var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                    if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                    {
                        foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                        {
                            selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);

                        }
                    }

                    List<SummaryCounts> summary = new List<SummaryCounts>();
                    int NumberOfHealthFacilities = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.HealthFacilityID).Distinct().Count();

                    int NumberOfPharmacies = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.PharmacyID).Distinct().Count();

                    int NumberOfProviders = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.ProviderID).Distinct().Count();

                    int NumberOfDrugClasses = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.DrugClassID).Distinct().Count();

                    int NumberOfDrugs = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.DrugId).Distinct().Count();

                    double AverageQuantityPerPrescription = 0;

                    var quantitiesPerPrescription = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && h.TotalQuantity > 0
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => (v.TotalQuantity + v.TotalRefillQuantity) / v.TotalPrescriptions);
                    if (quantitiesPerPrescription.Count() > 0)
                    {
                        AverageQuantityPerPrescription = quantitiesPerPrescription.Average();
                    }

                    int? NormalAmountToOrder = 0;
                    if (payload.DrugId > 0) {
                        NormalAmountToOrder = db.Drugs.Where(d => d.DrugID == payload.DrugId).First().NormalAmountToOrder;
                    }

                    SummaryCounts p = new SummaryCounts
                    {
                        TotalFacilities = NumberOfHealthFacilities,
                        TotalPharmacies = NumberOfPharmacies,
                        TotalProviders = NumberOfProviders,
                        TotalDrugClasses = NumberOfDrugClasses,
                        TotalDrugs = NumberOfDrugs,
                        AverageQuantityPerPrescription = (int)Math.Ceiling(AverageQuantityPerPrescription),
                        NormalAmountToOrder = NormalAmountToOrder == null ? 0 : NormalAmountToOrder,
                    };
                    summary.Add(p);
                    return Ok(summary);
                }
                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }
    }
}
