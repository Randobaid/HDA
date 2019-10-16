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
        private HDAReportsContext core = new HDAReportsContext();
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
        public IHttpActionResult GetProviders(int i)
        {
            List<ProviderVM> providers = new List<ProviderVM>();

            var ps = core.Providers; //core.OutPatientEncounterTotals.Where(p => p.HealthFacilityID == id).Select(o => o.ProviderID).Distinct();
            HDAReportsContext providerDb = new HDAReportsContext();
            foreach (Provider provider in ps)
            {
                //var provider = providerDb.Providers.Where(p => p.ProviderID == providerId).First();
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
