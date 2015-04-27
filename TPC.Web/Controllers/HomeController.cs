using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Web.Filters;
using TPC.Core;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;

namespace TPC.Web.Controllers
{
      [TPCAuthorize]
    public class HomeController : BaseController
    {
        public HomeController()
        {

        }
        private readonly IItemContainerService _itemcontainerPartialSrv;

        public HomeController(ItemContainerService ItemContainerSrv)
        {
            _itemcontainerPartialSrv = ItemContainerSrv;
            _itemcontainerPartialSrv.UserVM = UserVM;
        }
         [AllowAnonymous]
        public ActionResult Index()
        {
            ViewData["CurrentPage"] = "Home";
            if (UserVM == null )
            {
                return View("", _itemcontainerPartialSrv.GetFilters());
            }
            else
            {
                _itemcontainerPartialSrv.UserVM = UserVM;
                return View("", _itemcontainerPartialSrv.GetItemContainerVM(UserVM.CRMModelProperties.LoggedINCustomerUserID));
            }
            
        }

      
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }
      
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
