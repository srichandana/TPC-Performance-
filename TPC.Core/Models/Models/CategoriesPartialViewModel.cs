using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Common.Enumerations;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models.Models
{
   public class CategoriesPartialViewModel:BaseViewModel
    {
        private List<FilterModel> lstFilterModel;

        public List<FilterModel> LstFilterModel
        {
            get { return lstFilterModel; }
            set { lstFilterModel = value; }
        }


        public ItemGroupViewModel ItemGroupVM { get; set; }

        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private List<ComboBase> PageDenomination;

        public List<ComboBase> pageDenomination
        {
            get { return PageDenomination; }
            set { PageDenomination = value; }
        }

       private int CurrentPageIndex;

        public int currentPageIndex
        {
            get { return CurrentPageIndex; }
            set { CurrentPageIndex = value; }
        }

        private int selectedTitlesCount;

        public int SelectedTitlesCount
        {
            get { return selectedTitlesCount; }
            set { selectedTitlesCount = value; }
        }

        private double selectedTitlePrice;

        public double SelectedTitlePrice
        {
            get { return selectedTitlePrice; }
            set { selectedTitlePrice = value; }
        }

        private int selectedgroupID;

        public int SelectedGroupID
        {
            get { return selectedgroupID; }
            set { selectedgroupID = value; }
        }

        private string selectedTitleText;

        public string SelectedTitleText
        {
            get { return selectedTitleText; }
            set { selectedTitleText = value; }
        }



        private string groupDescription;

        public string GroupDescription
        {
            get { return groupDescription; }
            set { groupDescription = value; }
        }
        

        private int selectedQuoteItemCount;

        public int SelectedQuoteItemCount
        {
            get { return selectedQuoteItemCount; }
            set { selectedQuoteItemCount = value; }
        }
        private double selectedQuoteItemPrice;

        public double SelectedQuoteItemPrice
        {
            get { return selectedQuoteItemPrice; }
            set { selectedQuoteItemPrice = value; }
        }
        
    }
}
