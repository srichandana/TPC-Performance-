using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class CartDWPdfModel : QuoteViewModel
    {
       public CRMModel RepoAddress { get; set; }
       private int quoteTypeID;
       public AddressBaseModel CustomerAddress { get; set; }
       public int QuoteTypeID
       {
           get { return quoteTypeID; }
           set { quoteTypeID = value; }
       }
        
    }
}
