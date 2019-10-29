using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDAIdentity
{
    public class AccountViewModel
    {
        public class ExternalLoginViewModel
        {
            public string Name { get; set; }

            public string Url { get; set; }

            public string State { get; set; }
        }

        public class ManageInfoViewModel
        {
            public string LocalLoginProvider { get; set; }

            public string Email { get; set; }

            public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

            public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
        }

        public class UserInfoViewModel
        {
            public string Email { get; set; }
            
            //add username

            public bool HasRegistered { get; set; }

            public string LoginProvider { get; set; }

            public bool LockOutEnabled { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            //public string PhoneNumber { get; set; }

            public bool ChangePasswordNextLogin { get; set; }

            public List<UserRole> UserRoles { get; set; }

        }

        public class UserRole
        {
            public string Role { get; set; }
        }

        public class UserLoginInfoViewModel
        {
            public string LoginProvider { get; set; }

            public string ProviderKey { get; set; }


        }

        public class LoginViewModel
        {
            [Required]
            [Display(Name = "User Name")]
            [EmailAddress]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }
    }
}