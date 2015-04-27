using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models.Models
{
    public class FlatFileDetailModel
    {
        private string itemNumber;
        [Display(Name = "Item Number")]
        public string ItemNumber
        {
            get { return itemNumber; }
            set { itemNumber = value; }
        }

        private int quantity;
        [Display(Name = "Quantity")]
        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private double itemPrice;
        [Display(Name = "Item Price")]
        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }

        private double price;
        [Display(Name = "Price")]
        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        private string addInvRecptFlag;
        [Display(Name = "Additional Invoice Flag")]
        public string AddInvRecptFlag
        {
            get { return addInvRecptFlag; }
            set { addInvRecptFlag = value; }
        }

        private string orderSource;
        [Display(Name = "Order Source")]
        public string OrderSource
        {
            get { return orderSource; }
            set { orderSource = value; }
        }

        private string cataloging;
        [Display(Name = "Cataloging")]
        public string Cataloging
        {
            get { return cataloging; }
            set { cataloging = value; }
        }

        private string itemCode;
         [Display(Name = "")]
        public string ItemCode
        {
            get { return itemCode; }
            set { itemCode = value; }
        }
         private string productline;
         [Display(Name = "")]
         public string ProductLine
         {
             get { return productline; }
             set { productline = value; }
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
