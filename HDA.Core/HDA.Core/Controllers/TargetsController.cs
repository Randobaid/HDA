using System;
using System.Collections.Generic;
using System.Globalization;
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
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
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
                    Year = target.Year,
                    Month = target.Month,
                    MonthName = target.Month > 0 ? dateTimeFormatInfo.GetMonthName(target.Month ?? 0) : "",
                    Value = target.Value,
                };
                if (!(query is null) && query.TargetID > 0 && t.TargetID != query.TargetID) { continue; }
                if (!(query is null) && query.IndicatorID > 0 && t.IndicatorID != query.IndicatorID) { continue; }
                if (!(query is null) && query.HealthFacilityID > 0 && t.HealthFacilityID != query.HealthFacilityID) { continue; }
                if (!(query is null) && query.ProviderID > 0 && t.ProviderID != query.ProviderID) { continue; }
                if (!(query is null) && query.DomainLookupID > 0 && t.DomainLookupID != query.DomainLookupID) { continue; }
                if (!(query is null) && query.DirectorateLookupID > 0 && t.DirectorateLookupID != query.DirectorateLookupID) { continue; }
                if (!(query is null) && query.Year > 0 && t.Year != query.Year) { continue; }
                if (!(query is null) && query.Month > 0 && t.Month != query.Month) { continue; }
                targets.Add(t);
            }
            return Ok(targets);
        }

        [Route("api/targets/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach (var target in db.Targets.Where(i => i.TargetID == id))
            {
                targets.Add(new TargetViewModel {
                    TargetID = target.TargetID,
                    IndicatorID = target.IndicatorID,
                    HealthFacilityID = target.HealthFacilityID,
                    ProviderID = target.ProviderID,
                    DomainLookupID = target.DomainLookupID,
                    DirectorateLookupID = target.DirectorateLookupID,
                    EffectiveDate = target.EffectiveDate,
                    Year = target.Year,
                    Month = target.Month,
                    MonthName = target.Month > 0 ? dateTimeFormatInfo.GetMonthName(target.Month ?? 0) : "",
                    Value = target.Value,
                });
            }
            return Ok(targets);
        }
    }
}
