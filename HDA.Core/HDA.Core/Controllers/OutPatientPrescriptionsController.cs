using HDA.Core.Models.HDACore;
using HDA.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HDA.Core.Controllers
{
    public class OutPatientPrescriptionsController : ApiController
    {
        private HDACoreContext db = new HDACoreContext();
        public IHttpActionResult GetPrescriptionsPerInstitution([FromUri] OPRequest payload)
        {
            if (ModelState.IsValid)
            {
                DateTime fromDate = Convert.ToDateTime(payload.FromDate);
                DateTime toDate = Convert.ToDateTime(payload.ToDate);
                return Ok();
            }
            else
            {
                return BadRequest(ModelState);
            }

            
        }
    }
}
