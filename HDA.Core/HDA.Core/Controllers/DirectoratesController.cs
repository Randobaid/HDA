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
    public class DirectoratesController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();

        [Route("api/directorates")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] DirectorateViewModel query)
        {
            List<DirectorateViewModel> directorates = new List<DirectorateViewModel>();
            foreach (var directorate in db.Directorates)
            {
                DirectorateViewModel i = new DirectorateViewModel {
                    DirectorateID = directorate.DirectorateID,
                    DirectorateNameEn = directorate.DirectorateNameEn,
                    DirectorateNameAr = directorate.DirectorateNameAr,
                };
                if (!(query is null) && query.DirectorateID > 0 && i.DirectorateID != query.DirectorateID) { continue; }
                if (!(query is null) && !(query.DirectorateNameEn is null) && i.DirectorateNameEn != query.DirectorateNameEn) { continue; }
                if (!(query is null) && !(query.DirectorateNameAr is null) && i.DirectorateNameAr != query.DirectorateNameAr) { continue; }
                directorates.Add(i);
            }
            return Ok(directorates);
        }

        [Route("api/directorates/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            Directorate directorate = db.Directorates.Where(i => i.DirectorateID == id).FirstOrDefault();
            if(directorate == null)
            {
                return NotFound();
            }
            DirectorateViewModel directorateVM = new DirectorateViewModel {
                DirectorateID = directorate.DirectorateID,
                DirectorateNameEn = directorate.DirectorateNameEn,
                DirectorateNameAr = directorate.DirectorateNameAr,
            };
            return Ok(directorateVM);
        }

        [Route("api/directorates")]
        [HttpPost]
        public IHttpActionResult Create(Directorate directorate)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Directorates.Add(directorate);
                    db.SaveChanges();
                    return Ok(directorate);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/directorates/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(int id, DirectorateViewModel directorateViewModel)
        {
            Directorate directorate = db.Directorates.Where(i => i.DirectorateID == id).FirstOrDefault();
            if(directorate == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    directorate.DirectorateNameEn = directorateViewModel.DirectorateNameEn;
                    directorate.DirectorateNameAr = directorateViewModel.DirectorateNameAr;
                    db.SaveChanges();
                    return this.Details(directorate.DirectorateID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/directorates/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Directorate directorate = db.Directorates.Where(i => i.DirectorateID == id).FirstOrDefault();
            if(directorate == null)
            {
                return NotFound();
            }
            try
            {
                db.Directorates.Remove(directorate);
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
