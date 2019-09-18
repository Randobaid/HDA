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

        [Route("api/indicators/{id}")]
        [HttpGet]
        public IHttpActionResult Details(int id)
        {
            Indicator indicator = db.Indicators.Where(i => i.IndicatorID == id).FirstOrDefault();
            IndicatorViewModel indicatorVM = new IndicatorViewModel {
                IndicatorID = indicator.IndicatorID,
                IndicatorShortName = indicator.IndicatorShortName,
                IndicatorNameEn = indicator.IndicatorNameEn,
                IndicatorNameAr = indicator.IndicatorNameAr,
            };
            return Ok(indicatorVM);
        }

        [Route("api/indicators")]
        [HttpPost]
        public IHttpActionResult Create(Indicator indicator)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Indicators.Add(indicator);
                    db.SaveChanges();
                    return Ok(indicator);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/indicators/{id}")]
        [HttpPost]
        public IHttpActionResult Edit(int id, Indicator indicator)
        {
            return Ok();
        }

        [Route("api/indicators/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(int id, Indicator indicator)
        {
            return Ok();
        }
    }
}
