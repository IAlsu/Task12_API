using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Rewarding.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(Rewarding.App_Start.Startup))]

namespace Rewarding.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // настраиваем контекст и менеджер
            app.CreatePerOwinContext<PersonContext>(PersonContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }
    }
}