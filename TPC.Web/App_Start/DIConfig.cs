using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Unity.Mvc4;
using TPC.Core.Interfaces;
using TPC.Core;
using TPC.Core.Models;
using TPC.Web.Controllers.TCPControllers;
using Microsoft.Practices.Unity.Configuration;
//using TPC.Context;
//using TPC.Context.Interfaces;
//using TPC.Context.Infrastructure;

namespace Seibels.Claims.Web.App_Start
{
    public class DIConfig 
    {
        public static IUnityContainer Initialise()
        {
            var container = BuildUnityContainer();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            //GlobalConfiguration.Configuration.DependencyResolver = new WebApi.UnityDependencyResolver(container);

            return container;
        }

        private static IUnityContainer BuildUnityContainer()
        {
            IUnityContainer container = new UnityContainer()
    .LoadConfiguration();

           // var container =new UnityContainer();
           
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>(); 
      
            //container.RegisterType<IItemService, ItemService>();
            //container.RegisterType<IItemRepository, ItemRepository>();
            //container.RegisterType<IRepositoryContext, TPCRepositaryContext>();
          
           // container.RegisterInstance(typeof(ActiveQuoteController));
          
           // RegisterTypes(container);

            return container;
        }

        public static void RegisterTypes(IUnityContainer container)
        {

        }


    }
}
