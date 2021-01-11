using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyCloud
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Encrypt",
                url: "MyStorage/Encrypt/{id}/{key}",
                defaults: new { controller = "MyStorage", action = "Encrypt", id = UrlParameter.Optional, key = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Decrypt",
                url: "MyStorage/Decrypt/{id}/{key}",
                defaults: new { controller = "MyStorage", action = "Decrypt", id = UrlParameter.Optional, key = UrlParameter.Optional }
            );
        }
    }
}
