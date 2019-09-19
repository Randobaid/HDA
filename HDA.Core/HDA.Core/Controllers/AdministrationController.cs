using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HDA.Core.Controllers
{
    [Authorize]
    public class AdministrationController : Controller
    {
        // GET: Administration
        public ActionResult UserManagement()
        {
            return View();
        }

        public ActionResult DataManagement()
        {
            return View();
        }
    }
}