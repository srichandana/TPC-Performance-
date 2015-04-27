using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models
{
    public class OrderViewModel : BaseViewModel,IOrderModel
    {
        private string invoiceNumber;

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }
        private string custNumber;

        public string CustNumber
        {
            get { return custNumber; }
            set { custNumber = value; }
        }
        private string custDivNo;

        public string CustDivNo
        {
            get { return custDivNo; }
            set { custDivNo = value; }
        }
        private string purchaseOrderNumber;

        public string PurchaseOrderNumber
        {
            get { return purchaseOrderNumber; }
            set { purchaseOrderNumber = value; }
        }
        private InvoiceModel invoiceModel;

        public InvoiceModel InvoiceModel
        {
            get { return invoiceModel; }
            set { invoiceModel = value; }
        }



        private AddressBaseModel billto;

        public AddressBaseModel BillTO
        {
            get { return billto; }
            set { billto = value; }
        }
        private ShipToAddressModel shipTo;

        public ShipToAddressModel ShipTO
        {
            get { return shipTo; }
            set { shipTo = value; }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private double salesTax;

        public double SalesTax
        {
            get { return salesTax; }
            set { salesTax = value; }
        }
        private double shippingCharge;

        public double ShippingCharge
        {
            get { return shippingCharge; }
            set { shippingCharge = value; }
        }

        private double lessDiscount;

        public double LessDiscount
        {
            get { return lessDiscount; }
            set { lessDiscount = value; }
        }
        
        public CRMModel RepoAddress { get; set; }
        public List<CartViewModel> CartListView { get; set; }

        public string CustomerName { get; set; }

     
    }
}
