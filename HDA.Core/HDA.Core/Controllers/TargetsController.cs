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
                Indicator indicator = new HDACoreContext().Indicators.Where(i => i.IndicatorID == target.IndicatorID).FirstOrDefault();
                HealthFacility healthFacility = new HDACoreContext().HealthFacilities.Where(h => h.HealthFacilityID == target.HealthFacilityID).FirstOrDefault();
                Provider provider = new HDACoreContext().Providers.Where(i => i.ProviderID == target.ProviderID).FirstOrDefault();
                DomainLookup domainLookup = new HDACoreContext().DomainLookups.Where(i => i.DomainLookupID == target.DomainLookupID).FirstOrDefault();
                DirectorateLookup directorateLookup = new HDACoreContext().DirectorateLookups.Where(i => i.DirectorateLookupID == target.DirectorateLookupID).FirstOrDefault();
                TargetViewModel t = new TargetViewModel {
                    TargetID = target.TargetID,
                    IndicatorID = target.IndicatorID,
                    Indicator = target.IndicatorID != 0 ? new IndicatorViewModel {
                        IndicatorID = indicator.IndicatorID,
                        IndicatorShortName = indicator.IndicatorShortName,
                        IndicatorNameEn = indicator.IndicatorNameEn,
                        IndicatorNameAr = indicator.IndicatorNameAr,
                    } : new IndicatorViewModel {},
                    HealthFacilityID = target.HealthFacilityID,
                    HealthFacility = target.HealthFacilityID != null ? new HealthFacilityVM {
                        ID = healthFacility.HealthFacilityID,
                        HealthFacilityName = healthFacility.HealthFacilityNameEn,
                    } : new HealthFacilityVM {},
                    ProviderID = target.ProviderID,
                    Provider = target.ProviderID != null ? new ProviderVM {
                        ProviderID = provider.ProviderID,
                        ProviderName = provider.ProviderNameEn,
                    } : new ProviderVM {},
                    DomainLookupID = target.DomainLookupID,
                    DomainLookup = target.DomainLookupID != null ? new DomainLookupViewModel {
                        DomainLookupID = domainLookup.DomainLookupID,
                        DomainCode = domainLookup.DomainCode,
                        DomainNameEn = domainLookup.DomainNameEn,
                        DomainNameAr = domainLookup.DomainNameAr,
                    } : new DomainLookupViewModel {},
                    DirectorateLookupID = target.DirectorateLookupID,
                    DirectorateLookup = target.DirectorateLookupID != null ? new DirectorateLookupViewModel {
                        DirectorateLookupID = directorateLookup.DirectorateLookupID,
                        DirectorateNameEn = directorateLookup.DirectorateNameEn,
                        DirectorateNameAr = directorateLookup.DirectorateNameAr,
                    } : new DirectorateLookupViewModel {},
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
            if(target == null)
            {
                return NotFound();
            }
            Indicator indicator = new HDACoreContext().Indicators.Where(i => i.IndicatorID == target.IndicatorID).FirstOrDefault();
            HealthFacility healthFacility = new HDACoreContext().HealthFacilities.Where(h => h.HealthFacilityID == target.HealthFacilityID).FirstOrDefault();
            Provider provider = new HDACoreContext().Providers.Where(i => i.ProviderID == target.ProviderID).FirstOrDefault();
            DomainLookup domainLookup = new HDACoreContext().DomainLookups.Where(i => i.DomainLookupID == target.DomainLookupID).FirstOrDefault();
            DirectorateLookup directorateLookup = new HDACoreContext().DirectorateLookups.Where(i => i.DirectorateLookupID == target.DirectorateLookupID).FirstOrDefault();
            TargetViewModel targetViewModel = new TargetViewModel {
                TargetID = target.TargetID,
                IndicatorID = target.IndicatorID,
                Indicator = target.IndicatorID != 0 ? new IndicatorViewModel {
                    IndicatorID = indicator.IndicatorID,
                    IndicatorShortName = indicator.IndicatorShortName,
                    IndicatorNameEn = indicator.IndicatorNameEn,
                    IndicatorNameAr = indicator.IndicatorNameAr,
                } : new IndicatorViewModel {},
                HealthFacilityID = target.HealthFacilityID,
                HealthFacility = target.HealthFacilityID != null ? new HealthFacilityVM {
                    ID = healthFacility.HealthFacilityID,
                    HealthFacilityName = healthFacility.HealthFacilityNameEn,
                } : new HealthFacilityVM {},
                ProviderID = target.ProviderID,
                Provider = target.ProviderID != null ? new ProviderVM {
                    ProviderID = provider.ProviderID,
                    ProviderName = provider.ProviderNameEn,
                } : new ProviderVM {},
                DomainLookupID = target.DomainLookupID,
                DomainLookup = target.DomainLookupID != null ? new DomainLookupViewModel {
                    DomainLookupID = domainLookup.DomainLookupID,
                    DomainCode = domainLookup.DomainCode,
                    DomainNameEn = domainLookup.DomainNameEn,
                    DomainNameAr = domainLookup.DomainNameAr,
                } : new DomainLookupViewModel {},
                DirectorateLookupID = target.DirectorateLookupID,
                DirectorateLookup = target.DirectorateLookupID != null ? new DirectorateLookupViewModel {
                    DirectorateLookupID = directorateLookup.DirectorateLookupID,
                    DirectorateNameEn = directorateLookup.DirectorateNameEn,
                    DirectorateNameAr = directorateLookup.DirectorateNameAr,
                } : new DirectorateLookupViewModel {},
                EffectiveDate = target.EffectiveDate,
                Value = target.Value,
            };
            return Ok(targetViewModel);
        }

        [Route("api/targets")]
        [HttpPost]
        public IHttpActionResult Create(Target target)
        {
            try
            {
                if (target.IndicatorID > 0 && target.EffectiveDate != null && target.Value != null)
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
        [HttpPut]
        public IHttpActionResult Edit(int id, TargetViewModel targetViewModel)
        {
            Target target = db.Targets.Where(i => i.TargetID == id).FirstOrDefault();
            if(target == null)
            {
                return NotFound();
            }
            try
            {
                if (targetViewModel.IndicatorID > 0 && targetViewModel.EffectiveDate != null && targetViewModel.Value != null)
                {
                    target.IndicatorID = targetViewModel.IndicatorID;
                    target.HealthFacilityID = targetViewModel.HealthFacilityID;
                    target.ProviderID = targetViewModel.ProviderID;
                    target.DomainLookupID = targetViewModel.DomainLookupID;
                    target.DirectorateLookupID = targetViewModel.DirectorateLookupID;
                    target.EffectiveDate = targetViewModel.EffectiveDate;
                    target.Value = targetViewModel.Value;
                    db.SaveChanges();
                    return this.Details(target.TargetID);
                }
                return BadRequest("An error occured while updating your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/targets/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Target target = db.Targets.Where(i => i.TargetID == id).FirstOrDefault();
            if(target == null)
            {
                return NotFound();
            }
            try
            {
                db.Targets.Remove(target);
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
