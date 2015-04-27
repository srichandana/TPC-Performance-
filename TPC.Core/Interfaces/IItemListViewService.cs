using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Context;
using TPC.Context.EntityModel;

namespace TPC.Core.Interfaces
{
    public interface IItemListViewService : IService<ItemListViewModel>
    {
        ItemListViewModel GetListOfItems(int quoteDWID, string type, string noOfItems, int pagno, string selectionStatus = "");

        ItemListViewModel GetItemListDetailByClientID(int clientID);
        ItemListViewModel UpdateDW(KPLBasedCommonViewModel itemViewModel, string selectionStatus, string ddlSelectedValue, int pgno);
        ItemListViewModel GetListOfItemsBySelection(int clientID, string selectionStatus, int ddlSeletedValue, int pgno);
        List<KPLBasedCommonViewModel> GetSingleListOfItems(int quoteDWID, string noOfItems, int pageno, string selectionStatus = "");
        string getGroupName(int groupID);
        List<Item> GetActiveItemList();
        List<Item> GetItemsByPackageCriteria(int packageID);
        List<Item> GetItemsByGroupCritreria(int groupID);
        string GetListItemIDsbyGroupID(int groupID, int quoteId, string selectedFilters);
        ItemListViewModel GetDWItemsList(int quoteDWID, string type, string noOfItems, int pagno, string selectionStatus = "", bool isSingle = false);
        int GetDefaultDWByUserID();
        bool CheckItemInMas(string itemID);
        List<string> DeleteNoItemsByQuoteID(int quoteID);
    }
}
