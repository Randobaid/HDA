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
        public IHttpActionResult Index()
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach(var target in db.Targets)
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

        [Route("api/targets/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            List<TargetViewModel> targets = new List<TargetViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach(var target in db.Targets.Where(i => i.TargetID == id))
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
