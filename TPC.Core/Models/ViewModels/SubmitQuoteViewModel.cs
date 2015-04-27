using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TPC.Core.Infrastructure;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class SubmitQuoteViewModel : BaseViewModel
    {
        private string division;
        [Display(Name = "Division")]
        public string Division
        {
            get { return division; }
            set { division = value; }
        }

        private string customerNo;
        [Display(Name = "Cust No.")]
        public string CustomerNo
        {
            get { return customerNo; }
            set { customerNo = value; }
        }

        private List<ComboBase> lstSource;
        [Display(Name = "Source")]
        public List<ComboBase> LstSource
        {
            get { return lstSource; }
            set { lstSource = value; }
        }

        private List<ComboBase> lsttype;
        [Display(Name = "Type")]
        public List<ComboBase> LstType
        {
            get { return lsttype; }
            set { lsttype = value; }
        }

        private DateTime? futureBillingDate;
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Future Billing Date")]
        public DateTime? FutureBillingDate
        {
            get { return futureBillingDate; }
            set { futureBillingDate = value; }
        }

        private List<ComboBase> lstinvoiceRecipient;
        [Display(Name = "Invoice Recipient")]
        public List<ComboBase> LstInvoiceRecipient
        {
            get { return lstinvoiceRecipient; }
            set { lstinvoiceRecipient = value; }
        }

        private string poNO;
        [Display(Name = "PO No.")]
        public string PONo
        {
            get { return poNO; }
            set { poNO = value; }
        }

        private string comments;
        [Display(Name = "Comments")]
        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        private string billingReference;
        [Display(Name = "Billing Reference")]
        public string BillingReference
        {
            get { return billingReference; }
            set { billingReference = value; }
        }

        private List<ComboBase> lstPayer;
        [Display(Name = "Payer")]
        public List<ComboBase> LstPayer
        {
            get { return lstPayer; }
            set { lstPayer = value; }
        }

        private List<ComboBase> lstShiptItemsTo;
        [Display(Name = "Ship Items To")]
        public List<ComboBase> LstShipItemsTo
        {
            get { return lstShiptItemsTo; }
            set { lstShiptItemsTo = value; }
        }

        private List<ComboBase> lstAddInvRecipient;
        [Display(Name = "Add. Inv. Recipient")]
        public List<ComboBase> LstAddInvRecipent
        {
            get { return lstAddInvRecipient; }
            set { lstAddInvRecipient = value; }
        }

        //private string catalogStatusText;
        //[Display(Name = "Current Hold:")]
        //public string CatalogStatusText
        //{
        //    get { return catalogStatusText; }
        //    set { catalogStatusText = value; }
        //}

        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }

        private string invoiceRecipient;

        public string InvoiceRecipient
        {
            get { return invoiceRecipient; }
            set { invoiceRecipient = value; }
        }

        private int payer;

        public int Payer
        {
            get { return payer; }
            set { payer = value; }
        }

        private string shipItemsTo;

        public string ShipItemsTo
        {
            get { return shipItemsTo; }
            set { shipItemsTo = value; }
        }

        private string addInvRecipient;

        public string AddInvRecipient
        {
            get { return addInvRecipient; }
            set { addInvRecipient = value; }
        }

        private double dbValue;
        [Display(Name="DB:")]
        public double DBValue
        {
            get { return dbValue; }
            set { dbValue = value; }
        }

        private string invUserID;

        public string InvUserID
        {
            get { return invUserID; }
            set { invUserID = value; }
        }

        private string addInvFlag;

        public string AddInvFlag
        {
            get { return addInvFlag; }
            set { addInvFlag = value; }
        }

        private int sourceType;

        public int SourceType
        {
            get { return sourceType; }
            set { sourceType = value; }
        }
        

        private CustomerAddressesModel custAddressesModel;

        public CustomerAddressesModel CustAddressesModel
        {
            get { return custAddressesModel; }
            set { custAddressesModel = value; }
        }
        private Dictionary<string, bool> validationStatus;

        public Dictionary<string, bool> ValidationStatus
        {
            get { return validationStatus; }
            set { validationStatus = value; }
        }



    }
}
