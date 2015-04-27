using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.ViewModels
{
    public class ContactViewModel : BaseViewModel
    {

        private string repName;

        public string RepName
        {
            get { return repName; }
            set { repName = value; }
        }


        private string repEmail;

        public string RepEmail
        {
            get { return repEmail; }
            set { repEmail = value; }
        }

        private string phoneCustomerService;

        public string PhoneCustomerService
        {
            get { return phoneCustomerService; }
            set { phoneCustomerService = value; }
        }

        private string phoneDirect;

        public string PhoneDirect
        {
            get { return phoneDirect; }
            set { phoneDirect = value; }
        }

        private string repImage;

        public string RepImage
        {
            get { return repImage; }
            set { repImage = value; }
        }

        private string aboutRep;

        public string AboutRep
        {
            get { return aboutRep; }
            set { aboutRep = value; }
        }

        private string city;

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        private string street;

        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        private string pinCode;

        public string PinCode
        {
            get { return pinCode; }
            set { pinCode = value; }
        }

        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        
    }
}
