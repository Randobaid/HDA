using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDAReports;
using HDA.Core.Utilities;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class HealthFacilityTypeController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();

        [Route("api/HealthFacilityTypes")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] HealthFacilityTypeViewModel query)
        {
            List<HealthFacilityTypeViewModel> healthFacilityTypes = new List<HealthFacilityTypeViewModel>();
            foreach (var healthFacilityType in db.HealthFacilityTypes)
            {
                HealthFacilityTypeViewModel i = new HealthFacilityTypeViewModel
                {
                    HealthFacilityTypeID = healthFacilityType.HealthFacilityTypeID,
                    HealthFacilityTypeCode = healthFacilityType.HealthFacilityTypeCode,
                    HealthFacilityTypeNameEn = healthFacilityType.HealthFacilityTypeNameEn,
                    HealthFacilityTypeNameAr = healthFacilityType.HealthFacilityTypeNameAr,
                };
                if (!(query is null) && query.HealthFacilityTypeID > 0 && i.HealthFacilityTypeID != query.HealthFacilityTypeID) { continue; }
                if (!(query is null) && !(query.HealthFacilityTypeCode is null) && i.HealthFacilityTypeCode != query.HealthFacilityTypeCode) { continue; }
                if (!(query is null) && !(query.HealthFacilityTypeNameEn is null) && i.HealthFacilityTypeNameEn != query.HealthFacilityTypeNameEn) { continue; }
                if (!(query is null) && !(query.HealthFacilityTypeNameAr is null) && i.HealthFacilityTypeNameAr != query.HealthFacilityTypeNameAr) { continue; }
                healthFacilityTypes.Add(i);
            }
            return Ok(healthFacilityTypes);
        }

        [Route("api/HealthFacilityTypes/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            HealthFacilityType healthFacilityType = db.HealthFacilityTypes.Where(i => i.HealthFacilityTypeID == id).FirstOrDefault();
            if (healthFacilityType == null)
            {
                return NotFound();
            }
            HealthFacilityTypeViewModel healthFacilityTypeVM = new HealthFacilityTypeViewModel
            {
                HealthFacilityTypeID = healthFacilityType.HealthFacilityTypeID,
                HealthFacilityTypeCode = healthFacilityType.HealthFacilityTypeCode,
                HealthFacilityTypeNameEn = healthFacilityType.HealthFacilityTypeNameEn,
                HealthFacilityTypeNameAr = healthFacilityType.HealthFacilityTypeNameAr,
            };
            return Ok(healthFacilityTypeVM);
        }

        [Route("api/HealthFacilityTypes")]
        [HttpPost]
        public IHttpActionResult Create(HealthFacilityType healthFacilityType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.HealthFacilityTypes.Add(healthFacilityType);
                    db.SaveChanges();
                    return Ok(healthFacilityType);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/HealthFacilityTypes/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(int id, HealthFacilityTypeViewModel healthFacilityTypeViewModel)
        {
            HealthFacilityType healthFacilityType = db.HealthFacilityTypes.Where(i => i.HealthFacilityTypeID == id).FirstOrDefault();
            if (healthFacilityType == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    healthFacilityType.HealthFacilityTypeNameEn = healthFacilityTypeViewModel.HealthFacilityTypeNameEn;
                    healthFacilityType.HealthFacilityTypeNameAr = healthFacilityTypeViewModel.HealthFacilityTypeNameAr;
                    db.SaveChanges();
                    return this.Details(healthFacilityType.HealthFacilityTypeID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/HealthFacilityTypes/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            HealthFacilityType healthFacilityType = db.HealthFacilityTypes.Where(i => i.HealthFacilityTypeID == id).FirstOrDefault();
            if (healthFacilityType == null)
            {
                return NotFound();
            }
            try
            {
                db.HealthFacilityTypes.Remove(healthFacilityType);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("api/HealthFacilityTypes/GetAllowedHealthFacilityTypes")]
        [HttpGet]
        public IHttpActionResult GetAllowedHealthFacilityTypes()
        {
            try
            {
                List<HealthFacilityTypeViewModel> allowedHealthFacilityTypes = new List<HealthFacilityTypeViewModel>();
                var allowedHealthFacilityTypeIDs = new PermissionCheck().GetAllowedHealthFacilityTypeIds(User.Identity.GetUserId());
                if(allowedHealthFacilityTypeIDs.Count == 0)
                {
                    List<HealthFacilityTypeViewModel> allhealthFacilityTypes = new List<HealthFacilityTypeViewModel>();
                    foreach (var healthFacilityType in db.HealthFacilityTypes)
                    {
                        HealthFacilityTypeViewModel i = new HealthFacilityTypeViewModel
                        {
                            HealthFacilityTypeID = healthFacilityType.HealthFacilityTypeID,
                            HealthFacilityTypeCode = healthFacilityType.HealthFacilityTypeCode,
                            HealthFacilityTypeNameEn = healthFacilityType.HealthFacilityTypeNameEn,
                            HealthFacilityTypeNameAr = healthFacilityType.HealthFacilityTypeNameAr,
                        };
                        allhealthFacilityTypes.Add(i);
                    }
                    return Ok(allhealthFacilityTypes);
                }
                foreach (int i in allowedHealthFacilityTypeIDs)
                {
                    
                    HealthFacilityType healthFacilityType = db.HealthFacilityTypes.Where(t => t.HealthFacilityTypeID == i).FirstOrDefault();
                    HealthFacilityTypeViewModel healthFacilityTypeViewModel = new HealthFacilityTypeViewModel
                    {
                        HealthFacilityTypeID = healthFacilityType.HealthFacilityTypeID,
                        HealthFacilityTypeCode = healthFacilityType.HealthFacilityTypeCode,
                        HealthFacilityTypeNameEn = healthFacilityType.HealthFacilityTypeNameEn,
                        HealthFacilityTypeNameAr = healthFacilityType.HealthFacilityTypeNameAr
                    };
                    allowedHealthFacilityTypes.Add(healthFacilityTypeViewModel);
                }
                return Ok(allowedHealthFacilityTypes);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
