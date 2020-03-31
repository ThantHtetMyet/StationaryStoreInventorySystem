using ssi_system_ver_i.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ssi_system_ver_i.Controllers
{
    [RoutePrefix("ssi")]
    public class FrontController : Controller
    {

        // LOGIN
        [Route("login")]
        public ActionResult Go_Login()
        {
            return RedirectToAction("Log_In", "Staff");
        }

    }
}