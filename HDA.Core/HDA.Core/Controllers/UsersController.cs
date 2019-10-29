using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDAIdentity;
using Microsoft.AspNet.Identity;
using System.Security.Claims;

namespace HDA.Core.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : ApiController
    {
        private HDAIdentityContext db = new HDAIdentityContext();

        [Route("api/users")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] UserViewModel query)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var userViewModels = new List<UserViewModel>();
            foreach (var user in userManager.Users.ToList())
            {
                var userViewModel = new UserViewModel {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    //PhoneNumber = user.PhoneNumber,
                    Roles = new List<RoleViewModel>(),
                    RoleIds = new List<string>(),
                    DomainIds = new List<string>(),
                    DirectorateIds = new List<string>(),
                    GovernorateIds = new List<string>(),
                    HealthFacilityIds = new List<string>(),
                    IndicatorIds = new List<string>(),
                    ReportIds = new List<string>()
                };
                foreach(var roleName in userManager.GetRoles(user.Id))
                {
                    var role = roleManager.FindByName(roleName);
                    if (role != null) {
                        userViewModel.Roles.Add(new RoleViewModel {
                            Id = role.Id,
                            Name = role.Name,
                            Description = role.Description
                        });
                        userViewModel.RoleIds.Add(role.Id);
                    }
                }
                foreach(var claim in userManager.GetClaims(user.Id))
                {
                    switch (claim.Type)
                    {
                        case "HDA.Core.Models.HDAReports.Domain":
                            userViewModel.DomainIds.Add(claim.Value);
                            break;
                        case "HDA.Core.Models.HDAReports.Directorate":
                            userViewModel.DirectorateIds.Add(claim.Value);
                            break;
                        case "HDA.Core.Models.HDAReports.Governorate":
                            userViewModel.GovernorateIds.Add(claim.Value);
                            break;
                        case "HDA.Core.Models.HDAReports.HealthFacility":
                            userViewModel.HealthFacilityIds.Add(claim.Value);
                            break;
                        case "HDA.Core.Models.HDAReports.Report":
                            userViewModel.ReportIds.Add(claim.Value);
                            break;
                        case "HDA.Core.Models.HDAReports.Indicator":
                            userViewModel.IndicatorIds.Add(claim.Value);
                            break;
                        default:
                            break;
                    }
                }
                userViewModels.Add(userViewModel);
            }
            return Ok(userViewModels);
        }

        [Route("api/users/{id}")]
        [HttpGet]
        public IHttpActionResult Details(string id)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var user = userManager.FindById(id);
            if(user == null) {
                return NotFound();
            }
            var userViewModel = new UserViewModel {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                //PhoneNumber = user.PhoneNumber,
                Roles = new List<RoleViewModel>(),
                RoleIds = new List<string>(),
                DomainIds = new List<string>(),
                DirectorateIds = new List<string>(),
                GovernorateIds = new List<string>(),
                HealthFacilityIds = new List<string>(),
                ReportIds = new List<string>(),
                IndicatorIds = new List<string>()
            };
            foreach(var roleName in userManager.GetRoles(user.Id))
            {
                var role = roleManager.FindByName(roleName);
                if (role != null) {
                    userViewModel.Roles.Add(new RoleViewModel {
                        Id = role.Id,
                        Name = role.Name,
                        Description = role.Description
                    });
                }
            }
            foreach(var claim in userManager.GetClaims(user.Id))
            {
                switch (claim.Type)
                {
                    case "HDA.Core.Models.HDAReports.Domain":
                        userViewModel.DomainIds.Add(claim.Value);
                        break;
                    case "HDA.Core.Models.HDAReports.Directorate":
                        userViewModel.DirectorateIds.Add(claim.Value);
                        break;
                     case "HDA.Core.Models.HDAReports.Governorate":
                        userViewModel.GovernorateIds.Add(claim.Value);
                        break;
                    case "HDA.Core.Models.HDAReports.HealthFacility":
                        userViewModel.HealthFacilityIds.Add(claim.Value);
                        break;
                    case "HDA.Core.Models.HDAReports.Report":
                        userViewModel.ReportIds.Add(claim.Value);
                        break;
                    case "HDA.Core.Models.HDAReports.Indicator":
                        userViewModel.IndicatorIds.Add(claim.Value);
                        break;
                    default:
                        break;
                }
            }
            return Ok(userViewModel);
        }

        [Route("api/users")]
        [HttpPost]
        public IHttpActionResult Create(UserViewModel userViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUserManager userManager = new ApplicationUserManager(new ApplicationUserStore(db));
                    ApplicationRoleManager roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
                    var user = userManager.FindByEmail(userViewModel.Email);
                    if(user != null) {
                        return BadRequest("Already exists");
                    }
                    if (userViewModel.UserName != null) {
                        user = userManager.FindByName(userViewModel.UserName);
                        if(user != null) {
                            return BadRequest("Already exists");
                        }
                    }
                    else {
                        userViewModel.UserName = userViewModel.Email;
                    }
                    user = new ApplicationUser {
                        UserName = userViewModel.UserName,
                        FirstName = userViewModel.FirstName,
                        LastName = userViewModel.LastName,
                        Email = userViewModel.Email,
                        //PhoneNumber = userViewModel.PhoneNumber
                    };
                    if (userViewModel.Password != null) {
                        userManager.Create(user, userViewModel.Password);
                    } else {
                        userManager.Create(user);
                    }
                    userManager.SetLockoutEnabled(user.Id, false);
                    if (userViewModel.RoleIds != null) {
                        foreach (var roleId in userViewModel.RoleIds)
                        {
                            ApplicationRole role = roleManager.FindById(roleId);
                            if(role != null) {
                                userManager.AddToRole(user.Id, role.Name);
                            }
                        }
                    }
                    if (userViewModel.DomainIds != null)
                    {
                        foreach (var domainId in userViewModel.DomainIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Domain", domainId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.DirectorateIds != null)
                    {
                        foreach (var directorateId in userViewModel.DirectorateIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Directorate", directorateId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.GovernorateIds != null) {
                        foreach (var governorateId in userViewModel.GovernorateIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Governorate", governorateId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.HealthFacilityIds != null) {
                        foreach (var healthFacilityId in userViewModel.HealthFacilityIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.HealthFacility", healthFacilityId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.ReportIds != null)
                    {
                        foreach (var reportId in userViewModel.ReportIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Report", reportId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.IndicatorIds != null)
                    {
                        foreach (var indicatorId in userViewModel.IndicatorIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Indicator", indicatorId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    return this.Details(user.Id);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/users/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(string id, UserViewModel userViewModel)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var user = userManager.FindById(id);
            if(user == null)
            {
                return NotFound();
            }
            try
            {
                if (
                    userViewModel.FirstName != null &&
                    userViewModel.LastName != null &&
                    userViewModel.Email != null
                )
                {
                    user.FirstName = userViewModel.FirstName;
                    user.LastName = userViewModel.LastName;
                    user.Email = userViewModel.Email;
                    //user.PhoneNumber = userViewModel.PhoneNumber;
                    userManager.Update(user);
                    if (userViewModel.Password != null && userViewModel.Password != "") {
                        userManager.RemovePassword(user.Id);
                        userManager.AddPassword(user.Id, userViewModel.Password);
                        userManager.SetLockoutEnabled(user.Id, false);
                    }
                    foreach (var role in roleManager.Roles.ToList())
                    {
                        if (userManager.IsInRole(user.Id, role.Name)) {
                            userManager.RemoveFromRole(user.Id, role.Name);
                        }
                    }
                    // ensure remaining user has admin role
                    if (userManager.Users.Count() == 1) {
                        userManager.AddToRole(user.Id, "Admin");
                    }
                    if (userViewModel.RoleIds != null) {
                        foreach (var roleId in userViewModel.RoleIds)
                        {
                            ApplicationRole role = roleManager.FindById(roleId);
                            if(role != null) {
                                userManager.AddToRole(user.Id, role.Name);
                            }
                        }
                    }
                    // instead of checking every assigned claim against what the user has,
                    // we just remove all of them and then reassign
                    //if (userViewModel.DirectorateIds != null)
                    //{
                    foreach (var claim in userManager.GetClaims(user.Id))
                    {
                        userManager.RemoveClaim(user.Id, claim);
                    }
                    //}
                    if (userViewModel.DomainIds != null)
                    {
                        foreach (var domainId in userViewModel.DomainIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Domain", domainId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.DirectorateIds != null)
                    {
                        foreach (var directorateId in userViewModel.DirectorateIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Directorate", directorateId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.GovernorateIds != null) {
                        foreach (var governorateId in userViewModel.GovernorateIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Governorate", governorateId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.HealthFacilityIds != null) {
                        foreach (var healthFacilityId in userViewModel.HealthFacilityIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.HealthFacility", healthFacilityId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.ReportIds != null)
                    {
                        foreach (var reportId in userViewModel.ReportIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Report", reportId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    if (userViewModel.IndicatorIds != null)
                    {
                        foreach (var indicatorId in userViewModel.IndicatorIds)
                        {
                            var claim = new Claim("HDA.Core.Models.HDAReports.Indicator", indicatorId);
                            userManager.AddClaim(user.Id, claim);
                        }
                    }
                    return this.Details(user.Id);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/users/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var user = userManager.FindById(id);
            if(user == null)
            {
                return NotFound();
            }
            try
            {
                // ensure we don't delete our last user
                if (userManager.Users.Count() == 1) {
                    return BadRequest("Can't delete the only remaining user");
                }
                userManager.Delete(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
