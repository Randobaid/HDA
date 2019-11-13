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
    public class DomainsController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();

        [Route("api/domains")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] DomainViewModel query)
        {
            List<DomainViewModel> domains = new List<DomainViewModel>();
            foreach (var domain in db.Domains)
            {
                DomainViewModel i = new DomainViewModel {
                    DomainID = domain.DomainID,
                    DomainCode = domain.DomainCode,
                    DomainNameEn = domain.DomainNameEn,
                    DomainNameAr = domain.DomainNameAr,
                };
                if (!(query is null) && query.DomainID > 0 && i.DomainID != query.DomainID) { continue; }
                if (!(query is null) && !(query.DomainCode is null) && i.DomainCode != query.DomainCode) { continue; }
                if (!(query is null) && !(query.DomainNameEn is null) && i.DomainNameEn != query.DomainNameEn) { continue; }
                if (!(query is null) && !(query.DomainNameAr is null) && i.DomainNameAr != query.DomainNameAr) { continue; }
                domains.Add(i);
            }
            return Ok(domains);
        }

        [Route("api/domains/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            Domain domain = db.Domains.Where(i => i.DomainID == id).FirstOrDefault();
            if(domain == null)
            {
                return NotFound();
            }
            DomainViewModel domainVM = new DomainViewModel {
                DomainID = domain.DomainID,
                DomainCode = domain.DomainCode,
                DomainNameEn = domain.DomainNameEn,
                DomainNameAr = domain.DomainNameAr,
            };
            return Ok(domainVM);
        }

        [Route("api/domains")]
        [HttpPost]
        public IHttpActionResult Create(Domain domain)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Domains.Add(domain);
                    db.SaveChanges();
                    return Ok(domain);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/domains/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(int id, DomainViewModel domainViewModel)
        {
            Domain domain = db.Domains.Where(i => i.DomainID == id).FirstOrDefault();
            if(domain == null)
            {
                return NotFound();
            }
            try
            {
                if (ModelState.IsValid)
                {
                    domain.DomainCode = domainViewModel.DomainCode;
                    domain.DomainNameEn = domainViewModel.DomainNameEn;
                    domain.DomainNameAr = domainViewModel.DomainNameAr;
                    db.SaveChanges();
                    return this.Details(domain.DomainID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/domains/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Domain domain = db.Domains.Where(i => i.DomainID == id).FirstOrDefault();
            if(domain == null)
            {
                return NotFound();
            }
            try
            {
                db.Domains.Remove(domain);
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
