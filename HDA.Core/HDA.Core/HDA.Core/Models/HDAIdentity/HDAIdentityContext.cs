using Microsoft.AspNet.Identity.EntityFramework;
using MySql.Data.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAIdentity
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class HDAIdentityContext :
        IdentityDbContext<ApplicationUser
            , ApplicationRole
        , string
        , ApplicationUserLogin
        , ApplicationUserRole
        , ApplicationUserClaim>
    {
        public HDAIdentityContext() : base("hdaidentity")
        {

        }

        public static HDAIdentityContext Create()
        {
            return new HDAIdentityContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var role = modelBuilder.Entity<ApplicationRole>()
                .ToTable("Roles");

            role.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("RoleNameIndex")
                    { IsUnique = false }));

            var user = modelBuilder.Entity<ApplicationUser>()
                .ToTable("Users");
                

            user.Property(r => r.UserName)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("UserNameIndex")
                    { IsUnique = true }));

            var userRole = modelBuilder.Entity<ApplicationUserRole>()
                .ToTable("UserRoles");

            var userLogin = modelBuilder.Entity<ApplicationUserLogin>()
                .ToTable("UserLogins");

            var userClaims = modelBuilder.Entity<ApplicationUserClaim>()
                .ToTable("UserClaims");

        }
    }
}