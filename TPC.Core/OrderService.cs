using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPC.Context;
using TPC.Context.Interfaces;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Context.EntityModel;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core
{
    public class OrderService : ServiceBase<IOrderModel>, IOrderService
    {
      
    }
}
