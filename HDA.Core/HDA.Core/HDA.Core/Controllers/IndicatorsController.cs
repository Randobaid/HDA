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
    public class IndicatorsController : ApiController
    {
        private HDAReportsContext db = new HDAReportsContext();

        [Route("api/indicators")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] IndicatorViewModel query)
        {
            List<IndicatorViewModel> indicators = new List<IndicatorViewModel>();
            foreach (var indicator in db.Indicators)
            {
                IndicatorViewModel i = new IndicatorViewModel {
                    IndicatorID = indicator.IndicatorID,
                    IndicatorShortName = indicator.IndicatorShortName,
                    IndicatorNameEn = indicator.IndicatorNameEn,
                    IndicatorNameAr = indicator.IndicatorNameAr,
                };
                if (!(query is null) && query.IndicatorID > 0 && i.IndicatorID != query.IndicatorID) { continue; }
                if (!(query is null) && !(query.IndicatorShortName is null) && i.IndicatorShortName != query.IndicatorShortName) { continue; }
                if (!(query is null) && !(query.IndicatorNameEn is null) && i.IndicatorNameEn != query.IndicatorNameEn) { continue; }
                if (!(query is null) && !(query.IndicatorNameAr is null) && i.IndicatorNameAr != query.IndicatorNameAr) { continue; }
                indicators.Add(i);
            }
            return Ok(indicators);
        }
    }
}
