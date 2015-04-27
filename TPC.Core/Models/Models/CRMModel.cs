using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class CRMModel
    {
        private int custautoID;

        public int CustAutoID
        {
            get { return custautoID; }
            set { custautoID = value; }
        }

        private string custNO;

        public string CustNO
        {
            get { return custNO; }
            set { custNO = value; }
        }
        

        private string custFirstName = string.Empty;

        public string CustFirstName
        {
            get { return custFirstName; }
            set { custFirstName = value; }
        }

        private string custLastName = string.Empty;

        public string CustLastName
        {
            get { return custLastName; }
            set { custLastName = value; }
        }

        private string custParentName;

        public string CustParentName
        {
            get { return custParentName; }
            set { custParentName = value; }
        }
        

        private string custEmail;

        public string CustEmail
        {
            get { return custEmail; }
            set { custEmail = value; }
        }

        private string repEmail;

        public string RepEmail
        {
            get { return repEmail; }
            set { repEmail = value; }
        }

        private int repID;

        public int RepID
        {
            get { return repID; }
            set { repID = value; }
        }

        private string repName;

        public string RepName
        {
            get { return repName; }
            set { repName = value; }
        }
        private string repPhoneDirect;

        public string RepPhoneDirect
        {
            get { return repPhoneDirect; }
            set { repPhoneDirect = value; }
        }

        private string repPhoneCustomerSrvc;

        public string RepPhoneCustSrvc
        {
            get { return repPhoneCustomerSrvc; }
            set { repPhoneCustomerSrvc = value; }
        }
        
        
        

        private int divNO;

        public int DivNO
        {
            get { return divNO; }
            set { divNO = value; }
        }

        private string sessionID;

        public string SessionID
        {
            get { return sessionID; }
            set { sessionID = value; }
        }
        private string repPassword;

        public string RepPassword
        {
            get { return repPassword; }
            set { repPassword = value; }
        }
        private string persphone = string.Empty;

        public string Persphone
        {
            get { return persphone; }
            set { persphone = value; }
        }
        private bool isRepLoggedIN = false;

        public bool IsRepLoggedIN
        {
            get { return isRepLoggedIN ; }
            set { isRepLoggedIN  = value; }
        }

        private bool isReqPropFilled = false;

        public bool IsReqPropFilled
        {
            get { return isReqPropFilled; }
            set { isReqPropFilled = value; }
        }
        private int loggedINUserID;

        public int LoggedINUserID
        {
            get { return loggedINUserID; }
            set { loggedINUserID = value; }
        }

        private int loggedINCustomerUserID;

        public int LoggedINCustomerUserID
        {
            get { return loggedINCustomerUserID; }
            set { loggedINCustomerUserID = value; }
        }

        private int personID;

        public int CRMPersonID
        {
            get { return personID; }
            set { personID = value; }
        }

        private string CustName;

        public string custName
        {
            get { return CustName; }
            set { CustName = value; }
        }

        private bool isUserActive;

        public bool IsUserActive
        {
            get { return isUserActive; }
            set { isUserActive = value; }
        }
        
    }
}
