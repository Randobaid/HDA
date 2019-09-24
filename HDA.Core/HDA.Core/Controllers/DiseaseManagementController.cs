using HDA.Core.Models.HDACore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using LinqKit;

namespace HDA.Core.Controllers
{
    public class DiseaseManagementController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
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
        public IHttpActionResult GetNewCasesByAgeGroup([FromUri] DiseaseManagementRequestPayload payload, [FromBody] List<SelectedFacilityType> selectedFacilityTypes)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                var facilityTypeSearchPredicate = PredicateBuilder.New<DiagnosisTotal>();
                

                foreach (SelectedFacilityType s in selectedFacilityTypes)
                {
                    facilityTypeSearchPredicate =
                      facilityTypeSearchPredicate.Or(a => a.HealthFacilityTypeID == s.HealthFacilityTypeId);
                }

               

                List<NewCasesByAgeGroup> newCases = new List<NewCasesByAgeGroup>();
                var n = from t in db.DiagnosisTotals.
                        Where(facilityTypeSearchPredicate).
                        Where(h =>
                                h.Month >= fromDate.Month
                                && h.Month <= toDate.Month
                                && h.Year >= fromDate.Year
                                && h.Year <= toDate.Year
                                && ((payload.HealthFacilityID > 0) ? h.HealthFacilityID == payload.HealthFacilityID : true)
                                && h.DiagnosisCodeID >= payload.StartCodeID
                                && h.DiagnosisCodeID <= payload.EndCodeID)
                        group t by new {t.Year, t.Month, t.AgeGroup } into x
                        select new
                        {
                            x.Key.Year,
                            x.Key.Month,
                            x.Key.AgeGroup,
                            Total = x.Sum(t => t.Total),
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
    }
}
