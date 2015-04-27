using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models.Models
{
    public class CustomerAccountSCDWModel
    {
        public CartWhizardInfoModel ShopingCartInfo { get; set; }
        public List<CartWhizardInfoModel> DesicionWhizardInfo { get; set; }
        public string CatalogStatus { get; set; }
        private string customerAccountID;

        public string CustomerAccountID
        {
            get { return customerAccountID; }
            set { customerAccountID = value; }
        }

        private string customerAccountName;

        public string CustomerAccountName
        {
            get { return customerAccountName; }
            set { customerAccountName = value; }
        }

        private string userEmail;

        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        private string communicationEmail;

        public string CommunicationEmail
        {
            get { return communicationEmail; }
            set { communicationEmail = value; }
        }

        private DateTime? dwInitatedMailDate;

        public DateTime? DwInitiatedMailDate
        {
            get { return dwInitatedMailDate; }
            set { dwInitatedMailDate = value; }
        }
        
    }
}
