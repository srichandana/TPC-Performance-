using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;
using TPC.Common.Enumerations;
using WebMatrix.WebData;
using System.Web.Security;
using TPC.Core.Infrastructure;
using TPC.Web.Models;
using System.Security.Cryptography;
using System.Web.Helpers;
using System.Configuration;
using System.Text;

namespace TPC.Web.Controllers.TCPControllers
{
    [AllowAnonymous]
    public class DefaultController : BaseController
    {

        public ActionResult GetProductDetails(string quoteID, string quoteType)
        {
            if (WebSecurity.IsAuthenticated && UserVM!=null)
            {
                return RedirectToAction("GetProducts", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = "60", quoteID = 0 });
            }
            else
            {
                return RedirectToAction("GetSelectedCollectionItem", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = "60", quoteID = 0 });
            }

        }
        public ActionResult GetLibrarianResources()
        {

            if (WebSecurity.IsAuthenticated && UserVM != null)
            {
                return RedirectToAction("Librarian", "LibrarianResources");
            }
            else
            {
                ILibrarianResourcesService _librarianresources = new LibrarianResourcesService();
                return View("../TCPViews/LibrarianResources", _librarianresources.GetLibraryResource());
            }

        }
        public ActionResult GetFAQQuestions()
        {
            if (WebSecurity.IsAuthenticated && UserVM != null)
            {
                return RedirectToAction("FAQ", "FAQ");
            }
            else
            {
                IFAQService _faqsrv = new FAQService();
                return View("../TCPViews/FAQ", _faqsrv.GetDetails());
            }


        }
        public ActionResult GetContactDetails()
        {

            string returnUrl = Request.RawUrl;
            if (WebSecurity.IsAuthenticated && UserVM != null)
            {
                return RedirectToAction("ContactUs", "Contact");
            }
            else
            {
                if (returnUrl.Contains("Image"))
                {
                    return File(Request.QueryString["imgPath"], "image/jpeg");
                }
                else
                {
                    return View("../TCPViews/ContactPenworthy", new ContactViewModel());
                }
            }
        }

        //public JsonResult customerInfo(string email)
        //{
        //    LoginService loginsrv = new LoginService();
        //    List<ComboBase> custDetails = loginsrv.GetAllCustomersByUserEmail(email);
        //    return Json(custDetails);
        //}

        public ActionResult UnderConstruction()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
        public ActionResult UserPreValidations(string emailAndCustAutoID, string pass, string type = "")
        {
            if (HttpContext.Session["userVM"] != null)
                HttpContext.Session.Remove("userVM");
            WebSecurity.Logout();

            LoginService loginsrv = new LoginService();
            if (Request.IsAjaxRequest())
            {
                string[] emailAndIDs = emailAndCustAutoID.Split('~');
                List<ComboBase> custDetails = loginsrv.GetAllCustomersByUserEmail(emailAndIDs[0]);
                if (custDetails.Count == 0 || custDetails == null)
                {
                    return Json(UserPreValidationEnum.InvalidUserName.ToString());
                }
                if (custDetails.Count > 1 && emailAndIDs.Count() == 1)
                {
                    return Json(custDetails);
                }
                string hashedPassword = loginsrv.ValidateUser(emailAndCustAutoID);
                bool isDafultPassword = pass == ConfigurationManager.AppSettings["DefaultPassword"] ? true : false; //Crypto.VerifyHashedPassword(key, Resources.TCPResources.DafultPasswordText);
                if (!string.IsNullOrEmpty(pass))
                {
                    bool isValidUser = !string.IsNullOrEmpty(hashedPassword) && Crypto.VerifyHashedPassword(hashedPassword, pass);
                    if (isValidUser)
                    {
                        if (isDafultPassword)
                        {
                            string emailID = emailAndCustAutoID.Split('~')[0];
                            return View("../Account/Manage", new LoginModel() { UserEmail = emailID, IsManageAccount = true, OldPassword = pass });
                        }
                        else
                        {
                            return Json(UserPreValidationEnum.ValidUser.ToString());
                        }
                    }
                    else
                    {
                        //Update Password logic
                        if (!string.IsNullOrEmpty(hashedPassword) && Crypto.VerifyHashedPassword(hashedPassword, ConfigurationManager.AppSettings["DefaultPassword"]) && string.IsNullOrEmpty(type))
                        {
                            loginsrv.ChangePassword(emailAndCustAutoID, Crypto.HashPassword(pass));
                            return Json(UserPreValidationEnum.ValidUser.ToString());
                        }
                        else
                        {
                            if(loginsrv.PersonIDExists(emailAndCustAutoID))
                            return Json(UserPreValidationEnum.ChangePasswordFailed.ToString());
                            else
                            return Json(UserPreValidationEnum.InvalidUserName.ToString());
                        }
                    }
                }
                return Json(UserPreValidationEnum.ChangePasswordFailed.ToString());
            }
            else
            {
                string hashedPassword = loginsrv.ValidateUser(emailAndCustAutoID);
                bool isDafultPassword = pass == ConfigurationManager.AppSettings["DefaultPassword"] ? true : false; //Crypto.VerifyHashedPassword(key, Resources.TCPResources.DafultPasswordText);
                bool isValidUser = !string.IsNullOrEmpty(pass) && !string.IsNullOrEmpty(hashedPassword) && Crypto.VerifyHashedPassword(hashedPassword, pass);
                string[] email = emailAndCustAutoID.Split('~');
                string emailID = email[0];
                string custAutoID = emailAndCustAutoID.Count() > 1 ? email[1] : string.Empty;
                if (isValidUser)
                {
                    if (isDafultPassword)
                    {
                        List<ComboBase> custDetails = loginsrv.GetAllCustomersByUserEmail(emailAndCustAutoID);
                        if (custDetails.Count > 1)
                        {
                            ViewBag.lstUserList = custDetails;
                            return View("../TCPViews/Partial/ChangeLoginPassword", new LoginModel() { UserEmail = emailID, OldPassword = pass, CustAutoID = custAutoID });
                            //return RedirectToAction("GetSelectedCollectionItem", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = "60", quoteID = 0 });
                        }
                        return View("../TCPViews/Partial/ChangeLoginPassword", new LoginModel() { UserEmail = emailID, IsManageAccount = true, OldPassword = pass, CustAutoID = custAutoID });
                    }
                    else
                    {
                        return RedirectToAction("GetSelectedCollectionItem", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = "60", quoteID = 0 });
                    }
                }
                return RedirectToAction("GetSelectedCollectionItem", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = "60", quoteID = 0 });
            }

        }
        public ActionResult UserLogIn(string UID, string emailAndCustAutoID, string pass)
        {
            ViewBag.IsUrlLogin = true;
            return UserPreValidations(emailAndCustAutoID + "~" + UID, pass);
        }
        public ActionResult ForgotPasswordDetails(string emailAndCustAutoID, string type)
        {
            if (!string.IsNullOrEmpty(emailAndCustAutoID))
            {
                LoginService loginsrv = new LoginService();
                string[] emailAndIDs = emailAndCustAutoID.Split('~');
                List<ComboBase> custDetails = loginsrv.GetAllCustomersByUserEmail(emailAndIDs[0]);
                if (custDetails.Count == 0 || custDetails == null)
                {
                    return Json(UserPreValidationEnum.InvalidUserName.ToString());
                }
                if (custDetails.Count == 1 || (custDetails.Count > 1 && emailAndIDs.Count() == 2))
                {
                    if (type == "changepassword")
                    {
                        ViewBag.enableOldPassword = true;
                        string emailID = emailAndCustAutoID.Split('~')[0];
                        return View("../Account/Manage", new LoginModel() { UserEmail = emailID, IsManageAccount = true });
                    }
                    else
                    {
                        return View("../Account/ForgotPassWizard", new LoginModel());
                    }

                }
                else
                {
                    return Json(custDetails);
                }
            }
            // return View("../Account/ForgotPassWizard", new LoginModel());
            return Json(UserPreValidationEnum.InvalidUserName.ToString());
        }
        public ActionResult SendForgortPassword(string emailAndUserId)
        {
            if (string.IsNullOrEmpty(emailAndUserId))
            {
                return Json(UserPreValidationEnum.ChangePasswordFailed.ToString());
                // return View("../Account/ForgotPassWizard", new LoginModel());
            }
            LoginService loginsrv = new LoginService();
            string[] emailAndIDs = emailAndUserId.Split('~');
            string emailOnly = emailAndIDs[0];
            List<ComboBase> custDetails = loginsrv.GetAllCustomersByUserEmail(emailOnly);
            if (custDetails.Count > 0 && custDetails != null)
            {
                if (custDetails.Count() > 1 && emailAndIDs.Count() == 1)
                {
                    return Json(custDetails);
                }
                else
                {
                    string password = string.Empty;
                    if (emailAndIDs.Count() == 1)
                    {
                        password = custDetails.FirstOrDefault().ItemID + DateTime.Now.Minute + DateTime.Now.Second;
                    }
                    else
                    {
                        password = emailAndIDs[1] + DateTime.Now.Minute + DateTime.Now.Second;
                    }
                    string encryptedPassword = Crypto.HashPassword(password);
                    bool isPassUpdated = loginsrv.ChangePassword(emailAndUserId, encryptedPassword);
                    if (isPassUpdated)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("<p>Dear User,</p>");
                        sb.AppendLine("<p>We have processed your request for issuance of a password.</p>");
                        sb.AppendLine("<p>Your Email Id: " + emailOnly + "</p>");
                        sb.Append("<p>Your password is: " + password + "</p>");
                        sb.Append(". </p>");
                        sb.AppendLine();
                        EmailService mail = new EmailService();
                        mail.SendMail("Forgot Password", sb.ToString(), true, false, null, null, null, emailOnly, "FP");
                        return Json(UserPreValidationEnum.ValidUser.ToString());
                    }
                }
            }
            return Json(UserPreValidationEnum.ChangePasswordFailed.ToString());
        }
        public System.Data.DataTable ConvertToDataTable<T>(IList<T> data, string[] selection)
        {
            System.ComponentModel.PropertyDescriptorCollection properties =
               System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            System.Data.DataTable table = new System.Data.DataTable();
            //order arrangement
            foreach (string selectedName in selection)
            {
                System.ComponentModel.PropertyDescriptor prop = properties.Find(selectedName, true);
                if (prop != null)
                {
                    if ((!prop.PropertyType.IsGenericType && selection != null && selection.Contains(prop.Name.Trim())) || (selection != null && prop.PropertyType.IsGenericType && selection.Contains(prop.Name.Trim()) && (selection != null && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))))
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                    else if ((!prop.PropertyType.IsGenericType && selection == null) || (selection == null && prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }
            //foreach (System.ComponentModel.PropertyDescriptor prop in properties)
            //    if ((!prop.PropertyType.IsGenericType && selection != null && selection.Contains(prop.Name.Trim())) || (selection != null && prop.PropertyType.IsGenericType && selection.Contains(prop.Name.Trim()) && (selection != null && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))))
            //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            //    else if ((!prop.PropertyType.IsGenericType && selection == null) || (selection == null && prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
            //        table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                System.Data.DataRow row = table.NewRow();
                foreach (System.ComponentModel.PropertyDescriptor prop in properties)
                    if ((!prop.PropertyType.IsGenericType && selection != null && selection.Contains(prop.Name.Trim())) || (selection != null && prop.PropertyType.IsGenericType && selection.Contains(prop.Name.Trim()) && (selection != null && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                    else if ((!prop.PropertyType.IsGenericType && selection == null) || (selection == null && prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;

        }
    }
}
