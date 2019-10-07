using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDACore;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class DirectorateLookupsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();

        [Route("api/directorate-lookups")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] DirectorateLookupViewModel query)
        {
            List<DirectorateLookupViewModel> directorateLookups = new List<DirectorateLookupViewModel>();
            foreach (var directorateLookup in db.DirectorateLookups)
            {
                DirectorateLookupViewModel i = new DirectorateLookupViewModel {
                    DirectorateLookupID = directorateLookup.DirectorateLookupID,
                    DirectorateNameEn = directorateLookup.DirectorateNameEn,
                    DirectorateNameAr = directorateLookup.DirectorateNameAr,
                };
                if (!(query is null) && query.DirectorateLookupID > 0 && i.DirectorateLookupID != query.DirectorateLookupID) { continue; }
                if (!(query is null) && !(query.DirectorateNameEn is null) && i.DirectorateNameEn != query.DirectorateNameEn) { continue; }
                if (!(query is null) && !(query.DirectorateNameAr is null) && i.DirectorateNameAr != query.DirectorateNameAr) { continue; }
                directorateLookups.Add(i);
            }
            return Ok(directorateLookups);
        }

        [Route("api/directorate-lookups/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            DirectorateLookup directorateLookup = db.DirectorateLookups.Where(i => i.DirectorateLookupID == id).FirstOrDefault();
            if(directorateLookup == null)
            {
                return NotFound();
            }
            DirectorateLookupViewModel directorateLookupVM = new DirectorateLookupViewModel {
                DirectorateLookupID = directorateLookup.DirectorateLookupID,
                DirectorateNameEn = directorateLookup.DirectorateNameEn,
                DirectorateNameAr = directorateLookup.DirectorateNameAr,
            };
            return Ok(directorateLookupVM);
        }

        [Route("api/directorate-lookups")]
        [HttpPost]
        public IHttpActionResult Create(DirectorateLookup directorateLookup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DirectorateLookups.Add(directorateLookup);
                    db.SaveChanges();
                    return Ok(directorateLookup);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/directorate-lookups/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(int id, DirectorateLookupViewModel directorateLookupViewModel)
        {
            DirectorateLookup directorateLookup = db.DirectorateLookups.Where(i => i.DirectorateLookupID == id).FirstOrDefault();
            if(directorateLookup == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    directorateLookup.DirectorateNameEn = directorateLookupViewModel.DirectorateNameEn;
                    directorateLookup.DirectorateNameAr = directorateLookupViewModel.DirectorateNameAr;
                    db.SaveChanges();
                    return this.Details(directorateLookup.DirectorateLookupID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/directorate-lookups/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            DirectorateLookup directorateLookup = db.DirectorateLookups.Where(i => i.DirectorateLookupID == id).FirstOrDefault();
            if(directorateLookup == null)
            {
                return NotFound();
            }
            try
            {
                db.DirectorateLookups.Remove(directorateLookup);
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
