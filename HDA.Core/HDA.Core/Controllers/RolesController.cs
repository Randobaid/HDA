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
        [HttpPost]
        public IHttpActionResult Edit(string id, RoleViewModel role)
        {
            return Ok();
        }

        [Route("api/roles/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(string id, RoleViewModel role)
        {
            return Ok();
        }
    }
}
