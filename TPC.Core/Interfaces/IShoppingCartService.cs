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
    public interface IShoppingCartService : IService<ShoppingCartViewModel>
    {
        ShoppingCartViewModel GetShoppingCartView(int quoteid);
        ShoppingCartViewModel GetShoppingCartViewByClientID(int clientID);
        ShoppingCartViewModel GetDWByClientID(string clientID, int quoteId);
      
        List<string> UpdateQuantity(int currentQuoteId, int quantity, string itemID,string type="");
        Dictionary<string, Dictionary<string, string>> UpdateQuantityByQuoteId(int currentQuoteId, int quantity);

        int CreateOrder(int QuoteID, string POText, string comments);

        List<string> DeleteItem(int quoteid, string item, int QuoteTypeID);

        int GetNoofBooksCountbyQuoteID(int quoteID);

        List<string> GetlstScDetailsbyQuoteID(int quoteID,string type);

        List<FlatFileDetailModel> GetCatalogInfoData(int customerID, int quoteID, string strInsertionType);

        CartDWPdfModel GetQuotePdfDetails(int cartDWID);

    }
}
