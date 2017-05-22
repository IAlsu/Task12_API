using Rewarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rewarding.Controllers
{
    public abstract class SessionController : Controller
    {
        public TempChange GetTempChange()
        {
            TempChange tempChange = (TempChange)Session["TempChange"];
            if (tempChange == null)
            {
                tempChange = new TempChange();
                Session["TempChange"] = tempChange;
            }
            return tempChange;
        }
        public RedirectToRouteResult RemoveFromTempChanges(string returnUrl)
        {
            Session.Remove("TempChange");
            return RedirectToAction("Index", new { returnUrl });
        }
        

    }
}