using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core.Models;
using TPC.Web.Filters;
using TPC.Core;
using TPC.Core.Interfaces;
using Microsoft.Practices.Unity;
using TPC.Core.Models.ViewModels;
using System.Web.Security;

namespace TPC.Web.Controllers.TCPControllers
{
        [TPCAuthorize]
    public class ItemGroupingController : BaseController
    {
        //
        // GET: /ItemGrouping/
       public readonly IItemGroupingService _iItemGroupingService;
       public ItemGroupingController(ItemGroupingService itemGroupingService)
        {
            _iItemGroupingService = itemGroupingService;
            _iItemGroupingService.UserVM = UserVM;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetListOfItems(string itemID)
        {
            ItemGroupingViewModel itemGroupingViewModel = new ItemGroupingViewModel();
            itemGroupingViewModel =  _iItemGroupingService.GetItemsList(itemID);
            itemGroupingViewModel.UserVM = UserVM;
            return View("../TCPViews/ItemGrouping", itemGroupingViewModel);
        }
        public ActionResult AddNewGroupType(string groupType,string itemID)
        {
            _iItemGroupingService.AddNewGroupType(groupType);
            return View("../TCPViews/ItemGrouping",_iItemGroupingService.GetItemsList(itemID));
        }
        public ActionResult AddGroupParentage(string childGroupID, string parentGroupID)
        {
            return View("../TCPViews/ItemGrouping", _iItemGroupingService.AddGroupParentage(Convert.ToInt32(childGroupID),Convert.ToInt32(parentGroupID)));
        }

    }
}
