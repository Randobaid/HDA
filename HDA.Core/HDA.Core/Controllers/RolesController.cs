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
    [Authorize(Roles = "Admin")]
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
    }
}
