using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAIdentity
{
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            Id = Guid.NewGuid().ToString();
        }
        public ApplicationRole(string name) : this()
        {
            this.Name = name;
        }

        public string Description { get; set; }
    }
}