using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models.ViewModels
{
    public class CalTagViewModel 
    {
        private int quoteId;
        [Display(Name = "Web Quote Number")]
        public int QuoteID
        {
            get { return quoteId; }
            set { quoteId = value; }
        }

        private string merchandiseDescription;
        [Display(Name = "Merchandise Description")]
        public string MerchandiseDescription
        {
            get { return merchandiseDescription; }
            set { merchandiseDescription = value; }
        }

        private int actualWeight;
        [Display(Name = "Actual Weight")]
        public int ActualWeight
        {
            get { return actualWeight; }
            set { actualWeight = value; }
        }
        private string shipToName;
        [Display(Name = "Name")]
        public string ShipToName
        {
            get { return shipToName; }
            set { shipToName = value; }
        }
        private string primaryContact;
        [Display(Name = "Contact")]
        public string PrimaryContact
        {
            get { return primaryContact; }
            set { primaryContact = value; }
        }
        private string addressLin2;
        [Display(Name = "Address2")]
        public string AddressLine2
        {
            get { return addressLin2; }
            set { addressLin2 = value; }
        }
        private string addressLine3;
        [Display(Name = "Address3")]
        public string AddressLine3
        {
            get { return addressLine3; }
            set { addressLine3 = value; }
        }

        private string city;
        [Display(Name = "City")]
        public string City
        {
            get { return city; }
            set { city = value; }
        }
        private string state;
        [Display(Name = "State")]
        public string State
        {
            get { return state; }
            set { state = value; }
        }
        private string postalCode;
        [Display(Name = "Postal Code")]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }
        private string code;
        [Display(Name = "Code")]
        public string Code
        {
            get { return code; }
            set { code = value; }
        }
        private string returnSrvType;
        [Display(Name="Return Srv Type")]
        public string ReturnSrvType
        {
            get { return returnSrvType; }
            set { returnSrvType = value; }
        }


        private List<CalTagInfo> calTagInfo;

        public List<CalTagInfo> CalTagInfo
        {
            get { return calTagInfo; }
            set { calTagInfo = value; }
        }



        private List<ComboBase> lstsendOptions;

        public List<ComboBase> LstSendOptions
        {
            get { return lstsendOptions; }
            set { lstsendOptions = value; }
        }

        private string email;
        [Display(Name = "Email Address For ERL Send")]
        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private List<ComboBase> lstInvoiceTo;

        public List<ComboBase> LstInvoiceTo
        {
            get { return lstInvoiceTo; }
            set { lstInvoiceTo = value; }
        }
        private bool isCataloging;

        public bool IsCataloging
        {
            get { return isCataloging; }
            set { isCataloging = value; }
        }
        private string poNo;

        public string PONo
        {
            get { return poNo; }
            set { poNo = value; }
        }
        private DateTime futureDate;

        public DateTime FutureDate
        {
            get { return futureDate; }
            set { futureDate = value; }
        }

        private string invoiceTo;

        public string InvoiceTo
        {
            get { return invoiceTo; }
            set { invoiceTo = value; }
        }

    }
}
