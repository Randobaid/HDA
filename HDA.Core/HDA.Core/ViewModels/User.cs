using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDA.Core.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public List<string> RoleIds { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public List<string> DomainIds { get; set; }
        public List<string> DirectorateIds { get; set; }
        public List<string> GovernorateIds { get; set; }
        public List<string> HealthFacilityIds { get; set; }
        public List<string> ReportIds { get; set; }
        public List<string> IndicatorIds { get; set; }
    }
}