﻿using System;
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
                    PhoneNumber = user.PhoneNumber,
                    Roles = new List<RoleViewModel>()
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
                PhoneNumber = user.PhoneNumber,
                Roles = new List<RoleViewModel>()
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
                        PhoneNumber = userViewModel.PhoneNumber
                    };
                    if (userViewModel.Password != null) {
                        userManager.Create(user, userViewModel.Password);
                    } else {
                        userManager.Create(user);
                    }
                    userManager.SetLockoutEnabled(user.Id, false);
                    foreach (var roleId in userViewModel.RoleIds)
                    {
                        ApplicationRole role = roleManager.FindById(roleId);
                        if(role != null) {
                            userManager.AddToRole(user.Id, role.Name);
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
        [HttpPost]
        public IHttpActionResult Edit(int id, UserViewModel user)
        {
            return Ok();
        }

        [Route("api/users/{id}")]
        [HttpPost]
        public IHttpActionResult Delete(int id, UserViewModel user)
        {
            return Ok();
        }
    }
}