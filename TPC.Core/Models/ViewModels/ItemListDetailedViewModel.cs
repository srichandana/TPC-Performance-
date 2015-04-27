using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.ViewModels
{
   public class ItemListDetailedViewModel :ItemListViewModel
    {
        private ItemDetailedViewModel itemDVM;

        public ItemDetailedViewModel ItemDVM
        {
            get { return itemDVM; }
            set { itemDVM = value; }
        }

      
    }
}
