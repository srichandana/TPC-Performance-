using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core;
using TPC.Core.Interfaces;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers
{

    public class FAQController : BaseController
    {
         private readonly IFAQService _faqsrv;

         public FAQController(FAQService faqSrv)
        {
            _faqsrv = faqSrv;
            _faqsrv.UserVM = UserVM;

        }

         public ActionResult Index()
         {
             return View();
         }

         public ActionResult FAQ()
         {
             _faqsrv.UserVM = UserVM;
             return View("../TCPViews/FAQ",_faqsrv.GetDetails());
         }
    }
}
