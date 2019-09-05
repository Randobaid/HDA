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
        public IHttpActionResult Index()
        {
            List<IndicatorViewModel> targets = new List<IndicatorViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach(var target in db.Indicators)
            {
                targets.Add(new IndicatorViewModel {
                    IndicatorID = target.IndicatorID,
                    IndicatorName = target.IndicatorNameEn,
                });
            }
            return Ok(targets);
        }

        [Route("api/indicators/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            List<IndicatorViewModel> targets = new List<IndicatorViewModel>();
            DateTimeFormatInfo dateTimeFormatInfo = new DateTimeFormatInfo();
            foreach(var target in db.Indicators.Where(i => i.IndicatorID == id))
            {
                targets.Add(new IndicatorViewModel {
                    IndicatorID = target.IndicatorID,
                    IndicatorName = target.IndicatorNameEn,
                });
            }
            return Ok(targets);
        }
    }
}
