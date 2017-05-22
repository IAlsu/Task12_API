using System.Web.Http.Routing.Constraints;
using System.Web.Mvc;
using System.Web.Routing;

namespace Rewarding
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            //*** global error
            routes.MapRoute(
                name: "Error",
                url: "error/{error}",
                defaults: new { controller = "Error", action = "Error" }
            );

            // заменяет три закоменченных ниже роута
            routes.MapRoute(
                name: "PersonIdDeleteOrEdit",
                url: "user/{id}/{action}",
                defaults: new { controller = "Person" , action = "Details" },
                constraints: new { id = @"\d+", action = @"(Edit)|(Delete)|(Details)" }
            );

            ////***/user/10/delete
            //routes.MapRoute(
            //    name: "PersonIdDelete",
            //    url: "user/{id}/delete",
            //    defaults: new { controller = "Person", action = "Delete" },
            //    constraints: new { id = @"\d+" }
            //);

            ////***/user/10/edit
            //routes.MapRoute(
            //    name: "PersonIdEdit",
            //    url: "user/{id}/edit",
            //    defaults: new { controller = "Person", action = "Edit" },
            //    constraints: new { id = @"\d+" }
            //);

            ////***/user/10 
            //routes.MapRoute(
            //    name: "PersonId",
            //    url: "user/{id}",
            //    defaults: new { controller = "Person", action = "Details" },
            //    constraints: new { id = @"\d+" }
            //);

            //***/create-user 
            routes.MapRoute(
                name: "CreatePerson",
                url: "create-user",
                defaults: new { controller = "Person", action = "Create"}
            );

            //***user/John_Smith 
            routes.MapRoute(
                name: "PersonName",
                url: "user/{name}",
                defaults: new { controller = "Person", action = "Details", name = UrlParameter.Optional }
            );

            //***users/Alex 
            routes.MapRoute(
                name: "PeopleName",
                url: "users/{name}",
                defaults: new { controller = "Person", action = "Index", name = UrlParameter.Optional }
            );

            // то , что выше - исключает этот код
            //***users 
            //routes.MapRoute(
            //    name: "People",
            //    url: "users",
            //    defaults: new { controller = "Person", action = "Index" }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
