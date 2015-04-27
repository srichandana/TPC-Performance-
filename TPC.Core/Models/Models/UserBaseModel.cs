using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class UserBaseModel
    {
        private string userid;

        public string UserID
        {
            get { return userid; }
            set { userid = value; }
        }

        private string custID;

        public string CustomerID
        {
            get { return custID; }
            set { custID = value; }
        }
        
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }
        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string email;

        public string Email
        {
            get { return email; }
            set { email = value; }
        }
        private int rolID;

        public int RollID
        {
            get { return rolID; }
            set { rolID = value; }
        }
        private bool isLoggedIn = false;

        public bool IsLoggedIn
        {
            get { return isLoggedIn; }
            set { isLoggedIn = value; }
        }

        private string communicationEmail;

        public string CommunicationEmail
        {
            get { return communicationEmail; }
            set { communicationEmail = value; }
        }
        private string phoneNumber;
        public string PhoneNuber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        private string phoneExt;
        public string PhoneExt
        {
            get { return phoneExt; }
            set { phoneExt = value; }
        }
    }
}
