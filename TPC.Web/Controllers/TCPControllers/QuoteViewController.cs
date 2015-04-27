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

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]


    public class QuoteViewController : BaseController
    {
        //
        // GET: /QuoteView/

        private readonly IQuoteViewService _QuoteViewSrv;

        public QuoteViewController(QuoteViewService QuoteViewSrv)
        {
            _QuoteViewSrv = QuoteViewSrv;
            _QuoteViewSrv.UserVM = UserVM;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult QuoteView(int quoteid)
        {
            
            ViewData["SearchCategory"] = _QuoteViewSrv.getQuoteTypeText(quoteid);
          
            return View("../TCPViews/QuoteView", _QuoteViewSrv.GetQuoteView(quoteid));
        }

        // Changing price based on quantity
        [HttpPost]
        public ActionResult QuantityPrice(int QuoteDetailID, int quantity, string itemID, int quoteid)
        {
            ViewData["QuoteType"] = _QuoteViewSrv.getQuoteTypeText(quoteid);
            CartViewModel cartModel = new CartViewModel();
            cartModel.Quantity = quantity;
            cartModel.QuoteDetailID = QuoteDetailID;
            cartModel.ItemId = itemID;

            _QuoteViewSrv.UpdateQuantity(cartModel);
            return View("../TCPViews/QuoteView", _QuoteViewSrv.GetQuoteView(quoteid));
        }

        public bool UpdateIncludeCatalogStatus(int quoteID, bool IncludeCatalogStatus)
        {
            _QuoteViewSrv.UpdateIncludeCatalogStatus(quoteID, IncludeCatalogStatus);
            return true;
        }
    }
}
