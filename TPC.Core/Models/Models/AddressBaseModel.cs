using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
  public class AddressBaseModel
    {

        private string addressID;

        public string AddressID
        {
            get { return addressID; }
            set { addressID = value; }
        }

        private string customerName;

        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        private string addressLine1;

        public string AddressLine1
        {
            get { return addressLine1; }
            set { addressLine1 = value; }
        }

        private string addressLine2;

        public string AddressLine2
        {
            get { return addressLine2; }
            set { addressLine2 = value; }
        }

        private string addressLine3;

        public string AddressLine3
        {
            get { return addressLine3; }
            set { addressLine3 = value; }
        }


        private string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private string state;

        public string State
        {
            get { return state; }
            set { state = value; }
        }

        private string zipCode;

        public string ZipCode
        {
            get { return zipCode; }
            set { zipCode = value; }
        }

        private string countryCode;

        public string CountryCode
        {
            get { return countryCode; }
            set { countryCode = value; }
        }

        private string telephoneNo;

        public string TelephoneNo
        {
            get { return telephoneNo; }
            set { telephoneNo = value; }
        }

        private string telephoneExt;

        public string TelephoneExt
        {
            get { return telephoneExt; }
            set { telephoneExt = value; }
        }

        private string faxNo;

        public string FaxNo
        {
            get { return faxNo; }
            set { faxNo = value; }
        }

        private string divNo;

        public string DivNo
        {
            get { return divNo; }
            set { divNo = value; }
        }

        private string customerNo;

        public string CustomerNo
        {
            get { return customerNo; }
            set { customerNo = value; }
        }

    }
}
