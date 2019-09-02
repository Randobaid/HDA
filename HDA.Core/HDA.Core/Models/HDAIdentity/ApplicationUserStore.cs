using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;

namespace HDA.Core.Models.HDAIdentity
{
    public class ApplicationUserStore 
        : UserStore<ApplicationUser
            , ApplicationRole
            , string
            , ApplicationUserLogin
            , ApplicationUserRole
            , ApplicationUserClaim>
        , IUserStore<ApplicationUser, string>
        , IDisposable
    {
        public ApplicationUserStore() : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }
        public ApplicationUserStore(DbContext context) : base(context)
        {

        }
    }
}