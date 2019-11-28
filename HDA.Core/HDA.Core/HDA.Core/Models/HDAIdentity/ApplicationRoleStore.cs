using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace HDA.Core.Models.HDAIdentity
{
    public class ApplicationRoleStore
        : RoleStore<ApplicationRole, string, ApplicationUserRole>,
        IQueryableRoleStore<ApplicationRole, string>,
        IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }
}