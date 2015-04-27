using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;

namespace TPC.Core.Models
{
    public class ItemGroupViewModel :BaseViewModel, IItemGroupModel
    {
        public int GroupID { get; set; }
        public string GroupName { get; set; }

        private string setDescription = string.Empty;
        public string SetDescription
        {
            get { return setDescription; }
            set { setDescription = value; }
        }
        private int groupItemCount;
        public int GroupItemCount
        {
            get
            {
                if (ItemPVM != null)
                {
                    return ItemPVM.ListItemVM.Count();
                }
                else
                {
                    return 0;
                }
            }
            set {
                groupItemCount = value;
            } 
        }
        public int GroupPriority { get; set; }
        public ItemParentViewModel ItemPVM { get; set; }

        public List<ItemGroupViewModel> lstChildItemGVM { get; set; }

        public ItemGroupViewModel SearchIGVM { get; set; }
    }
}
