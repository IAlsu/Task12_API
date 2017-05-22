using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rewarding.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string error)
        {
            return View((object)error);
        }
    }
}