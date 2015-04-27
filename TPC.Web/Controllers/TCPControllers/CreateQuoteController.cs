using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]

    public class CreateQuoteController : BaseController
    {
        public ActionResult CreateQuote()
        {
            List<SelectListItem> productTypeList = new List<SelectListItem>();
            productTypeList.Add(new SelectListItem { Text = "Pre Bound", Value = "0", Selected = true });
            productTypeList.Add(new SelectListItem { Text = "Library Bound", Value = "1"});
            productTypeList.Add(new SelectListItem { Text = "Board Book", Value = "2"});
            productTypeList.Add(new SelectListItem { Text = "Puppets", Value = "0"});

            ViewBag.ddlProductType = productTypeList;

            return View("../TCPViews/CreateQuote");
        }

        [HttpPost]
        public ActionResult CreateQuotePartial()
        {
            List<SelectListItem> productTypeList = new List<SelectListItem>();

            productTypeList.Add(new SelectListItem { Text = "Preview", Value = "0" });
            productTypeList.Add(new SelectListItem { Text = "Direct Sale", Value = "1" });
            productTypeList.Add(new SelectListItem { Text = "Decision Wizard", Value = "2" });
            productTypeList.Add(new SelectListItem { Text = "Literature", Value = "3" });

            ViewBag.ddlProductType = productTypeList;
            return PartialView("../TCPViews/Partial/Quote/CreateQuotePartial");
        }

    }
}
