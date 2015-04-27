using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers.PartialControllers
{
    [TPCAuthorize]

    public class SelectTableColumnsController : BaseController
    {
        //
        // GET: /SelectTableColumns/

        public ActionResult SelectTableColumns()
        {
            return View("../TCPViews/Partial/SelectTableColumnsPartial");
        }

    }
}
