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
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Web.Controllers.TCPControllers
{
      [TPCAuthorize]

    public class OrderController : BaseController
    {
          private readonly IOrderService _orderSrv;

          public OrderController(OrderService orderSrv)
        {
            _orderSrv = orderSrv;
            _orderSrv.UserVM = UserVM;
        }

        //
        // GET: /Order/

        public ActionResult Index()
        {
            return View();
        }

        

    }
}
