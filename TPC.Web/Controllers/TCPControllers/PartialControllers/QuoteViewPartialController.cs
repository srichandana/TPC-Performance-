using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]

    public class QuoteViewPartialController : BaseController
    {
        //// GET: /QuoteViewPartial/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuoteView(string quoteID)
        {
            return View("../TCPViews/QuoteView");
        }

    }
}
