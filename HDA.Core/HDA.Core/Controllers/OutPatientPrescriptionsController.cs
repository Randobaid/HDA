using HDA.Core.Utilities;
using HDA.Core.Models.HDAReports;
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
        private HDAReportsContext db = new HDAReportsContext();
        [HttpPost]
        public IHttpActionResult GetPrescriptionsPerInstitution([FromUri] OPRequest payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<PrescriptionTotal>();
                foreach(string healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                
                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    searchPredicate = searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (string id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                    }
                }

                HDAReportsContext hfDb = new HDAReportsContext();
                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();

                if (selectedFacilitiesPayload.HealthFacilities.Count == 0 || selectedFacilitiesPayload.HealthFacilities.Count > 5)
                {
                    var g = from t in db.PrescriptionTotals.
                            Where(searchPredicate).
                            Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                            Where(allowedHealthFacilityIDsSP).
                            Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && ((payload.PharmacyID.Length > 0) ? h.PharmacyID == payload.PharmacyID : true)
                                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                                && ((payload.DrugId.Length > 0) ? h.DrugId == payload.DrugId : true)
                            )
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
                            Where(allowedHealthFacilityIDsSP).
                            Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && ((payload.PharmacyID.Length > 0) ? h.PharmacyID == payload.PharmacyID : true)
                                && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                                && ((payload.DrugId.Length > 0) ? h.DrugId == payload.DrugId : true)
                            )
                            group t by new {t.HealthFacilityID, t.Year, t.Month } into x
                            select new
                            {
                                x.Key.HealthFacilityID,
                                x.Key.Year,
                                MonthId = x.Key.Month,
                                Total = x.Sum(t => (int?)t.TotalPrescriptions) ?? 0
                            };
                    foreach (var total in g.OrderBy(d => new { d.Year, d.MonthId }))
                    {
                        PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                        {
                            HealthFacilityName = hfDb.HealthFacilities.Where(a => a.HealthFacilityID == total.HealthFacilityID).First().HealthFacilityNameEn,
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
            HDAReportsContext pharmDb = new HDAReportsContext();
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<PrescriptionTotal>();
                foreach (string healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    searchPredicate = searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (string id in selectedFacilitiesPayload.HealthFacilities)
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
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            && ((payload.PharmacyID.Length > 0) ? h.PharmacyID == payload.PharmacyID : true)
                            && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                            && ((payload.DrugId.Length > 0) ? h.DrugId == payload.DrugId : true)
                        )
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
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<PrescriptionTotal>();
                foreach (string healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    searchPredicate = searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (string id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                    }
                }
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                HDAReportsContext hfDb = new HDAReportsContext();
                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();
                var g = from t in db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            && ((payload.PharmacyID.Length > 0) ? h.PharmacyID == payload.PharmacyID : true)
                            && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                            && ((payload.DrugId.Length > 0) ? h.DrugId == payload.DrugId : true)
                        )
                        group t by new {t.HealthFacilityID } into x
                        select new
                        {
                            x.Key.HealthFacilityID,
                            Total = x.Sum(t => t.TotalPrescriptions),
                        };
                var result = g.OrderByDescending(d => new { d.Total });
                foreach (var total in result)
                {

                    PrescriptionsPerInstitutionTotal p = new PrescriptionsPerInstitutionTotal
                    {
                        HealthFacilityName = hfDb.HealthFacilities.Where(a => a.HealthFacilityID == total.HealthFacilityID).First().HealthFacilityNameEn,
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
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                try
                {
                    var allowedHealthFacilityIDsSP = PredicateBuilder.New<PrescriptionTotal>();
                    foreach (string healthFacilityId in allowedHealthFacilityIDs)
                    {
                        allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                    }

                    var selectedFacilitiesPayload = selectedFacilityPayload.First();

                    DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                    DateTime toDate = Convert.ToDateTime(payload.ToDate);

                    var searchPredicate = PredicateBuilder.New<PrescriptionTotal>();
                    foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                    {
                        searchPredicate = searchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                    }

                    var basePrescriptionsTotalSP = PredicateBuilder.New<PrescriptionTotal>();
                    basePrescriptionsTotalSP = basePrescriptionsTotalSP.And(a => a.PrescriptionTotalID > 0);

                    var selectedHealthFacilitiesSP = PredicateBuilder.New<PrescriptionTotal>();
                    if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                    {
                        foreach (string id in selectedFacilitiesPayload.HealthFacilities)
                        {
                            selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                        }
                    }

                    List<SummaryCounts> summary = new List<SummaryCounts>();
                    double AverageQuantityPerPrescription = 0;
                    int TotalQuantityPerDrug = 0;
                    int NormalAmountToOrder = 0;
                    

                    List<PrescriptionTotal> totals = db.PrescriptionTotals.
                        Where(searchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : basePrescriptionsTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                            h.Month >= fromDate.Month
                            && h.Month <= toDate.Month
                            && h.Year >= fromDate.Year
                            && h.Year <= toDate.Year
                            && h.TotalQuantity > 0
                            && ((payload.PharmacyID.Length > 0) ? h.PharmacyID == payload.PharmacyID : true)
                            && ((payload.DrugClassId > 0) ? h.DrugClassID == payload.DrugClassId : true)
                            && ((payload.DrugId.Length > 0) ? h.DrugId == payload.DrugId : true)
                        ).ToList();

                    if (payload.DrugId.Length > 0)
                    {
                        NormalAmountToOrder = db.Drugs.Where(d => d.DrugID == payload.DrugId).First().NormalAmountToOrder;
                        TotalQuantityPerDrug = totals.Select(v => v.TotalQuantity).Sum() + totals.Select(v => v.TotalRefillQuantity).Sum();
                        int sumPrescriptions = totals.Select(v => v.TotalPrescriptions).Sum();
                        if(sumPrescriptions > 0)
                        {
                            AverageQuantityPerPrescription = TotalQuantityPerDrug / sumPrescriptions;
                        }
                        
                    }

                    SummaryCounts p = new SummaryCounts
                    {
                        TotalFacilities = totals.Select(v => v.HealthFacilityID).Distinct().Count(),
                        TotalPharmacies = totals.Select(v => v.PharmacyID).Distinct().Count(),
                        TotalProviders = totals.Select(v => v.ProviderID).Distinct().Count(),
                        TotalDrugClasses = totals.Select(v => v.DrugClassID).Distinct().Count(),
                        TotalDrugs = totals.Select(v => v.DrugId).Distinct().Count(),
                        TotalQuantityPerDrug = TotalQuantityPerDrug,
                        AverageQuantityPerPrescription = (int)Math.Ceiling(AverageQuantityPerPrescription),
                        NormalAmountToOrder =  NormalAmountToOrder,
                    };
                    summary.Add(p);
                    return Ok(summary);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IHttpActionResult PostHealthFacilitiesByType([FromBody] List<SelectedFacilityType> payload)
        {

            List<HealthFacilityVM> healthFacilities = new List<HealthFacilityVM>();

            var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
            var allowedHealthFacilityIDsSP = PredicateBuilder.New<HealthFacility>();
            foreach (string healthFacilityId in allowedHealthFacilityIDs)
            {
                allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
            }

            var searchPredicate = PredicateBuilder.New<HealthFacility>();
            foreach (SelectedFacilityType str in payload)
            {
                searchPredicate =
                  searchPredicate.Or(a => a.HealthFacilityTypeID == str.HealthFacilityTypeId);
            }


            var hfs = db.HealthFacilities.
                Where(searchPredicate).
                Where(allowedHealthFacilityIDsSP);
            foreach (var hf in hfs)
            {
                HealthFacilityVM h = new HealthFacilityVM();
                h.ID = hf.HealthFacilityID;
                h.HealthFacilityName = hf.HealthFacilityNameEn;
                healthFacilities.Add(h);
            }
            return Ok(healthFacilities);
        }

        [HttpPost]
        public IHttpActionResult GetPharmacies([FromBody] List<string> payload)
        {
            List<PharmacyVM> pharmacies = new List<PharmacyVM>();

            var selectedHealthFacilitiesSP = PredicateBuilder.New<Pharmacy>();
            foreach (string id in payload)
            {
                selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
            }

            var hfs = db.Pharmacies.Where(selectedHealthFacilitiesSP);
            foreach (var hf in hfs)
            {
                PharmacyVM h = new PharmacyVM
                {
                    ID = hf.PharmacyID,
                    PharmacyName = hf.PharmacyName,
                    HealthFacilityID = hf.HealthFacilityID
                };
                pharmacies.Add(h);
            }
            return Ok(pharmacies);
        }

        public IHttpActionResult GetDrugClasses()
        {
            List<DrugClassVM> drugClasses = new List<DrugClassVM>();
            var hfs = db.DrugClasses;
            foreach (var hf in hfs)
            {
                DrugClassVM h = new DrugClassVM
                {
                    ID = hf.DrugClassID,
                    DrugClassName = hf.DrugClassNameEn,
                };
                drugClasses.Add(h);
            }
            return Ok(drugClasses);
        }

        public IHttpActionResult GetDrugs(int id)
        {
            List<DrugVM> drugs = new List<DrugVM>();
            var hfs = db.Drugs.Where(d => d.DrugClassID == id);
            foreach (var hf in hfs)
            {
                DrugVM h = new DrugVM
                {
                    ID = hf.DrugID,
                    DrugName = hf.DrugGenericName,
                };
                drugs.Add(h);
            }
            return Ok(drugs);
        }



    }
}
