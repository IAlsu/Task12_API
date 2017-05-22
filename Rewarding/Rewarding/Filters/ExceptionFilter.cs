using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rewarding.Filters
{
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary {
                    { "controller", "Error" }, { "action", "Error" }
                   });
            filterContext.ExceptionHandled = true;
        }
    }
}