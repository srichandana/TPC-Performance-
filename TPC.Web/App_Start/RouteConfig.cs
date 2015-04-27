using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TPC.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "Default_Collection",
               url: "GetCollection/{groupName}",
               defaults: new { controller = "ItemContainerPartial", action = "GetSelectedCollectionItemByName", groupName = UrlParameter.Optional }
           );
           // routes.MapRoute(
           //    name: "Default_Collection",
           //    url: "{Controller}/{action}",
           //    defaults: new { controller = "Default", action = "UnderConstruction" }
           //);
            routes.MapRoute(
              name: "Default_CustAcc",
              url: "{Controller}/{action}",
              defaults: new { controller = "Default", action = "GetProductDetails" },
              namespaces: new string[] { "TPC.Web.Controllers.TCPControllers" }
          );

          //  routes.MapRoute(
          //    name: "Default_Reps",
          //    url: "{Type}/{Controller}/{action}",
          //    defaults: new {controller = "ActiveQuote", action = "ActiveQuote"},
          //    namespaces: new string[] { "TPC.Web.Controllers.TCPControllers" }
          //);

            routes.MapRoute(
               name: "ShoppingCart",
               url: "{Controller}/{action}",
               defaults: new { controller = "ShoppingCart", action = "ShoppingCart" },
               namespaces: new string[] { "TPC.Web.Controllers.TCPControllers" }
           );

            routes.MapRoute(
               name: "ItemContianer",
               url: "{Controller}/{action}",
               defaults: new { controller = "ItemContainerPartial", action = "ItemContainer" },
               namespaces: new string[] { "TPC.Web.Controllers.TCPControllers" }
           );

            routes.MapRoute(
              name: "Quote",
              url: "{Controller}/{action}",
              defaults: new { controller = "CreateQuote", action = "CreateQuote" },
              namespaces: new string[] { "TPC.Web.Controllers.TCPControllers" }
          );

            routes.MapRoute(
                name: "Default_Home",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "GetProductDetails", id = UrlParameter.Optional }
            );

            routes.MapRoute(
              name: "Default_Image",
              url: "{controller}/{id}",
              defaults: new { controller = "Image", id = UrlParameter.Optional },
              namespaces: new string[] { "TPC.Web.Controllers" }
          );

            routes.MapRoute(
              name: "Default_Pdf",
              url: "{controller}/{path}",
              defaults: new { controller = "Image", path = UrlParameter.Optional },
              namespaces: new string[] { "TPC.Web.Controllers" }
               );
      
        }
    }
}