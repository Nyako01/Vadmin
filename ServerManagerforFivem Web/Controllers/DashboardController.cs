using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FivemServerManager_Web.Models;

namespace FivemServerManager_Web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard/Random
        public ActionResult Home()
        {
            var home = new Dashboard() { status = "OK" };
            return View(home);
        }
    }
}