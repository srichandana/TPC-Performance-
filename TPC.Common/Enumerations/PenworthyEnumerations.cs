using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Common.Enumeration
{
  public static  class PenworthyEnumerations
    {
       public enum ProductTypeList
        {
            APromotion,
            CardKit,
            
            CardKitSR,
            Catalog,
            CatalogSR,
           
            DelBooks,
            Discount,
           
            FG,
            Library,
            Other,
            Paper,
            PreBound,
            Realia,
            Shipping,
            Unused,
            Video
  
         }
      public  enum subject
        {
            
            ArtScience,
            Concept,
            History,
           
            Language,
            Science,
             
            Social
                      
        }
      public  enum ReviewSource
        {
            
            BookList,
            Kirkus,
            PW,
            SLJ,
            Horn,
            LJ
        }
      public  enum InterestGrage
        {
          

        }

      public enum QuoteType
      {
          Quote=1,DecisionWhizard=2,ShoppingCart=13,WebHold=7,Literature=3,Preview=4
      }

     

    }
}
