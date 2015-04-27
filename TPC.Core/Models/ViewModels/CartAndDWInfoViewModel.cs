using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models.ViewModels
{
    public class CartAndDWInfoViewModel : BaseViewModel
    {
        private int quantity;

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        private double price;

        public double Price
        {
            get { return price; }
            set { price = value; }
        }

        private int dwQuantity;

        public int DWQuantity
        {
            get { return dwQuantity; }
            set { dwQuantity = value; }
        }

        private double dwPrice;

        public double DWPrice
        {
            get { return dwPrice; }
            set { dwPrice = value; }
        }
        
        

    }
}
