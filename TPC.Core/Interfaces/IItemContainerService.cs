using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
   public interface IItemContainerService :IService<ItemContainerViewModel>
    {
       ItemContainerViewModel GetItemContainerVM(int QuoteID);

       KPLItemConatinerViewModel FillItemDetails(string quoteID, string type);

       bool DeleteItemFromQuote(int quoteID, string itemID,string type="");
       ItemContainerViewModel GetFilters();

       CategoriesItemContainerViewModel GetSelectedCollectionItem(int groupID, int pageIndex,string noofItemsPerPage, int quoteID, string selectedPackageIdsList="");
       List<FilterModel> getAllGroups();
       FilterModel getCollectionDetailsByID(int id);
       List<PackageModel> getAllpackage(int groupID);
       List<KPLBasedCommonViewModel> getitemlistByGruopId(int groupID);
       bool AddNewPackage(PackageModel Model);
       bool AddNewOrUpdateCollection(FilterModel filtermodel);
       bool updatepackage(List<string> lstpackageids, int groupID);
       Dictionary<string, string> updateCollectionItems(List<string> Groupids, List<string> lstItemIDs, List<string> lstUnchekItemIDs, int currentGroupID);
       bool DeleteCollection(int groupID);
       string GetGroupNamebyGroupID(int groupID);
       List<PackageModel> DeleteExistPackage(int groupID);
       bool DeletePackage(int packageid);
    }
}
