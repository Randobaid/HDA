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
    public class TargetsController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();

        [Route("api/targets")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] TargetViewModel query)
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            foreach (var target in db.Targets)
            {
                Indicator indicator = new HDAReportsContext().Indicators.Where(i => i.IndicatorID == target.IndicatorID).FirstOrDefault();
                HealthFacility healthFacility = new HDAReportsContext().HealthFacilities.Where(h => h.HealthFacilityID == target.HealthFacilityID).FirstOrDefault();
                Provider provider = new HDAReportsContext().Providers.Where(i => i.ProviderID == target.ProviderID).FirstOrDefault();
                Domain domain = new HDAReportsContext().Domains.Where(i => i.DomainID == target.DomainID).FirstOrDefault();
                Directorate directorate = new HDAReportsContext().Directorates.Where(i => i.DirectorateID == target.DirectorateID).FirstOrDefault();
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
                    DomainID = target.DomainID,
                    Domain = target.DomainID > 0 ? new DomainViewModel {
                        DomainID = domain.DomainID,
                        DomainCode = domain.DomainCode,
                        DomainNameEn = domain.DomainNameEn,
                        DomainNameAr = domain.DomainNameAr,
                    } : new DomainViewModel {},
                    DirectorateID = target.DirectorateID,
                    Directorate = target.DirectorateID > 0 ? new DirectorateViewModel {
                        DirectorateID = directorate.DirectorateID,
                        DirectorateNameEn = directorate.DirectorateNameEn,
                        DirectorateNameAr = directorate.DirectorateNameAr,
                    } : new DirectorateViewModel {},
                    EffectiveDate = target.EffectiveDate,
                    Value = target.Value,
                };
                if (!(query is null) && query.TargetID > 0 && t.TargetID != query.TargetID) { continue; }
                if (!(query is null) && query.IndicatorID > 0 && t.IndicatorID != query.IndicatorID) { continue; }
                if (!(query is null) && query.HealthFacilityID.Length > 0 && t.HealthFacilityID != query.HealthFacilityID) { continue; }
                if (!(query is null) && query.ProviderID.Length > 0 && t.ProviderID != query.ProviderID) { continue; }
                if (!(query is null) && query.DomainID > 0 && t.DomainID != query.DomainID) { continue; }
                if (!(query is null) && query.DirectorateID > 0 && t.DirectorateID != query.DirectorateID) { continue; }
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
            Indicator indicator = new HDAReportsContext().Indicators.Where(i => i.IndicatorID == target.IndicatorID).FirstOrDefault();
            HealthFacility healthFacility = new HDAReportsContext().HealthFacilities.Where(h => h.HealthFacilityID == target.HealthFacilityID).FirstOrDefault();
            Provider provider = new HDAReportsContext().Providers.Where(i => i.ProviderID == target.ProviderID).FirstOrDefault();
            Domain domain = new HDAReportsContext().Domains.Where(i => i.DomainID == target.DomainID).FirstOrDefault();
            Directorate directorate = new HDAReportsContext().Directorates.Where(i => i.DirectorateID == target.DirectorateID).FirstOrDefault();
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
                DomainID = target.DomainID,
                Domain = target.DomainID > 0 ? new DomainViewModel {
                    DomainID = domain.DomainID,
                    DomainCode = domain.DomainCode,
                    DomainNameEn = domain.DomainNameEn,
                    DomainNameAr = domain.DomainNameAr,
                } : new DomainViewModel {},
                DirectorateID = target.DirectorateID,
                Directorate = target.DirectorateID > 0 ? new DirectorateViewModel {
                    DirectorateID = directorate.DirectorateID,
                    DirectorateNameEn = directorate.DirectorateNameEn,
                    DirectorateNameAr = directorate.DirectorateNameAr,
                } : new DirectorateViewModel {},
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
                    target.DomainID = targetViewModel.DomainID;
                    target.DirectorateID = targetViewModel.DirectorateID;
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
    }
}
