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
    public class IndicatorsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();

        [Route("api/indicators")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] IndicatorViewModel query)
        {
            List<IndicatorViewModel> indicators = new List<IndicatorViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach (var indicator in db.Indicators)
            {
                IndicatorViewModel i = new IndicatorViewModel {
                    IndicatorID = indicator.IndicatorID,
                    IndicatorName = indicator.IndicatorNameEn,
                };
                if (!(query is null) && query.IndicatorID > 0 && i.IndicatorID != query.IndicatorID) { continue; }
                if (!(query is null) && !(query.IndicatorName is null) && i.IndicatorName != query.IndicatorName) { continue; }
                indicators.Add(i);
            }
            return Ok(indicators);
        }

        [Route("api/indicators/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            List<IndicatorViewModel> indicators = new List<IndicatorViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach (var indicator in db.Indicators.Where(i => i.IndicatorID == id))
            {
                indicators.Add(new IndicatorViewModel {
                    IndicatorID = indicator.IndicatorID,
                    IndicatorName = indicator.IndicatorNameEn,
                });
            }
            return Ok(indicators);
        }
    }
}
