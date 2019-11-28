using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HDA.Core.Models.HDAReports;
using HDA.Core.Models.HDAIdentity;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Utilities
{
    public class PermissionCheck
    {
        private HDAReportsContext db = new HDAReportsContext();

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
        
        public List<int> GetAllowedGovernorateIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedGovernorateIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.Governorate"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            return allowedGovernorateIDs;
        }

        public List<int> GetAllowedFacilityIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedHealthFacilityIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.HealthFacility"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            var allowedDomainIDs = this.GetAllowedDomainIds(userId);
            var allowedDirectorateIDs = this.GetAllowedDirectorateIds(userId);
            var allowedGovernorateIDs = this.GetAllowedGovernorateIds(userId);
            var allHealthFacilityIDs = db.HealthFacilities.Where(h => 
                allowedDomainIDs.Contains((int) h.DomainID) ||
                allowedDirectorateIDs.Contains((int) h.DirectorateID) ||
                allowedGovernorateIDs.Contains((int) h.GovernorateID) ||
                allowedHealthFacilityIDs.Contains((int) h.HealthFacilityID)
            ).Select(h => h.HealthFacilityID).ToList();
            return allHealthFacilityIDs;
        }

        public List<int> GetAllowedReportIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedReportIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.Report"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            return allowedReportIDs;
        }

        public List<int> GetAllowedIndicatorIds(string userId)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(new HDAIdentityContext()));
            var allowedIndicatorIDs = userManager.GetClaims(userId).Where(
                c => c.Type == "HDA.Core.Models.HDAReports.Indicator"
            ).Select(c => Convert.ToInt32(c.Value)).ToList();
            return allowedIndicatorIDs;
        }

        public Boolean IsAllowedOnReport(string reportName, string userId)
        {
            var allowedReportIDs = this.GetAllowedReportIds(userId);
            var reportNo = db.Reports.Where(r =>
                allowedReportIDs.Contains(r.ReportID) &&
                (
                    r.ReportCode == reportName ||
                    r.ReportNameAr == reportName ||
                    r.ReportNameEn == reportName
                )
            ).Count();
            if (reportNo >= 1) {
                return true;
            } else {
                return false;
            }
        }
    }
}