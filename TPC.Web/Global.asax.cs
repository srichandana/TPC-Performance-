using Microsoft.Practices.Unity;
using Seibels.Claims.Web.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TPC.Core;
using TPC.Core.Mapping;
using TPC.Web.Infrastructure;

namespace TPC.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            TPCMapper.RegisterMappers();
            DIConfig.Initialise();
            AuthConfig.RegisterAuth();
            FilterConfig.RegisterHttpFilters(GlobalConfiguration.Configuration.Filters);
            ////Set for Controller Factory
            //IControllerFactory controllerFactory = new UnityControllerFactory( DIConfig.Initialise());

            //ControllerBuilder.Current.SetControllerFactory(controllerFactory);

        }
    }
}