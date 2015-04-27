using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Helpers;
using System.Web.Security;
using TPC.Core;
using TPC.Core.Models.ViewModels;
using WebMatrix.WebData;

namespace TPC.Web.Filters
{
    public class MyProvider : SimpleMembershipProvider
    {
        public override bool ValidateUser(string login, string password)
        {
            LoginService lgnSrv = new LoginService();
            lgnSrv.UserVM = (UserViewModel)HttpContext.Current.Session["UserVM"];
            string hashedPassword = lgnSrv.ValidateUser(login);
            if (hashedPassword == password)
            {
                return true;
            }
            return !string.IsNullOrEmpty(hashedPassword) ? Crypto.VerifyHashedPassword(hashedPassword, password) : false;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            LoginService lgnSrv = new LoginService();
            lgnSrv.UserVM = (UserViewModel)HttpContext.Current.Session["UserVM"];
            int providerUserKey = lgnSrv.GetUserIDByEmail(username);
            return new MembershipUser(Membership.Provider.Name, username, providerUserKey, null, null, null, true, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            LoginService lgnSrv = new LoginService();
            lgnSrv.UserVM = (UserViewModel)HttpContext.Current.Session["UserVM"];
            string newPasshash = Crypto.HashPassword(newPassword);
            return lgnSrv.ChangePassword(username, newPasshash);
        }
    }
}