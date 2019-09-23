using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HDA.Core.ViewModels;
using HDA.Core.Models.HDAIdentity;
using Microsoft.AspNet.Identity;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class RolesController : ApiController
    {
        private HDAIdentityContext db = new HDAIdentityContext();

        [Route("api/roles")]
        [HttpGet]
        public IHttpActionResult Index([FromUri] RoleViewModel query)
        {
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var roleViewModels = new List<RoleViewModel>();
            foreach (var role in roleManager.Roles.ToList())
            {
                var roleViewModel = new RoleViewModel {
                    Id = role.Id,
                    Name = role.Name,
                    Description = role.Description
                };
                roleViewModels.Add(roleViewModel);
            }
            return Ok(roleViewModels);
        }

        [Route("api/roles/{id}")]
        [HttpGet]
        public IHttpActionResult Details(string id)
        {
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var role = roleManager.FindById(id);
            if(role == null)
            {
                return NotFound();
            }
            var roleViewModel = new RoleViewModel {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
            return Ok(roleViewModel);
        }

        [Route("api/roles")]
        [HttpPost]
        public IHttpActionResult Create(RoleViewModel roleViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationRoleManager roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
                    var role = roleManager.FindByName(roleViewModel.Name);
                    if(role != null)
                    {
                        return BadRequest("Already exists");
                    }
                    role = new ApplicationRole {
                        Name = roleViewModel.Name,
                        Description = roleViewModel.Description
                    };
                    roleManager.Create(role);
                    return this.Details(role.Id);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/roles/{id}")]
        [HttpPut]
        public IHttpActionResult Edit(string id, RoleViewModel roleViewModel)
        {
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var role = roleManager.FindById(id);
            if(role == null)
            {
                return NotFound();
            }
            if(role.Name == "Admin")
            {
                return BadRequest("You cannot edit the Admin role");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    role.Name = roleViewModel.Name;
                    role.Description = roleViewModel.Description;
                    roleManager.Update(role);
                    return this.Details(role.Id);
                }
                return BadRequest("An error occured while saving your record");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("api/roles/{id}")]
        [HttpDelete]
        public IHttpActionResult Delete(string id)
        {
            var userManager = new ApplicationUserManager(new ApplicationUserStore(db));
            var roleManager = new ApplicationRoleManager(new ApplicationRoleStore(db));
            var role = roleManager.FindById(id);
            if(role == null)
            {
                return NotFound();
            }
            if(role.Name == "Admin")
            {
                return BadRequest("You cannot delete the Admin role");
            }
            try
            {
                foreach (var user in userManager.Users.ToList())
                {
                    if(userManager.IsInRole(user.Id, role.Name)) {
                        userManager.RemoveFromRole(user.Id, role.Name);
                    }
                }
                roleManager.Delete(role);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
