﻿using HDA.Core.Models.HDAIdentity;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace HDA.Core.Migrations
{
    internal sealed class HDAIdentityConfig : DbMigrationsConfiguration<HDAIdentityContext>
    {
        public HDAIdentityConfig()
        {
            AutomaticMigrationsEnabled = false;
            //AutomaticMigrationDataLossAllowed = true;
            MigrationsDirectory = @"Migrations\HDAIdentity";
            MigrationsNamespace = "HDA.Core.Migrations.HDAIdentity";
        }
        protected override void Seed(HDAIdentityContext context)
        {
            SeedRoles(context);
            SeedUsers(context);
            base.Seed(context);
        }

        private void SeedUsers(HDAIdentityContext context)
        {
            var userStore = new ApplicationUserStore(context);
            var userManager = new ApplicationUserManager(userStore);

            var user = userManager.FindByName("admin@hda.core");
            if(user == null)
            {
                user = new ApplicationUser
                {
                    UserName = "admin@hda.core",
                    Email = "admin@hda.core",
                    FirstName = "System",
                    LastName = "Admin"
                };
                var result = userManager.Create(user, "@dm1n131");
                result = userManager.SetLockoutEnabled(user.Id, false);

                
            }
            var rolesForAdmin = userManager.GetRoles(user.Id);
            if (!rolesForAdmin.Contains("Admin"))
            {
                var result = userManager.AddToRole(user.Id, "Admin");
            }
        }

        private void SeedRoles(HDAIdentityContext context)
        {
            var roleStore = new ApplicationRoleStore(context);
            var roleManager = new ApplicationRoleManager(roleStore);
            var roles = new List<ApplicationRole> {
                new ApplicationRole { Name = "Admin", Description = "Administrator" },
            };
            foreach(var role in roles)
            {
                var r = roleManager.FindByName(role.Name);
                if(r == null)
                {
                    roleManager.Create(role);
                }
            }
        }
    }
}