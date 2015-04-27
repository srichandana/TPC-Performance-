using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class QuoteModel : BaseViewModel
    {
        private int quoteID;

        [Display(Name = "ID")]
        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private DateTime createdDate;

        [Display(Name = "Created")]
        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private DateTime updateDate;

        [Display(Name = "Updated")]
        public DateTime UpdateDate
        {
            get { return updateDate; }
            set { updateDate = value; }
        }

        private string quoteName;
        [Display(Name = "Name")]
        public string QuoteName
        {
            get { return quoteName; }
            set { quoteName = value; }
        }


        private string quoteTypeText;

        [Display(Name = "Quote Type")]
        public string QuoteTypeText
        {
            get { return quoteTypeText; }
            set { quoteTypeText = value; }
        }

        private int quoteTypeID;

        [Display(Name = "Quote TypeID")]
        public int QuoteTypeID
        {
            get { return quoteTypeID; }
            set { quoteTypeID = value; }
        }


        private int totalItems;

        [Display(Name = "Items")]
        public int TotalItems
        {
            get { return totalItems; }
            set { totalItems = value; }
        }

        private double totalPrice;

        [Display(Name = "Total")]
        public double TotalPrice
        {
            get { return totalPrice; }
            set { totalPrice = value; }
        }

        private string quoteText;

        public string QuoteText
        {
            get { return quoteText; }
            set { quoteText = value; }
        }

        private int custUserID;

        public int CustUserID
        {
            get { return custUserID; }
            set { custUserID = value; }
        }


        private int statusID;
        [Display(Name = "Status")]
        public int StatusID
        {
            get { return statusID; }
            set { statusID = value; }
        }

        private string statusText;

        public string StatusText
        {
            get { return statusText; }
            set { statusText = value; }
        }


        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }

        private int arDivisionNo;

        public int ARDivisionNo
        {
            get { return arDivisionNo; }
            set { arDivisionNo = value; }
        }

        private string invoiceNumber;

        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { invoiceNumber = value; }
        }
       
        private string purchaseOrderNumber;

        public string PurchaseOrderNumber
        {
            get { return purchaseOrderNumber; }
            set { purchaseOrderNumber = value; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private DateTime? quotemaildate;

        public DateTime? QuoteMailDate
        {
            get { return quotemaildate; }
            set { quotemaildate = value; }
        }
        
    }
}
