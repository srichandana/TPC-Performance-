using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;

namespace TPC.Core.Interfaces
{
   public interface IQuoteViewService : IService<QuoteViewModel>
    {
       QuoteViewModel GetQuoteView(int quoteid);

       bool UpdateQuantity(CartViewModel cartViewModel);

       string getQuoteTypeText(int quotid);

       string getQuoteTitleText(int quotid);

       int getQuoteTypeId(int quotid);

       int getCustomerSCQuoteID();

       bool UpdateIncludeCatalogStatus(int quoteID, bool IncludeCatalogStatus);

       int getGroupId(string groupName="");
    }
}
