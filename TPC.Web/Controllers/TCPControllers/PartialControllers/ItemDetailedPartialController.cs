using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core.Interfaces;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers.PartialControllers
{
    [TPCAuthorize]
 
    public class ItemDetailedPartialController : BaseController
    {
        //
        // GET: /ItemDetailedPartial/
        private readonly IItemService _itemservice;
        public ItemDetailedPartialController(IItemService itemService)
        {
            _itemservice = itemService;
            _itemservice.UserVM = UserVM;
        }

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ItemDetail(string itemID, int QuoteID)
        {

            return PartialView("../TCPViews/Partial/ItemDetailedPartial", _itemservice.GetItemListDetailByID(itemID, QuoteID.ToString()));
        }

       

    }
}
