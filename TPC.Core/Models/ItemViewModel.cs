using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TPC.Core.Interfaces;
using System.Reflection;

namespace TPC.Core.Models
{
    public class ItemViewModel : BaseViewModel, IItemModel
    {

        private string itemID;
        [Display(Name = "ItemID", Order = 0)]
        public string ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        private string isbn;

        [Display(Name = "ISBN", Order = 25)]
        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }
        private string barcode;

        [Display(Name = "Barcode", Order = -1)]
        public string Barcode
        {
            get { return barcode; }
            set { barcode = value; }
        }

        private string description;

        [Display(Name = "Description", Order = -1)]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private bool isSelected;

        [Display(Name = "IsSelected", Order = -1)]
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private string isChecked;

        [Display(Name = "IsChecked", Order = -1)]
        public string IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }

        private string quoteFlag;
        [Display(Name = "QuoteFlag", Order = -1)]
        public string QuoteFlag
        {
            get { return quoteFlag; }
            set { quoteFlag = value; }
        }

        private int quoteID;

        [Display(Name = "QuoteID", Order = -1)]
        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }
        private string dWSelectionStatus;

        [Display(Name = "DWSelectionStatus", Order = -1)]
        public string DWSelectionStatus
        {
            get { return dWSelectionStatus; }
            set { dWSelectionStatus = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private bool isInSCDWQuote = false;
        [Display(Name = "IsInShoppingCart", Order = -1)]
        public bool IsInSCDWQuote
        {
            get { return isInSCDWQuote; }
            set { isInSCDWQuote = value; }
        }

        private string quoteTypeText;
        [Display(Name = "QuoteTypeText", Order = -1)]
        public string QuoteTypeText
        {
            get { return quoteTypeText; }
            set { quoteTypeText = value; }
        }

        private bool isInCustomerTitles;
        [Display(Name = "Title Brought Before", Order = -1)]
        public bool IsInCustomerTitles
        {
            get { return isInCustomerTitles; }
            set { isInCustomerTitles = value; }
        }
        private bool isPreviewItem;
        [Display(Name = "Preview Item", Order = -1)]
        public bool IsPreviewItem
        {
            get { return isPreviewItem; }
            set { isPreviewItem = value; }
        }

        private bool seriersBroughtBefore;
        [Display(Name = "Series Brought Before", Order = -1)]
        public bool SeriesBroughtBefore
        {
            get { return seriersBroughtBefore; }
            set { seriersBroughtBefore = value; }
        }


        private bool charecterBroughtBefore;
        [Display(Name = "Character Brought Before", Order = -1)]
        public bool CharecterBroughtBefore
        {
            get { return charecterBroughtBefore; }
            set { charecterBroughtBefore = value; }
        }
        private double iprice;
        [Display(Name = "IPrice", Order = -1)]
        public double IPrice
        {
            get { return iprice; }
            set { iprice = value; }
        }

        private string grade;
        [Display(Name = "Grade", Order = -1)]
        public string Grade
        {
            get { return grade; }
            set { grade = value; }
        }
        private string arlevel;
        [Display(Name = "ARLevel", Order = 26)]
        public string ARLevel
        {
            get { return arlevel; }
            set { arlevel = value; }
        }

        private string lexile;
        [Display(Name = "Lexile", Order = 27)]
        public string Lexile
        {
            get { return lexile; }
            set { lexile = value; }
        }


        private string rclevel;
        [Display(Name = "RCLevel", Order = 27)]
        public string RCLevel
        {
            get { return rclevel; }
            set { rclevel = value; }
        }

        private string author;
        [Display(Name = "Author", Order = -1)]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private int quantity;
          [Display(Name = "Quantity", Order = -1)]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        //private DateTime onlstdate;
        //[Display(Name = "Onlstdate", Order = -1)]
        //public DateTime Onlstdate
        //{
        //    get { return onlstdate; }
        //    set { onlstdate = value; }
        //}

        private string productLine=string.Empty;
        [Display(Name = "ProductLine", Order = -1)]
        public string ProductLine
        {
            get { return productLine; }
            set { productLine = value; }
        }

        private string status=string.Empty;
        [Display(Name = "Status", Order = -1)]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        private bool? bookList=false;
        [Display(Name = "BookList", Order = -1)]
        public bool? BookList
        {
            get { return bookList; }
            set { bookList = value; }
        }

        private string horn=string.Empty;
        [Display(Name = "Horn", Order = -1)]
        public string Horn
        {
            get { return horn; }
            set { horn = value; }
        }

        private string kirkus=string.Empty;
        [Display(Name = "Kirkus", Order = -1)]
        public string Kirkus
        {
            get { return kirkus; }
            set { kirkus = value; }
        }

        private string lj=string.Empty;
        [Display(Name = "Lj", Order = -1)]
        public string Lj
        {
            get { return lj; }
            set { lj = value; }
        }

        private string pw=string.Empty;
        [Display(Name = "Pw", Order = -1)]
        public string Pw
        {
            get { return pw; }
            set { pw = value; }
        }

        private string slj=string.Empty;
        [Display(Name = "Slj", Order = -1)]
        public string Slj
        {
            get { return slj; }
            set { slj = value; }
        }

        private string classificationType=string.Empty;
         [Display(Name = "classificationType", Order = -1)]
        public string ClassificationType
        {
            get { return classificationType; }
            set { classificationType = value; }
        }
          private string strcopyRight=string.Empty;
         [Display(Name = "strcopyRight", Order = -1)]
        public string strCopyRight
        {
            get { return strcopyRight; }
            set { strcopyRight = value; }
        }

         private string dWDate=string.Empty;
         [Display(Name = "DWDate", Order = 29)]
         public string DWDate
         {
             get { return dWDate; }
             set { dWDate = value; }
         }
        
        
    }

}
