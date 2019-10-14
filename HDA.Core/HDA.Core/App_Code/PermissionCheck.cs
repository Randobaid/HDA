using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HDA.Core.Models.HDAReports;
using HDA.Core.Models.HDAIdentity;
using Microsoft.AspNet.Identity;

namespace HDA.Core.App_Code
{
    public class PermissionCheck
    {
        private HDAReportsContext db = new HDAReportsContext();

        public List<int> GetAllowedIndicatorIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedIndicatorIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.Indicator"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            return allowedIndicatorIDs;
        }

        public List<int> GetAllowedDomainIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedDomainIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.Domain"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            return allowedDomainIDs;
        }

        public List<int> GetAllowedDirectorateIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedDirectorateIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.Directorate"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            return allowedDirectorateIDs;
        }

        public List<int> GetAllowedFacilityIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedHealthFacilityIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.HealthFacility"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            var allowedDomainIDs = this.GetAllowedDomainIds(userId);
            var allowedDirectorateIDs = this.GetAllowedDirectorateIds(userId);
            var allHealthFacilityIDs = db.HealthFacilities.Where(h => 
                allowedDomainIDs.Contains((int) h.DomainID) ||
                allowedDirectorateIDs.Contains((int) h.DirectorateID) ||
                allowedHealthFacilityIDs.Contains((int) h.HealthFacilityID)
            ).Select(h => h.HealthFacilityID).ToList();
            return allHealthFacilityIDs;
        }
    }
}