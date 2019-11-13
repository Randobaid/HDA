using System.Collections.Generic;
using System.Web.Mvc;
using HDA.Core.Models.HDAReports;
using HDA.Core.ViewModels;
using Microsoft.AspNet.Identity;
using HDA.Core.Models.HDAIdentity;
using System.Web.Http;
using System;
using Newtonsoft.Json.Linq;

namespace HDA.Core.Controllers
{
    [System.Web.Mvc.Authorize]
    public class ProfileController : Controller
    {
        private HDAIdentityContext db = new HDAIdentityContext();
        public ActionResult Index()
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var user = userManager.FindById(User.Identity.GetUserId());
            var userViewModel = new UserViewModel {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                //PhoneNumber = user.PhoneNumber,
                RoleIds = new List<string>(),
                DomainIds = new List<string>(),
                DirectorateIds = new List<string>(),
                GovernorateIds = new List<string>(),
                HealthFacilityTypeIds = new List<string>(),
                HealthFacilityIds = new List<string>(),
                ReportIds = new List<string>(),
                IndicatorIds = new List<string>()
            };

            foreach(var roleName in userManager.GetRoles(user.Id))
            {
                userViewModel.RoleIds.Add(roleName);
            }
            var domains = new HDAReportsContext().Domains;
            var directorates = new HDAReportsContext().Directorates;
            var governorates = new HDAReportsContext().Governorates;
            var HealthFacilityTypes = new HDAReportsContext().HealthFacilityTypes;
            var healthFacilities = new HDAReportsContext().HealthFacilities;
            var reports = new HDAReportsContext().Reports;
            var indicators = new HDAReportsContext().Indicators;
            foreach(var claim in userManager.GetClaims(user.Id))
            {
                switch (claim.Type)
                {
                    case "HDA.Core.Models.HDAReports.Domain":
                        foreach(var domain in domains)
                        {
                            if (domain.DomainID.ToString() == claim.Value) {
                                userViewModel.DomainIds.Add(domain.DomainNameEn);
                            }
                        }
                        break;
                    case "HDA.Core.Models.HDAReports.Directorate":
                        foreach(var directorate in directorates)
                        {
                            if (directorate.DirectorateID.ToString() == claim.Value) {
                                userViewModel.DirectorateIds.Add(directorate.DirectorateNameEn);
                            }
                        }
                        break;
                     case "HDA.Core.Models.HDAReports.Governorate":
                        foreach(var governorate in governorates)
                        {
                            if (governorate.GovernorateID.ToString() == claim.Value) {
                                userViewModel.GovernorateIds.Add(governorate.GovernorateNameEn);
                            }
                        }
                        break;
                    case "HDA.Core.Models.HDAReports.HealthFacilityTypes":
                        foreach (var healthFacilityType in HealthFacilityTypes)
                        {
                            if (healthFacilityType.HealthFacilityTypeID.ToString() == claim.Value)
                            {
                                userViewModel.HealthFacilityTypeIds.Add(healthFacilityType.HealthFacilityTypeNameEn);
                            }
                        }
                        break;
                    case "HDA.Core.Models.HDAReports.HealthFacility":
                        foreach(var healthFacility in healthFacilities)
                        {
                            if (healthFacility.HealthFacilityID.ToString() == claim.Value) {
                                userViewModel.HealthFacilityIds.Add(healthFacility.HealthFacilityNameEn);
                            }
                        }
                        break;
                    case "HDA.Core.Models.HDAReports.Report":
                        foreach(var report in reports)
                        {
                            if (report.ReportID.ToString() == claim.Value) {
                                userViewModel.ReportIds.Add(report.ReportNameEn);
                            }
                        }
                        break;
                    case "HDA.Core.Models.HDAReports.Indicator":
                        foreach(var indicator in indicators)
                        {
                            if (indicator.IndicatorID.ToString() == claim.Value) {
                                userViewModel.IndicatorIds.Add(indicator.IndicatorNameEn);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            ViewBag.userViewModel = userViewModel;
            return View();
        }
    }

    [System.Web.Http.Authorize]
    public class ApiProfileController : ApiController
    {
        private HDAIdentityContext db = new HDAIdentityContext();

        [System.Web.Http.Route("api/profile")]
        [System.Web.Http.HttpPut]
        public IHttpActionResult Edit(UserViewModel userViewModel)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                if (
                    userViewModel.FirstName != null &&
                    userViewModel.LastName != null
                )
                {
                    user.FirstName = userViewModel.FirstName;
                    user.LastName = userViewModel.LastName;
                    //user.PhoneNumber = userViewModel.PhoneNumber;
                    userManager.Update(user);
                    return Ok(user);
                }
                return BadRequest("An error occured while update your profile");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [System.Web.Http.Route("api/profile/change-password")]
        [System.Web.Http.HttpPut]
        public IHttpActionResult ChangePassword([FromBody] JObject data)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var user = userManager.FindById(User.Identity.GetUserId());
            try
            {
                var currentPassword = data["CurrentPassword"].ToString();
                var password = data["Password"].ToString();

                if (
                    currentPassword != null &&
                    currentPassword != "" &&
                    password != null &&
                    password != ""
                )
                {
                    if(userManager.ChangePassword(user.Id, currentPassword, password).Succeeded == true) {
                        return Ok();
                    }
                }
                return BadRequest("An error occured while changing your password");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}