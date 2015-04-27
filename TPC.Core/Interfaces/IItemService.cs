using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
   public interface IItemService:IService<ItemViewModel>
    {
       ItemListDetailedViewModel GetItemListDetailByID(string itemID, string QuoteID );
       ItemDetailedViewModel GetItemByISBN(string ISBN, string QuoteID);
        List<ItemViewModel> GetItemList();
        ItemListViewModel selectedOptions(string itemID, string quoteID, int userID, string type = "", int quantity = 0);
       ItemListViewModel AddSeriesByItemID(string itemID, int quoteID, int currentUserId);
       bool UpdateQuoteDetails(string ItemID);
       ItemDetailedViewModel GetItemByID(string itemID, string QuoteID, string type = "");
       ItemListDetailedViewModel GetItemDetailedListDetailByClientID(int clientID);
       SingleItemDetailedModel GetSingleItemDetailsWithSets(string itemID, string quoteDWID);
       SingleItemDetailedModel UpdateDWSingleItemDetails(KPLBasedCommonViewModel kplVM);
       string GetItemIDsListByIsbn(string ISBN,string QuoteID);
       string getItemIdByISBN(string ISBN,string QuoteID);
       Quote GetQuoteByLoggedIn(int quoteId, string type = "");
    }
}
