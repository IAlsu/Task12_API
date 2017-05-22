using Rewarding.App_Start;
using Rewarding.Filters;
using Rewarding.Migrations;
using Rewarding.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rewarding
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //Database.SetInitializer<PersonContext>(new AppDbInitializer());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            FilterConfig.RegisterGlobalFilters(new GlobalFilterCollection());
            BundleConfig.RegisterBundle();
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PersonContext, Configuration>());
        }
    }
}
