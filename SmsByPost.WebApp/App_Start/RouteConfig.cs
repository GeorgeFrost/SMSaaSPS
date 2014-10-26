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
               name: "ArrivedAtDestinationSortingDepotRoute",
               url: "api/ArrivedAtDestinationSortingDepot/{id}",
               defaults: new { controller = "ArrivedAtDestinationSortingDepot", action = "Post", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "ArrivedAtLocalSortingHouseRoute",
               url: "api/ArrivedAtLocalSortingHouse/{id}",
               defaults: new { controller = "ArrivedAtLocalSortingHouse", action = "Post", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               name: "ArrivedAtNationalSortingHubRoute",
               url: "api/ArrivedAtNationalSortingHub/{id}",
               defaults: new { controller = "ArrivedAtNationalSortingHub", action = "Post", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "OnRouteToDeliveryRoute",
              url: "api/OnRouteToDelivery/{id}",
              defaults: new { controller = "OnRouteToDelivery", action = "Post", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "DispatchRoute",
               url: "api/Dispatch/{id}",
               defaults: new { controller = "DispatchEvent", action = "Post", id = UrlParameter.Optional }
           );

            routes.MapRoute(
                name: "PhoneViewRoute",
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
