using HDA.Core.Models.HDAReports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using HDA.Core.ViewModels;
using LinqKit;
using HDA.Core.Utilities;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class DiseaseManagementController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();
        [HttpPost]
        public IHttpActionResult GetDiagnosisCodes([FromBody] SelectedStartCode selectedStartCode)
        {
            if (selectedStartCode == null)
            {
                return BadRequest(new ArgumentNullException(nameof(selectedStartCode)).Message);
            }

            List<DiagnosisCodeVM> diagnosisCodes = new List<DiagnosisCodeVM>();
            var codes = db.DiagnosisCodes.Where(d => (selectedStartCode.DiagnosisCodeID > 0) ? d.Code.StartsWith(selectedStartCode.DiagnosisCode.Substring(0, 3)) : true);
            foreach(var code in codes)
            {
                DiagnosisCodeVM d = new DiagnosisCodeVM
                {
                    DiagnosisCodeID = code.DiagnosisCodeID,
                    Code = code.Code,
                    CodeName = code.DiagnosisNameEn,
                };
                diagnosisCodes.Add(d);
            }

            return Ok(diagnosisCodes);
        }

        [HttpPost]
        public IHttpActionResult GetNewCasesByAgeGroup([FromUri] DiseaseManagementRequestPayload payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {

                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<DiagnosisTotal>();
                foreach (int healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                var selectedFacilitiesPayload = selectedFacilityPayload.First();
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                var facilityTypeSearchPredicate = PredicateBuilder.New<DiagnosisTotal>();
                

                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    facilityTypeSearchPredicate =
                      facilityTypeSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var baseDiagnosisTotalSP = PredicateBuilder.New<DiagnosisTotal>();
                baseDiagnosisTotalSP = baseDiagnosisTotalSP.And(d => d.DiagnosisTotalID > 0);
                var selectedHealthFacilitiesSP = PredicateBuilder.New<DiagnosisTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);

                    }
                }


                List<NewCasesByAgeGroup> newCases = new List<NewCasesByAgeGroup>();
                var n = from t in db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseDiagnosisTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && h.DiagnosisCodeID >= payload.StartCodeID
                                && h.DiagnosisCodeID <= payload.EndCodeID)
                        group t by new {t.Year, t.Month, t.AgeGroup } into x
                        select new
                        {
                            x.Key.Year,
                            x.Key.Month,
                            x.Key.AgeGroup,
                            Total = x.Sum(t => (int?)t.Total) ?? 0,
                        };
                foreach(var total in n.OrderBy(d => new { d.Year, d.Month}))
                {
                    
                    NewCasesByAgeGroup p = new NewCasesByAgeGroup
                    {
                        Year = total.Year,
                        Month = total.Month,
                        AgeGroup = total.AgeGroup,
                        Total = total.Total
                    };
                    newCases.Add(p);
                }
                


                return Ok(newCases);
            }
            return BadRequest(ModelState);

        }

        [HttpPost]
        public IHttpActionResult GetNewCasesByGender([FromUri] DiseaseManagementRequestPayload payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<DiagnosisTotal>();
                foreach (int healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                var selectedFacilitiesPayload = selectedFacilityPayload.First();
                var facilityTypeSearchPredicate = PredicateBuilder.New<DiagnosisTotal>();


                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    facilityTypeSearchPredicate =
                      facilityTypeSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var baseDiagnosisTotalSP = PredicateBuilder.New<DiagnosisTotal>();
                baseDiagnosisTotalSP = baseDiagnosisTotalSP.And(d => d.DiagnosisTotalID > 0);
                var selectedHealthFacilitiesSP = PredicateBuilder.New<DiagnosisTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);

                    }
                }
                List<NewCasesByGender> newCases = new List<NewCasesByGender>();
                var n = from t in db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseDiagnosisTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && h.DiagnosisCodeID >= payload.StartCodeID
                                && h.DiagnosisCodeID <= payload.EndCodeID)
                        group t by new { t.Year, t.Month } into x
                        select new
                        {
                            x.Key.Year,
                            x.Key.Month,
                            FemaleTotal = x.Where(w => w.Gender.GenderEn.ToLower() == "female").Sum(t => (int?)t.Total) ?? 0,
                            MaleTotal = x.Where(w => w.Gender.GenderEn.ToLower() == "male").Sum(t => (int?)t.Total) ?? 0,
                        };
                foreach (var total in n.OrderBy(d => new { d.Year, d.Month }))
                {

                    NewCasesByGender p = new NewCasesByGender
                    {
                        Year = total.Year,
                        Month = total.Month,
                        FemaleTotal = total.FemaleTotal,
                        MaleTotal = total.MaleTotal,
                    };
                    newCases.Add(p);
                }
                return Ok(newCases);
            }
            return BadRequest(ModelState);

        }

        [HttpPost]
        public IHttpActionResult GetTotalsByAgeGroup([FromUri] DiseaseManagementRequestPayload payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<DiagnosisTotal>();
                foreach (int healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                var selectedFacilitiesPayload = selectedFacilityPayload.First();
                var facilityTypeSearchPredicate = PredicateBuilder.New<DiagnosisTotal>();
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    facilityTypeSearchPredicate =
                      facilityTypeSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var baseDiagnosisTotalSP = PredicateBuilder.New<DiagnosisTotal>();
                baseDiagnosisTotalSP = baseDiagnosisTotalSP.And(d => d.DiagnosisTotalID > 0);
                var selectedHealthFacilitiesSP = PredicateBuilder.New<DiagnosisTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                    }
                }

                List<TotalsByAgeGroup> totalCases = new List<TotalsByAgeGroup>();
                var n = from t in db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseDiagnosisTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && h.DiagnosisCodeID >= payload.StartCodeID
                                && h.DiagnosisCodeID <= payload.EndCodeID)
                        group t by 1
                        into x
                        select new
                        {
                            Total01 = x.Where(w => w.AgeGroup.ToLower() == "0-1").Sum(t => (int?)t.Total) ?? 0,
                            Total25 = x.Where(w => w.AgeGroup.ToLower() == "2-5").Sum(t => (int?)t.Total) ?? 0,
                            Total614 = x.Where(w => w.AgeGroup.ToLower() == "6-14").Sum(t => (int?)t.Total) ?? 0,
                            Total1535 = x.Where(w => w.AgeGroup.ToLower() == "15-35").Sum(t => (int?)t.Total) ?? 0,
                            Total3660 = x.Where(w => w.AgeGroup.ToLower() == "36-60").Sum(t => (int?)t.Total) ?? 0,
                            Total60Plus = x.Where(w => w.AgeGroup.ToLower() == "60+").Sum(t => (int?)t.Total) ?? 0,
                        };
                foreach (var total in n)
                {

                    TotalsByAgeGroup p = new TotalsByAgeGroup
                    {
                        Total01 = total.Total01,
                        Total25 = total.Total25,
                        Total614 = total.Total614,
                        Total1535 = total.Total1535,
                        Total3660 = total.Total3660,
                        Total60Plus = total.Total60Plus,
                    };
                    totalCases.Add(p);
                }
                return Ok(totalCases);
            }
            return BadRequest(ModelState);

        }

        [HttpPost]
        public IHttpActionResult GetTotalsByGender([FromUri] DiseaseManagementRequestPayload payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<DiagnosisTotal>();
                foreach (int healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                var selectedFacilitiesPayload = selectedFacilityPayload.First();
                var facilityTypeSearchPredicate = PredicateBuilder.New<DiagnosisTotal>();
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    facilityTypeSearchPredicate =
                      facilityTypeSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var baseDiagnosisTotalSP = PredicateBuilder.New<DiagnosisTotal>();
                baseDiagnosisTotalSP = baseDiagnosisTotalSP.And(d => d.DiagnosisTotalID > 0);
                var selectedHealthFacilitiesSP = PredicateBuilder.New<DiagnosisTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                    }
                }


                List<TotalsByGender> totalCases = new List<TotalsByGender>();
                var n = from t in db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseDiagnosisTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && h.DiagnosisCodeID >= payload.StartCodeID
                                && h.DiagnosisCodeID <= payload.EndCodeID)
                        group t by 1
                        into x
                        select new
                        {
                            TotalFemale = x.Where(w => w.Gender.GenderEn.ToLower() == "female").Sum(t => (int?)t.Total) ?? 0,
                            TotalMale = x.Where(w => w.Gender.GenderEn.ToLower() == "male").Sum(t => (int?)t.Total) ?? 0,
                        };
                foreach (var total in n)
                {

                    TotalsByGender p = new TotalsByGender
                    {
                        FemaleTotal = total.TotalFemale,
                        MaleTotal = total.TotalMale,
                    };
                    totalCases.Add(p);
                }
                return Ok(totalCases);
            }
            return BadRequest(ModelState);

        }

        public class Summary
        {
            public int TotalCases { get; set; }
        }

        [HttpPost]
        public IHttpActionResult GetSummaries([FromUri] DiseaseManagementRequestPayload payload, [FromBody] List<SelectedFacilityPayload> selectedFacilityPayload)
        {
            if (ModelState.IsValid)
            {
                var allowedHealthFacilityIDs = new PermissionCheck().GetAllowedFacilityIds(User.Identity.GetUserId());
                var allowedHealthFacilityIDsSP = PredicateBuilder.New<DiagnosisTotal>();
                foreach (int healthFacilityId in allowedHealthFacilityIDs)
                {
                    allowedHealthFacilityIDsSP = allowedHealthFacilityIDsSP.Or(a => a.HealthFacilityID == healthFacilityId);
                }

                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                var selectedFacilitiesPayload = selectedFacilityPayload.First();

                var facilityTypeSearchPredicate = PredicateBuilder.New<DiagnosisTotal>();
                foreach (SelectedFacilityType s in selectedFacilitiesPayload.HealthFacilityTypes)
                {
                    facilityTypeSearchPredicate =
                      facilityTypeSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

                var baseDiagnosisTotalSP = PredicateBuilder.New<DiagnosisTotal>();
                baseDiagnosisTotalSP = baseDiagnosisTotalSP.And(d => d.DiagnosisTotalID > 0);
                var selectedHealthFacilitiesSP = PredicateBuilder.New<DiagnosisTotal>();
                if (selectedFacilitiesPayload.HealthFacilities.Count > 0)
                {
                    foreach (int id in selectedFacilitiesPayload.HealthFacilities)
                    {
                        selectedHealthFacilitiesSP = selectedHealthFacilitiesSP.Or(a => a.HealthFacilityID == id);
                    }
                }

                int NumberOfMonths = db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseDiagnosisTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                    h.Month >= fromDate.Month
                    && h.Month <= toDate.Month
                    && h.Year >= fromDate.Year
                    && h.Year <= toDate.Year)
                   .Select(v => new { v.Year, v.Month }).Distinct().Count();

                List<Summary> summaries = new List<Summary>();
                var n = from t in db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where((selectedHealthFacilitiesSP.IsStarted) ? selectedHealthFacilitiesSP : baseDiagnosisTotalSP).
                        Where(allowedHealthFacilityIDsSP).
                        Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                                && h.DiagnosisCodeID >= payload.StartCodeID
                                && h.DiagnosisCodeID <= payload.EndCodeID)
                        group t by 1
                        into x
                        select new
                        {
                            AllCases = x.Where(w => 1==1).Sum(t => (int?)t.Total) ?? 0
                        };
                foreach (var total in n)
                {
                    Summary s = new Summary
                    {
                        TotalCases = total.AllCases
                    };
                    summaries.Add(s);
                    //AverageNewCasesPerMonth = total.AllCases/ NumberOfMonths;
                    //TotalNewCases = total.AllCases;
                }
                List<DiagnosisSummary> diagnosisSummaries = new List<DiagnosisSummary>();

                DiagnosisSummary diagnosisSummary = new DiagnosisSummary
                {
                    AverageNewCasesPerMonth = summaries.First().TotalCases/NumberOfMonths,
                    TotalNewCases = summaries.First().TotalCases
                };
                diagnosisSummaries.Add(diagnosisSummary);

                return Ok(diagnosisSummaries);
            }
            return BadRequest(ModelState);

        }

    }
}
