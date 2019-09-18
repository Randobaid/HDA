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
    public class TargetsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();

        [Route("api/targets")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] TargetViewModel query)
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            foreach (var target in db.Targets)
            {
                TargetViewModel t = new TargetViewModel {
                    TargetID = target.TargetID,
                    IndicatorID = target.IndicatorID,
                    HealthFacilityID = target.HealthFacilityID,
                    ProviderID = target.ProviderID,
                    DomainLookupID = target.DomainLookupID,
                    DirectorateLookupID = target.DirectorateLookupID,
                    EffectiveDate = target.EffectiveDate,
                    Value = target.Value,
                };
                if (!(query is null) && query.TargetID > 0 && t.TargetID != query.TargetID) { continue; }
                if (!(query is null) && query.IndicatorID > 0 && t.IndicatorID != query.IndicatorID) { continue; }
                if (!(query is null) && query.HealthFacilityID > 0 && t.HealthFacilityID != query.HealthFacilityID) { continue; }
                if (!(query is null) && query.ProviderID > 0 && t.ProviderID != query.ProviderID) { continue; }
                if (!(query is null) && query.DomainLookupID > 0 && t.DomainLookupID != query.DomainLookupID) { continue; }
                if (!(query is null) && query.DirectorateLookupID > 0 && t.DirectorateLookupID != query.DirectorateLookupID) { continue; }
                targets.Add(t);
            }
            return Ok(targets);
        }

        [Route("api/targets/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            Target target = db.Targets.Where(i => i.TargetID == id).FirstOrDefault();
            TargetViewModel targetVM = new TargetViewModel {
                TargetID = target.TargetID,
                IndicatorID = target.IndicatorID,
                HealthFacilityID = target.HealthFacilityID,
                ProviderID = target.ProviderID,
                DomainLookupID = target.DomainLookupID,
                DirectorateLookupID = target.DirectorateLookupID,
                EffectiveDate = target.EffectiveDate,
                Value = target.Value,
            };
            return Ok(targetVM);
        }

        [Route("api/targets")]
        [HttpPost]
        public IHttpActionResult Create(Target target)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Targets.Add(target);
                    db.SaveChanges();
                    return Ok(target);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/targets/{id}")]
        [HttpPost]
        public IHttpActionResult Edit(int id, Target target)
        {
            return Ok();
        }

        [Route("api/targets/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(int id, Target target)
        {
            return Ok();
        }
    }
}
