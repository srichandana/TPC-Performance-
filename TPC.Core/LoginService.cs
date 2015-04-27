using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Common.Enumerations;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;
using TPC.Core.Models.ViewModels;
using System.Configuration;
using TPC.Core.Models.Models;
using System.Data.SqlClient;
using TPC.Core.Infrastructure;
using TPC.Core.Interfaces;

namespace TPC.Core
{
    public class LoginService : ServiceBase<IUserViewModel>, ILoginService
    {
        IContextBase _context = new ContextBase();
        public UserViewModel FillUserVMByUserID(int userID, UserViewModel usrModelProp)
        {
            User usr = _context.User.GetSingle(e => e.UserId == userID);
            if (usr != null)
            {
                FillCRMProperties(usrModelProp, usr);
                FillPreferences(usrModelProp);
            }
            List<Quote> lstDw = _context.Quote.GetAll(e => e.UserID == usrModelProp.CRMModelProperties.LoggedINCustomerUserID &&
        e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == (int)QuoteStatusEnum.Open).ToList();

            usrModelProp.CurrentQuoteID = _context.Quote.GetSingle(e => e.UserID == usrModelProp.CRMModelProperties.LoggedINCustomerUserID &&
              e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.StatusID == (int)QuoteStatusEnum.Open).QuoteID;

            usrModelProp.DWDetails = new Dictionary<int, string>();
            if (lstDw.Count != 0)
            {
                for (int i = 0; i < lstDw.Count; i++)
                {
                    usrModelProp.DWDetails.Add(lstDw[i].QuoteID, lstDw[i].QuoteTitle);
                }
            }

            return usrModelProp;
        }

        private void
            FillCRMProperties(UserViewModel usrModelProp, User usr)
        {
            if (usr.webpages_Roles.FirstOrDefault().RoleId == (int)UserRolesEnum.Repo || usr.webpages_Roles.FirstOrDefault().RoleId == (int)UserRolesEnum.AdminRep)
            {
                User custUser = null;
                if (usrModelProp.CRMModelProperties != null)
                    custUser = InsertUpdateUser(usrModelProp);

                usrModelProp.CRMModelProperties.CustFirstName = custUser.FirstName;
                usrModelProp.CRMModelProperties.CustLastName = custUser.LastName;
                usrModelProp.CRMModelProperties.CustEmail = custUser.Email;
                usrModelProp.CRMModelProperties.CustAutoID = Convert.ToInt32(custUser.CustAutoID);
                usrModelProp.CRMModelProperties.custName = custUser.Customer == null ? string.Empty : custUser.Customer.CustomerName;
                usrModelProp.CRMModelProperties.CustNO = custUser.Customer.CustomerNO;
                usrModelProp.CRMModelProperties.CustParentName = custUser.Customer.CustomerName;
                usrModelProp.CRMModelProperties.LoggedINCustomerUserID = custUser.UserId;
                usrModelProp.CRMModelProperties.CRMPersonID = Convert.ToInt32(custUser.PersonID);
                // usrModelProp.CRMModelProperties.Persphone = usrModelProp.CustomerAccountInfoObj.PhoneNuber;
                usrModelProp.CRMModelProperties.RepEmail = usr.Email;
                usrModelProp.CRMModelProperties.RepID = usrModelProp.CRMModelProperties.RepID; //(int)usr.RepUsers.FirstOrDefault().RepID;
                usrModelProp.CRMModelProperties.RepName = usr.UserName;
                usrModelProp.CRMModelProperties.RepPhoneDirect = usr.RepUsers.FirstOrDefault().PhoneDirect;
                usrModelProp.CRMModelProperties.RepPhoneCustSrvc = usr.RepUsers.FirstOrDefault().PhoneCustomerService;
                usrModelProp.CRMModelProperties.IsUserActive = (bool)custUser.ISActive;

                usrModelProp.CRMModelProperties.DivNO = usrModelProp != null && usrModelProp.CRMModelProperties != null ? usrModelProp.CRMModelProperties.DivNO : (int)custUser.Customer.DivisionNo;

                usrModelProp.CRMModelProperties.LoggedINUserID = usr.UserId;
                usrModelProp.CRMModelProperties.IsRepLoggedIN = true;
                usrModelProp.CRMModelProperties.IsReqPropFilled = true;
                usrModelProp.CustomerAddressBaseModel = GetCustomerShipToAddress(usrModelProp.CRMModelProperties.CustAutoID);
            }
            else
            {
                RepUser repUser = null;
                if (usr.Customer.CustomerRep == null)
                {
                    repUser = _context.RepUser.GetSingle(e => e.RepUserID == 25);//Defualt repuerid - 25 - Julie.Plantz
                }
                else
                {
                    repUser = _context.RepUser.GetSingle(e => e.RepUserID == usr.Customer.CustomerRep.RepUserID);
                }
                usrModelProp.CRMModelProperties = new CRMModel();
                usrModelProp.CRMModelProperties.CustFirstName = usr.FirstName;
                usrModelProp.CRMModelProperties.CustLastName = usr.LastName;
                usrModelProp.CRMModelProperties.CustEmail = usr.Email;
                usrModelProp.CRMModelProperties.CustAutoID = Convert.ToInt32(usr.CustAutoID);
                usrModelProp.CRMModelProperties.CustParentName = usr.Customer.CustomerName;
                usrModelProp.CRMModelProperties.LoggedINCustomerUserID = usr.UserId;
                usrModelProp.CRMModelProperties.CRMPersonID = Convert.ToInt32(usr.PersonID);
                usrModelProp.CRMModelProperties.CustNO = usr.Customer.CustomerNO;
                usrModelProp.CRMModelProperties.custName = usr.Customer == null ? string.Empty : usr.Customer.CustomerName;
                // usrModelProp.CRMModelProperties.Persphone = usrModelProp.CustomerAccountInfoObj.PhoneNuber;
                usrModelProp.CRMModelProperties.RepEmail = repUser.User.Email;
                usrModelProp.CRMModelProperties.RepID = (int)repUser.RepID;
                usrModelProp.CRMModelProperties.RepName = repUser.User.UserName;
                usrModelProp.CRMModelProperties.RepPhoneDirect = repUser.PhoneDirect;
                usrModelProp.CRMModelProperties.RepPhoneCustSrvc = repUser.PhoneCustomerService;
                usrModelProp.CRMModelProperties.IsUserActive = (bool)usr.ISActive;

                usrModelProp.CRMModelProperties.DivNO = usrModelProp != null && usrModelProp.CRMModelProperties != null ? usrModelProp.CRMModelProperties.DivNO : (int)usr.Customer.DivisionNo;
                usrModelProp.SCCount = GetSCCountByCustID(Convert.ToInt32(usr.UserId));
                usrModelProp.SCPrice = GetSCPriceByCustID(Convert.ToInt32(usr.UserId));

                usrModelProp.CRMModelProperties.LoggedINUserID = usr.UserId;
                usrModelProp.CRMModelProperties.IsRepLoggedIN = false;
                usrModelProp.CRMModelProperties.IsReqPropFilled = true;
                usrModelProp.CustomerAddressBaseModel = GetCustomerShipToAddress(usrModelProp.CRMModelProperties.CustAutoID);
            }
        }

        private User InsertUpdateUser(UserViewModel usrModelProp)
        {
            User user;
            SqlParameter[] parameter = {
                                           new SqlParameter("@personId", usrModelProp.CRMModelProperties.CRMPersonID),
                                               new SqlParameter("@repId", usrModelProp.CRMModelProperties.RepID),
                                                new SqlParameter("@customerNo", usrModelProp.CRMModelProperties.CustNO),
                                                 new SqlParameter("@ardivisionNO", usrModelProp.CRMModelProperties.DivNO)
                                        };

            _context.User.ExecSp("sp_InserUpdateUserByPersonId @personId,@repId,@customerNo,@ardivisionNO", parameter);
            user = _context.User.GetSingle(e => e.PersonID == usrModelProp.CRMModelProperties.CRMPersonID);
            return user;
        }

        private ShipToAddressModel GetCustomerShipToAddress(int CustAutoID)
        {
            ShipToAddressModel CustomerAddressBaseModel = new ShipToAddressModel();

            CustomerShipToAddress customerShipToAddress = _context.CustomerShipToAddress.GetSingle(e => e.CustAutoID == CustAutoID);
            if (customerShipToAddress != null)
            {
                CustomerAddressBaseModel.ShipToAddress = new AddressBaseModel();
                CustomerAddressBaseModel.ShipToAddress.AddressLine1 = customerShipToAddress.ShipToAddress1 != null ? customerShipToAddress.ShipToAddress1 : customerShipToAddress.ShipToAddress2;
                CustomerAddressBaseModel.ShipToName = customerShipToAddress.ShipToName;
                CustomerAddressBaseModel.ShipToAddress.City = customerShipToAddress.ShipToCity;
                CustomerAddressBaseModel.ShipToAddress.State = customerShipToAddress.ShipToState;
                CustomerAddressBaseModel.ShipToAddress.ZipCode = customerShipToAddress.ShipToZipCode;
                CustomerAddressBaseModel.ShipToAddress.CountryCode = customerShipToAddress.ShipToCountryCode;
            }
            return CustomerAddressBaseModel;
        }

        private User GetUserByCustNoandDiVNoEmail(string custNo, int divNo, string custUserEmail)
        {
            return _context.Customer.GetSingle(e =>
                       e.CustomerNO == custNo).Users.Where(e =>
                       e.Email == custUserEmail).FirstOrDefault();
        }
        public void FillPreferences(UserViewModel userVM)
        {
            int currentUserID = userVM.CRMModelProperties.LoggedINUserID;
            List<UserPreference> lstuserPreference = _context.UserPreference.GetAll(e => e.UserID == currentUserID).ToList();
            userVM.Preferences = new Dictionary<string, string>();
            if (lstuserPreference.Count != 0)
            {
                userVM.Preferences = lstuserPreference.ToDictionary(d => d.PreferredType, d => d.Preferred);
            }
        }

        private int GetSCCountByCustID(int UserID)
        {
            return _context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserID).QuoteDetails.Sum(e => e.Quantity);
        }

        private decimal? GetSCPriceByCustID(int UserID)
        {
            Quote CurrentQuote = _context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserID);
            string taxschuduleID = CurrentQuote.User.Customer != null ? CurrentQuote.User.Customer.TaxScheduleID : null;
            decimal SalesTax = 0;
            if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                SalesTax = 0;
            else
                SalesTax = Convert.ToDecimal(_context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);
            decimal? price = CurrentQuote.QuoteDetails.Sum(z => z.Item.Price * z.Quantity);
            decimal? totalPrice = price + (price * SalesTax);
            return totalPrice;
        }

        public int GetUserIDByEmail(string email)
        {
            User usr = GetUserDetailsByEmail(email);
            return GetActiveUserID(usr);
          
        }

        private User GetUserDetailsByEmail(string email)
        {
            string[] emailAndCustAutoID = email.Split('~');
            string emailOnly = emailAndCustAutoID[0];
            string custAutoID = emailAndCustAutoID.Count() > 1 ? emailAndCustAutoID[1] : string.Empty;

            if (!string.IsNullOrEmpty(custAutoID))
            {
                int custAutoIDint = Convert.ToInt32(custAutoID);
                return _context.User.GetSingle(e => e.Email == emailOnly && e.UserId == custAutoIDint && e.PersonID != null);
            }
            else
            {
                return _context.User.GetSingle(e => e.Email == emailOnly && e.ISActive == true);
            }
        }

        public int GetActiveUserID(User usr)
        {
            int userID = 0;
            if (usr != null)
            {
                if (GetUserStatusbyRoleID(usr))
                {
                    userID = usr.UserId;
                }
            }
            return userID;
        }

        public bool GetUserStatusbyRoleID(User usr)
        {
            int RoleID = usr.webpages_Roles.FirstOrDefault().RoleId;
            if (RoleID == (int)UserRolesEnum.Repo || RoleID == (int)UserRolesEnum.AdminRep)
            {
                int personID = (UserVM == null || UserVM.CRMModelProperties == null) ? 0 : UserVM.CRMModelProperties.CRMPersonID;
                return GetUserStatusbyPersonID(personID);
            }
            else
            {
                return (bool)usr.ISActive  && usr.PersonID!=null;
            }

        }

        public User GetUserByEmail(string email)
        {
            User usr = null;
            string[] emailAndCustAutoID = email.Split('~');
            string emailOnly = emailAndCustAutoID[0];
            string custAutoID = emailAndCustAutoID.Count() > 1 ? emailAndCustAutoID[1] : string.Empty;
            if (!string.IsNullOrEmpty(custAutoID))
            {
                int custAutoIDint = Convert.ToInt32(custAutoID);
                usr = _context.User.GetSingle(e => e.Email == emailOnly && e.CustAutoID == custAutoIDint);
            }
            else
            {
                usr = _context.User.GetSingle(e => e.Email == emailOnly);
            }
            return usr;
        }

        public string GetUserPWHashByID(int uID)
        {
            webpages_Membership usrMembership = _context.webpages_Membership.GetSingle(e => e.UserId == uID);
            if (usrMembership != null)
                return usrMembership.Password;
            else
                return string.Empty;
        }

        public bool IsUserInRole(string username, string roleName)
        {
            return _context.User.GetSingle(e => e.Email == username).webpages_Roles.Where(e => e.RoleName == roleName).FirstOrDefault() != null ? true : false;
        }

        public string[] GetRolesForUser(string username)
        {
            int userID = GetUserIDByEmail(username);
            return _context.User.GetSingle(e => e.UserId == userID).webpages_Roles.Select(e => e.RoleName).ToArray();
        }

        public string ValidateUser(string login)
        {
            int usrID = GetUserIDByEmail(login);
            if (usrID != 0)
            {
                return GetUserPWHashByID(usrID);
            }
            else
                return string.Empty;
        }
        public List<ComboBase> GetAllCustomersByUserEmail(string email)
        {

            List<ComboBase> custDetails = new List<ComboBase>();
            List<User> lstUsers = _context.User.GetAll(e => e.Email == email && e.ISActive == true && e.PersonID != null).ToList();
            custDetails = lstUsers.Select(e => new ComboBase { ItemID = e.UserId.ToString(), ItemValue = e.Customer != null ? e.UserName + "--" + e.Customer.CustomerName : string.Empty }).ToList();
            return custDetails;
        }

        public bool ChangePassword(string username, string newPassword)
        {
            int usrID = GetUserIDByEmail(username);
            if (usrID != 0)
            {
                webpages_Membership webMembership = _context.webpages_Membership.GetSingle(e => e.UserId == usrID);
                webMembership.Password = newPassword;
                _context.webpages_Membership.SaveChanges();
                return true;
            }
            return false;
        }

        public Dictionary<string, string> GetUserEmailPasswordByQuoteID(int quoteID)
        {
            User usr = _context.Quote.GetSingle(e => e.QuoteID == quoteID).User;
            Dictionary<string, string> dctUserEmailpassword = new Dictionary<string, string>();
            if (usr != null)
            {
                string password = _context.webpages_Membership.GetSingle(e => e.UserId == usr.UserId).Password;
                dctUserEmailpassword.Add("Email", usr.Email);
                dctUserEmailpassword.Add("Password", password);
                dctUserEmailpassword.Add("UserID", usr.UserId.ToString());
            }
            return dctUserEmailpassword;
        }


        public bool GetUserStatusbyPersonID(int personID)
        {
            User user = _context.User.GetSingle(e => e.PersonID == personID);
            //if user does not exits for that personID
            if (user == null)
            {
                user = InsertUpdateUser(UserVM);
            }

            if (user != null)
            {
                return (bool)user.ISActive;
            }
            return false;
        }

        public bool PersonIDExists(string email)
        {
            User user = GetUserDetailsByEmail(email);
            return user!=null ? user.PersonID != null ? true: false:false;
        }

    }
}

