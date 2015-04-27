using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models
{
    public class ShoppingCartViewModel :BaseViewModel, IShoppingCartModel
    {
        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private int quoteTypeID;

        public int QuoteTypeID
        {
            get { return quoteTypeID; }
            set { quoteTypeID = value; }
        }

        private int custUserID;

        public int CustUserID
        {
            get { return custUserID; }
            set { custUserID = value; }
        }
        
        /* For Service*/
        public List<CartViewModel> CartListView { get; set; }

        private bool includeCatalogStatus;

        public bool IncludeCatalogStatus
        {
            get { return includeCatalogStatus; }
            set { includeCatalogStatus = value; }
        }
        private decimal salesTax;
        public decimal SalesTax
        {
            get { return salesTax; }
            set { salesTax = value; }
        }

        private string shippingAddress;
                
        public string ShippingAddress
        {
            get { return shippingAddress; }
            set { shippingAddress = value; }
        }

        private string shipto;

        public string ShipTo
        {
            get { return shipto; }
            set { shipto = value; }
        }

        private string shipToCity;

        public string ShipToCity
        {
            get { return shipToCity; }
            set { shipToCity = value; }
        }

        private string state;

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        private string zipcode;

        public string ZipCode
        {
            get { return zipcode; }
            set { zipcode = value; }
        }

        private string country;

        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        private string customername;

        public string CustomerName
        {
            get { return customername; }
            set { customername = value; }
        }
        
        
    }
}
