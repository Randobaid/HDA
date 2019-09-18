using HDA.Core.Models.HDACore;
using HDA.Core.ViewModels;
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
        public IHttpActionResult GetPrescriptionsPerInstitution([FromUri] OPRequest payload)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerInstitutionTotal> monthlyTotals = new List<PrescriptionsPerInstitutionTotal>();
                var g = from t in db.PrescriptionTotals.Where(h =>
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

        public IHttpActionResult GetPrescriptionsPerPharmacy([FromUri] OPRequest payload)
        {
            HDACoreContext pharmDb = new HDACoreContext();
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);

                List<PrescriptionsPerPharmacy> monthlyTotals = new List<PrescriptionsPerPharmacy>();
                var g = from t in db.PrescriptionTotals.Where(h =>
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
                            TotalQuantity = x.Sum(t=>t.TotalQuantity)
                        };
                foreach (var total in g.OrderByDescending(d => new { d.TotalPrescriptions }))
                {
                    
                    PrescriptionsPerPharmacy p = new PrescriptionsPerPharmacy
                    {
                        PharmacyName = pharmDb.Pharmacies.Where(d => d.PharmacyID == total.PharmacyID).First().PharmacyName,
                        TotalPrescriptions = total.TotalPrescriptions,
                        TotalQuantity = total.TotalQuantity
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

    }
}
