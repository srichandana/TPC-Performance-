using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Infrastructure;
using TPC.Core.Models.ViewModels;
namespace TPC.Core.Models
{
    public class ItemListViewModel : BaseViewModel
    {
        private List<KPLBasedCommonViewModel> kplItemListVM;

        public List<KPLBasedCommonViewModel> KPLItemListVM
        {
            get { return kplItemListVM; }
            set { kplItemListVM = value; }
        }

        private ItemGroupViewModel itemGroupViewM;

        public ItemGroupViewModel ItemGroupViewM
        {
            get { return itemGroupViewM; }
            set { itemGroupViewM = value; }
        }
        
      private List<ComboBase>  PageDenomination;

      public List<ComboBase>  pageDenomination
      {
          get { return PageDenomination; }
          set { PageDenomination = value; }
      }
      private int quoteID;

      public int QuoteID
      {
          get { return quoteID; }
          set { quoteID = value; }
      }

      private int NextPage;

      public int nextPage
      {
          get { return NextPage; }
          set { NextPage = value; }
      }
      private int PreviousPage;

      public int previousPage
      {
          get { return PreviousPage; }
          set { PreviousPage = value; }
      }
      private int CurrentPageIndex;

      public int currentPageIndex
      {
          get { return CurrentPageIndex; }
          set { CurrentPageIndex = value; }
      }
    
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

      private int seletedCount;

      public int SelectionCount
      {
          get { return seletedCount; }
          set { seletedCount = value; }
      }

      private List<ItemDetailedViewModel> listItemDetailedViewModel;

      public List<ItemDetailedViewModel> ListItemDetailedViewModel
      {
          get { return listItemDetailedViewModel; }
          set { listItemDetailedViewModel = value; }
      }
      private int groupID;

      public int GroupID
      {
          get { return groupID; }
          set { groupID = value; }
      }


      private string quoteTitle;

      public string QuoteTitle
      {
          get { return quoteTitle; }
          set { quoteTitle = value; }
      }


      private int scItemsCount;

      public int SCItemsCount
      {
          get { return scItemsCount; }
          set { scItemsCount = value; }
      }

      private decimal? scItemsPrice;

      public decimal? SCItemsPrice
      {
          get { return scItemsPrice; }
          set { scItemsPrice = value; }
      }
  
      private string quoteType;

      public string QuoteType
      {
          get { return quoteType; }
          set { quoteType = value; }
      }

      private string itemIDs;

      public string ItemIDS
      {
          get { return itemIDs; }
          set { itemIDs = value; }
      }

      private decimal yesTotalPrice;

      public decimal YesTotalPrice
      {
          get { return yesTotalPrice; }
          set { yesTotalPrice = value; }
      }
        
    }
}
