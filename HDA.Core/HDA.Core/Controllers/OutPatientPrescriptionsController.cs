using HDA.Core.Models.HDACore;
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
    public class OutPatientPrescriptionsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        [HttpPost]
        public IHttpActionResult GetPrescriptionsPerInstitution([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();

                foreach (SelectedFacilityType s in selectedFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }
                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();
                var g = from t in db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                            && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                            && ((payload.DrugClassId > 0)? h.DrugClassID == payload.DrugClassId : true)
                            && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true))
                        group t by new { t.Month, t.Year} into x
                        select new
                        {
                            x.Key.Year,
                            MonthId = x.Key.Month,
                            Total = x.Sum(t => t.TotalPrescriptions)
                        };
                foreach(var total in g.OrderBy(d => new { d.Year, d.MonthId }))
                {
                    DateTimeFormatInfo d = new DateTimeFormatInfo();
                    PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                    {
                        Year = total.Year,
                        MonthId = total.MonthId,
                        MonthName = d.GetMonthName(total.MonthId + 1),
                        DrugClass = 0,
                        DrugId = 0,
                        Total = total.Total
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
        public IHttpActionResult GetPrescriptionsPerPharmacy([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            HDACoreContext pharmDb = new HDACoreContext();
            if (ModelState.IsValid)
            {
                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();

                foreach (SelectedFacilityType s in selectedFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerPharmacy> monthlyTotals = new List<PrescriptionsPerPharmacy>();
                var g = from t in db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where(h =>
                h.Month >= fromDate.Month
                && h.Month <= toDate.Month
                && h.Year >= fromDate.Year
                && h.Year <= toDate.Year
                && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
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
        public IHttpActionResult GetPrescriptionsPerProvider([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            HDACoreContext providerQuery = new HDACoreContext();
            if (ModelState.IsValid)
            {
                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();

                foreach (SelectedFacilityType s in selectedFacilityTypes)
                {
                    searchPredicate =
                      searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerProvider> monthlyTotals = new List<PrescriptionsPerProvider>();
                var g = from t in db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where(h =>
                h.Month >= fromDate.Month
                && h.Month <= toDate.Month
                && h.Year >= fromDate.Year
                && h.Year <= toDate.Year
                && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true))
                        group t by new { t.ProviderID } into x
                        select new
                        {
                            x.Key.ProviderID,
                            TotalPrescriptions = x.Sum(t => t.TotalPrescriptions),
                        };
                foreach (var total in g.OrderByDescending(d => new { d.TotalPrescriptions }))
                {

                    PrescriptionsPerProvider p = new PrescriptionsPerProvider
                    {
                        ProviderName = providerQuery.Providers.Where(d => d.ProviderID == total.ProviderID).First().ProviderNameEn,
                        TotalPrescriptions = total.TotalPrescriptions,
                        
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
        public IHttpActionResult GetSummaryCounts([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                    DateTime toDate = Convert.ToDateTime(payload.ToDate);


                    var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();

                    foreach (SelectedFacilityType s in selectedFacilityTypes)
                    {
                        searchPredicate =
                          searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    }

                    List<SummaryCounts> summary = new List<SummaryCounts>();
                    int NumberOfHealthFacilities = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.HealthFacilityID).Distinct().Count();

                    int NumberOfPharmacies = db.PrescriptionTotals.
                        Where(searchPredicate)
                        .Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.PharmacyID).Distinct().Count();

                    int NumberOfProviders = db.PrescriptionTotals.
                        Where(searchPredicate)
                        .Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.ProviderID).Distinct().Count();

                    int NumberOfDrugClasses = db.PrescriptionTotals.
                        Where(searchPredicate)
                        .Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.DrugClassID).Distinct().Count();

                    int NumberOfDrugs = db.PrescriptionTotals.
                        Where(searchPredicate)
                        .Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                    && ((payload.PharmacyID > 0) ? h.PharmacyID == payload.PharmacyID : true)
                    && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                    && ((payload.DrugId > 0) ? h.DrugId == payload.DrugId : true)).
                    Select(v => v.DrugId).Distinct().Count();

                    double AverageQuantityPerPrescription = 0;

                    var quantitiesPerPrescription = db.PrescriptionTotals.
                        Where(searchPredicate)
                        .Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year
                    && h.TotalQuantity > 0
                    && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
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
