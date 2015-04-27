using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;

namespace TPC.Core.Models
{
  public   class ItemParentViewModel:IItemParentModel
    {

      public List<ItemViewModel> ListItemVM { get; set; }

    }
}
