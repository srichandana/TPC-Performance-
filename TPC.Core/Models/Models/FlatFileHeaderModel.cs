using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models.Models
{
    public class FlatFileHeaderModel
    {
        private string orderDate;
        [Display(Name = "Order Date")]
        public string OrderDate
        {
            get { return orderDate; }
            set { orderDate = value; }
        }

        private string customerNumber;
        [Display(Name = "Customer Number")]
        public string CustomerNumber
        {
            get { return customerNumber; }
            set { customerNumber = value; }
        }

        private string divisionNo = "00";
        [Display(Name = "Division No")]
        public string DivisionNo
        {
            get { return divisionNo; }
            set { divisionNo = value; }
        }


        private string customerPO;
        [Display(Name = "Customer PO")]
        public string CustomerPO
        {
            get { return customerPO; }
            set { customerPO = value; }
        }

        private string orderSubtype;
        [Display(Name = "Order Subtype")]
        public string OrderSubtype
        {
            get { return orderSubtype; }
            set { orderSubtype = value; }
        }

        private string futureBillingdate;
        [Display(Name = "Future Billing Date")]
        public string FutureBillingdate
        {
            get { return futureBillingdate; }
            set { futureBillingdate = value; }
        }

        private string crmCommunicationID;
        [Display(Name = "CRM Communication ID")]
        public string CRMCommunicationID
        {
            get { return crmCommunicationID; }
            set { crmCommunicationID = value; }
        }

        private string invoiceRecipient;
        [Display(Name = "Invoice Recipient")]
        public string InvoiceRecipient
        {
            get { return invoiceRecipient; }
            set { invoiceRecipient = value; }
        }

        private string confirmTo;
        [Display(Name = "Confirm To")]
        public string ConfirmTo
        {
            get { return confirmTo; }
            set { confirmTo = value; }
        }

        private string comment;
        [Display(Name = "Comment")]
        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        private string salesPersonID;
        [Display(Name = "Sales Person ID")]
        public string SalesPersonID
        {
            get { return salesPersonID; }
            set { salesPersonID = value; }
        }

        private string quoteNumber;
        [Display(Name = "Quote Number")]
        public string QuoteNumber
        {
            get { return quoteNumber; }
            set { quoteNumber = value; }
        }

        private string invoiceRecipientName;
        [Display(Name = "Invoice Recipient Name")]
        public string InvoiceRecipientName
        {
            get { return invoiceRecipientName; }
            set { invoiceRecipientName = value; }
        }

        private string invoiceRecipientAddress1;
        [Display(Name = "Invoice Recipient Address 1")]
        public string InvoiceRecipientAddress1
        {
            get { return invoiceRecipientAddress1; }
            set { invoiceRecipientAddress1 = value; }
        }

        private string invoiceRecipientAddress2;
        [Display(Name = "Invoice Recipient Address 2")]
        public string InvoiceRecipientAddress2
        {
            get { return invoiceRecipientAddress2; }
            set { invoiceRecipientAddress2 = value; }
        }

        private string invoiceRecipientAddress3;
        [Display(Name = "Invoice Recipient Address 3")]
        public string InvoiceRecipientAddress3
        {
            get { return invoiceRecipientAddress3; }
            set { invoiceRecipientAddress3 = value; }
        }

        private string invoiceRecipientZip;
        [Display(Name = "Invoice Recipient Zip")]
        public string InvoiceRecipientZip
        {
            get { return invoiceRecipientZip; }
            set { invoiceRecipientZip = value; }
        }

        private string invoiceRecipientCity;
        [Display(Name = "Invoice Recipient City")]
        public string InvoiceRecipientCity
        {
            get { return invoiceRecipientCity; }
            set { invoiceRecipientCity = value; }
        }

        private string invoiceRecipientState;
        [Display(Name = "Invoice Recipient State")]
        public string InvoiceRecipientState
        {
            get { return invoiceRecipientState; }
            set { invoiceRecipientState = value; }
        }

        private string billToName;
        [Display(Name = "Bill To Name")]
        public string BillToName
        {
            get { return billToName; }
            set { billToName = value; }
        }

        private string billToAddress1;
        [Display(Name = "Bill To Address 1")]
        public string BillToAddress1
        {
            get { return billToAddress1; }
            set { billToAddress1 = value; }
        }

        private string billToAddress2;
        [Display(Name = "Bill To Address 2")]
        public string BillToAddress2
        {
            get { return billToAddress2; }
            set { billToAddress2 = value; }
        }

        private string billToAddress3;
        [Display(Name = "Bill To Address 3")]
        public string BillToAddress3
        {
            get { return billToAddress3; }
            set { billToAddress3 = value; }
        }

        private string billToZip;
        [Display(Name = "Bill To Zip")]
        public string BillToZip
        {
            get { return billToZip; }
            set { billToZip = value; }
        }

        private string billToCity;
        [Display(Name = "Bill To City")]
        public string BillToCity
        {
            get { return billToCity; }
            set { billToCity = value; }
        }

        private string billToState;
        [Display(Name = "Bill To State")]
        public string BillToState
        {
            get { return billToState; }
            set { billToState = value; }
        }

        private string billingReference;
        [Display(Name = "Billing Reference")]
        public string BillingReference
        {
            get { return billingReference; }
            set { billingReference = value; }
        }

        private string shipToName;
        [Display(Name = "Ship To Name")]
        public string ShipToName
        {
            get { return shipToName; }
            set { shipToName = value; }
        }

        private string shipToAddress1;
        [Display(Name = "Ship To Address 1")]
        public string ShipToAddress1
        {
            get { return shipToAddress1; }
            set { shipToAddress1 = value; }
        }

        private string shipToAddress2;
        [Display(Name = "Ship To Address 2")]
        public string ShipToAddress2
        {
            get { return shipToAddress2; }
            set { shipToAddress2 = value; }
        }

        private string shipToAddress3;
        [Display(Name = "Ship To Address 3")]
        public string ShipToAddress3
        {
            get { return shipToAddress3; }
            set { shipToAddress3 = value; }
        }

        private string shipToZip;
        [Display(Name = "Ship To Zip")]
        public string ShipToZip
        {
            get { return shipToZip; }
            set { shipToZip = value; }
        }

        private string shipToCity;
        [Display(Name = "Ship To City")]
        public string ShipToCity
        {
            get { return shipToCity; }
            set { shipToCity = value; }
        }

        private string shipToState;
        [Display(Name = "Ship To State")]
        public string ShipToState
        {
            get { return shipToState; }
            set { shipToState = value; }
        }


        [Display(Name = "Catalog")]
        public CatalogFlatFileModel catalogFileModel { get; set; }


    }
}
