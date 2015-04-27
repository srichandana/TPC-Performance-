using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models
{
    public class SearchViewModel :BaseViewModel , ISearchModel
    {
        public List<Author> author { get; set; }

        public List<SeriesAndCharacter> SeriesAndCharecter { get; set; }

        public List<Item> item { get; set; }
        
        private ItemParentViewModel itemParentVM;

        public ItemParentViewModel ItemParentVM
        {
            get { return itemParentVM; }
            set { itemParentVM = value; }
        }

        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }


        private ItemGroupViewModel itemGroupVM;

        public ItemGroupViewModel ItemGroupVM
        {
            get { return itemGroupVM; }
            set { itemGroupVM = value; }
        }
    }
}
