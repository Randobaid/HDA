using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HDA.Core.Models.HDACore;
using HDA.Core.Models.HDAIdentity;
using Microsoft.AspNet.Identity;

namespace HDA.Core.App_Code
{
    public class PermissionCheck
    {
        private HDACoreContext db = new HDACoreContext();

        public List<int> GetAllowedDomainIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var userRoles = userManager.GetRoles(userId);
            var allowedDomainLookupIDs = db.DomainLookups.Where(d => userRoles.Contains(d.DomainCode)).Select(d => d.DomainLookupID).ToList();
            return allowedDomainLookupIDs;
        }

        public List<int> GetAllowedFacilityIds(string userId)
        {
            var allowedDomainLookupIDs = this.GetAllowedDomainIds(userId);
            var allowedHealthFacilityIDs = db.HealthFacilities.Where(h => allowedDomainLookupIDs.Contains((int)h.DomainLookupID)).Select(h => h.HealthFacilityID).ToList();
            return allowedHealthFacilityIDs;
        }
    }
}