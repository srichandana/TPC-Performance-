using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models
{
    public class CartViewModel
    {
        //adding order property for email link pdf generation
        [Display(Name = "ItemID", Order = -1)]
        public string ItemId { get; set; }
        [Display(Name = "Total Price", Order = -1)]
        public double TotalPrice { get; set; }

        private string title;
        [Display(Name = "Title", Order = 2)]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string author;
        [Display(Name = "Author",Order=-1)]
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private double itemPrice;
        [Display(Name = "Item Price", Order = 6)]
        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }

        private int quantity;
        [Display(Name = "Quantity", Order = 1)]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private double price;
        [Display(Name = "Total",Order=7)]
        public double Price
        {
            get { return price = ItemPrice * Quantity; }
            set { price = value; }
        }

        private int quoteDetailID;
        [Display(Name = "QuoteDetailID",Order=-1)]
        public int QuoteDetailID
        {
            get { return quoteDetailID; }
            set { quoteDetailID = value; }
        }

        private string series;
        [Display(Name = "Series", Order = 5)]
        public string Series
        {
            get { return series; }
            set { series = value; }
        }

        private string isbn;
        [Display(Name = "ISBN", Order = 4)]
        public string ISBN
        {
            get { return isbn; }
            set { isbn = value; }
        }

        private bool includeCatalog;
        [Display(Name = "Include Catalog", Order =-1)]
        public bool IncludeCatalog
        {
            get { return includeCatalog; }
            set { includeCatalog = value; }
        }
        private string type;
        [Display(Name = "Type", Order = -1)]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string acRcLevelText;
        [Display(Name = "", Order = 3)]
        public string AcRcLevelText
        {
            get { return acRcLevelText; }
            set { acRcLevelText = value; }
        }

        private string itemStatus;
         [Display(Name = "Item Status", Order = -1)]
        public string ItemStatus
        {
            get { return itemStatus; }
            set { itemStatus = value; }
        }

         private string ar;
         [Display(Name = "AR", Order = -1)]
         public string AR
         {
             get { return ar; }
             set { ar = value; }
         }

         private string lexile;
         [Display(Name = "Lexile", Order = -1)]
         public string Lexile
         {
             get { return lexile; }
             set { lexile = value; }
         }

        private string dwstatusID;
        [Display(Name = "Sts", Order = -1)]
        public string DwstatusID
        {
            get { return dwstatusID; }
            set { dwstatusID = value; }
        }
         private string isinGroupItems;

         public string IsInGroupItems
         {
             get { return isinGroupItems; }
             set { isinGroupItems = value; }
         }


         private string productline;
         [Display(Name = "ProductLine", Order = -1)]
         public string ProductLine
         {
             get { return productline; }
             set { productline = value; }
         }

         private string rc;
         [Display(Name = "", Order = -1)]
         public string RC
         {
             get { return rc; }
             set { rc = value; }
         }
         private string leveltype;
         [Display(Name = "", Order = -1)]
         public string LevelType
         {
             get { return leveltype; }
             set { leveltype = value; }
         }
        
        
    }
}

