using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
    public interface IItemGroupingService : IService<ItemGroupingViewModel>
    {
        ItemGroupingViewModel GetItemsList(string itemID);
        void AddNewGroupType(string groupType);
        bool AddGroupParentage(int childgroupName, int parentGroupID);
    }
}
