using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Infrastructure;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class ItemGroupingViewModel : BaseViewModel
    {
        private KPLBasedCommonViewModel kplItemListVM;

        public KPLBasedCommonViewModel KPLItemListVM
        {
            get { return kplItemListVM; }
            set { kplItemListVM = value; }
        }
        private List<ComboBase> itemIDs;

        public List<ComboBase> ItemIDs
        {
            get { return itemIDs; }
            set { itemIDs = value; }
        }

        private List<ItemGroupViewModel> lstitemGVM;

        public List<ItemGroupViewModel> LstItemGVM
        {
            get { return lstitemGVM; }
            set { lstitemGVM = value; }
        }

        private List<ComboBase> lstGroupTypes;

        public List<ComboBase> LstGroupTypes
        {
            get { return lstGroupTypes; }
            set { lstGroupTypes = value; }
        }
        

      

    }
}
