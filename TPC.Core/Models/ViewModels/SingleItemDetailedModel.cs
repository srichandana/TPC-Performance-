using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.ViewModels
{
    public class SingleItemDetailedModel : BaseViewModel
    {

        private KPLBasedCommonViewModel kplViewModel;

        public KPLBasedCommonViewModel KPLViewModel
        {
            get { return kplViewModel; }
            set { kplViewModel = value; }
        }

        private ItemGroupViewModel itemGroupVM;

        public ItemGroupViewModel ItemGroupVm
        {
            get { return itemGroupVM; }
            set { itemGroupVM = value; }
        }

        //private ItemParentViewModel itemParentVM;

        //public ItemParentViewModel ItemParentVM
        //{
        //    get { return itemParentVM; }
        //    set { itemParentVM = value; }
        //}

        private int NoOfYesCount;

        public int noOfYesCount
        {
            get { return NoOfYesCount; }
            set { NoOfYesCount = value; }
        }
        private int NoOfNoCount;

        public int noOfNoCount
        {
            get { return NoOfNoCount; }
            set { NoOfNoCount = value; }
        }

        private int NoOfMayBeCount;

        public int noOfMaybeCount
        {
            get { return NoOfMayBeCount; }
            set { NoOfMayBeCount = value; }
        }

        private int NoOfNewCount;

        public int noOfNewCount
        {
            get { return NoOfNewCount; }
            set { NoOfNewCount = value; }
        }

    }
}
