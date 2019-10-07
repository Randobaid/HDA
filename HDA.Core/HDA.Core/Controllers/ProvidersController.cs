using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace HDA.Core.Controllers
{
    public class ProvidersController : ApiController
    {
        //private HDAReportsContext db = new HDAReportsContext();
        private HDACoreContext core = new HDACoreContext();
        public IHttpActionResult GetProviders()
        {
            List<ProviderVM> providers = new List<ProviderVM>();
            foreach (var provider in core.Providers)
            {
                ProviderVM pVM = new ProviderVM
                {
                    ProviderID = provider.ProviderID,
                    ProviderName = provider.ProviderNameEn
                };
                providers.Add(pVM);
            }
            return Ok(providers);
        }
        public IHttpActionResult GetProviders(int id)
        {
            List<ProviderVM> providers = new List<ProviderVM>();

            var ps = core.OutPatientEncounterTotals.Where(p => p.HealthFacilityID == id).Select(o => o.ProviderID).Distinct();
            HDACoreContext providerDb = new HDACoreContext();
            foreach (int providerId in ps)
            {
                var provider = providerDb.Providers.Where(p => p.ProviderID == providerId).First();
                ProviderVM pVM = new ProviderVM
                {
                    ProviderID = provider.ProviderID,
                    ProviderName = provider.ProviderNameEn
                };
                providers.Add(pVM);
            }
            return Ok(providers);
        }
    }
}
