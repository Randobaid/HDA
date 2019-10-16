using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDAReports;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class GovernoratesController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();

        [Route("api/governorates")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] GovernorateViewModel query)
        {
            List<GovernorateViewModel> governorates = new List<GovernorateViewModel>();
            foreach (var governorate in db.Governorates)
            {
                GovernorateViewModel i = new GovernorateViewModel {
                    GovernorateID = governorate.GovernorateID,
                    GovernorateCode = governorate.GovernorateCode,
                    GovernorateNameAr = governorate.GovernorateNameAr,
                    GovernorateNameEn = governorate.GovernorateNameEn,
                    RegionID = governorate.RegionID,
                    ShapeFile = governorate.ShapeFile,
                };
                if (!(query is null) && query.GovernorateID > 0 && i.GovernorateID != query.GovernorateID) { continue; }
                if (!(query is null) && query.GovernorateCode > 0 && i.GovernorateCode != query.GovernorateCode) { continue; }
                if (!(query is null) && !(query.GovernorateNameAr is null) && i.GovernorateNameAr != query.GovernorateNameAr) { continue; }
                if (!(query is null) && !(query.GovernorateNameEn is null) && i.GovernorateNameEn != query.GovernorateNameEn) { continue; }
                if (!(query is null) && query.RegionID > 0 && i.RegionID != query.RegionID) { continue; }
                if (!(query is null) && !(query.ShapeFile is null) && i.ShapeFile != query.ShapeFile) { continue; }
                governorates.Add(i);
            }
            return Ok(governorates);
        }

        [Route("api/governorates/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            Governorate governorate = db.Governorates.Where(i => i.GovernorateID == id).FirstOrDefault();
            if(governorate == null)
            {
                return NotFound();
            }
            GovernorateViewModel governorateVM = new GovernorateViewModel {
                GovernorateID = governorate.GovernorateID,
                GovernorateCode = governorate.GovernorateCode,
                GovernorateNameAr = governorate.GovernorateNameAr,
                GovernorateNameEn = governorate.GovernorateNameEn,
                RegionID = governorate.RegionID,
                ShapeFile = governorate.ShapeFile,
            };
            return Ok(governorateVM);
        }

        [Route("api/governorates")]
        [HttpPost]
        public IHttpActionResult Create(Governorate governorate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Governorates.Add(governorate);
                    db.SaveChanges();
                    return Ok(governorate);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/governorates/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(int id, GovernorateViewModel governorateViewModel)
        {
            Governorate governorate = db.Governorates.Where(i => i.GovernorateID == id).FirstOrDefault();
            if(governorate == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    governorate.GovernorateCode = governorateViewModel.GovernorateCode;
                    governorate.GovernorateNameAr = governorateViewModel.GovernorateNameAr;
                    governorate.GovernorateNameEn = governorateViewModel.GovernorateNameEn;
                    governorate.RegionID = governorateViewModel.RegionID;
                    governorate.ShapeFile = governorateViewModel.ShapeFile;
                    db.SaveChanges();
                    return this.Details(governorate.GovernorateID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/governorates/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Governorate governorate = db.Governorates.Where(i => i.GovernorateID == id).FirstOrDefault();
            if(governorate == null)
            {
                return NotFound();
            }
            try
            {
                db.Governorates.Remove(governorate);
                db.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
