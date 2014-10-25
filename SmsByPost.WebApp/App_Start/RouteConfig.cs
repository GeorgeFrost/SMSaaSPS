using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SmsByPost
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "api",
               url: "api/Dispatch/{id}",
               defaults: new { controller = "DispatchEvent", action = "Post", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "phoneviewroute",
                url: "phoneview/{msisdn}",
                defaults: new { controller = "PhoneView", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
