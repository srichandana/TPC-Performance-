using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models
{
    public class ItemContainerViewModel :BaseViewModel, IItemContainerModel
    {
        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private string quotetype;

        public string QuoteType
        {
            get { return quotetype; }
            set { quotetype = value; }
        }

        //public List<CategoryItemGroupModel> lstCategoryGroupModel { get; set; }
        public List<ItemGroupViewModel> ListItemGropVM { get; set; }

        public List<FilterModel> ListFilterVM { get; set; }

        public List<ComboBase> lstPostiveFilters { get; set; }

    }

}
