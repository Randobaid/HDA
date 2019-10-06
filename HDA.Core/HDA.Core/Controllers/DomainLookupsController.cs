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
    public class DomainLookupsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();

        [Route("api/domain-lookups")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] DomainLookupViewModel query)
        {
            List<DomainLookupViewModel> domainLookups = new List<DomainLookupViewModel>();
            foreach (var domainLookup in db.DomainLookups)
            {
                DomainLookupViewModel i = new DomainLookupViewModel {
                    DomainLookupID = domainLookup.DomainLookupID,
                    DomainCode = domainLookup.DomainCode,
                    DomainNameEn = domainLookup.DomainNameEn,
                    DomainNameAr = domainLookup.DomainNameAr,
                };
                if (!(query is null) && query.DomainLookupID > 0 && i.DomainLookupID != query.DomainLookupID) { continue; }
                if (!(query is null) && !(query.DomainCode is null) && i.DomainCode != query.DomainCode) { continue; }
                if (!(query is null) && !(query.DomainNameEn is null) && i.DomainNameEn != query.DomainNameEn) { continue; }
                if (!(query is null) && !(query.DomainNameAr is null) && i.DomainNameAr != query.DomainNameAr) { continue; }
                domainLookups.Add(i);
            }
            return Ok(domainLookups);
        }

        [Route("api/domain-lookups/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            DomainLookup domainLookup = db.DomainLookups.Where(i => i.DomainLookupID == id).FirstOrDefault();
            if(domainLookup == null)
            {
                return NotFound();
            }
            DomainLookupViewModel domainLookupVM = new DomainLookupViewModel {
                DomainLookupID = domainLookup.DomainLookupID,
                DomainCode = domainLookup.DomainCode,
                DomainNameEn = domainLookup.DomainNameEn,
                DomainNameAr = domainLookup.DomainNameAr,
            };
            return Ok(domainLookupVM);
        }

        [Route("api/domain-lookups")]
        [HttpPost]
        public IHttpActionResult Create(DomainLookup domainLookup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.DomainLookups.Add(domainLookup);
                    db.SaveChanges();
                    return Ok(domainLookup);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/domain-lookups/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(int id, DomainLookupViewModel domainLookupViewModel)
        {
            DomainLookup domainLookup = db.DomainLookups.Where(i => i.DomainLookupID == id).FirstOrDefault();
            if(domainLookup == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    domainLookup.DomainCode = domainLookupViewModel.DomainCode;
                    domainLookup.DomainNameEn = domainLookupViewModel.DomainNameEn;
                    domainLookup.DomainNameAr = domainLookupViewModel.DomainNameAr;
                    db.SaveChanges();
                    return this.Details(domainLookup.DomainLookupID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/domain-lookups/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            DomainLookup domainLookup = db.DomainLookups.Where(i => i.DomainLookupID == id).FirstOrDefault();
            if(domainLookup == null)
            {
                return NotFound();
            }
            try
            {
                db.DomainLookups.Remove(domainLookup);
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
