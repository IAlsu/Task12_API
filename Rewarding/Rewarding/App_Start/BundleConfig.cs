using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace Rewarding.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundle()
        {
            var bundle = new StyleBundle("~/Content/css");
            bundle.Include("~/Content/bootstrap.css", "~/Content/Site.css");
            BundleTable.Bundles.Add(bundle);
        }
    }
}