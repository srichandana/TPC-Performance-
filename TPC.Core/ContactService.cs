using System;
using TPC.Core.Models.ViewModels;
using TPC.Core.Interfaces;
using TPC.Context.Interfaces;
using TPC.Context;
using System.Configuration;
using TPC.Context.EntityModel;
using System.Collections.Generic;
using System.Linq;

namespace TPC.Core
{

    public class ContactService : ServiceBase<IContactModel>, IContactService
    {

        public ContactViewModel getPenworthyContactDetails(int userID)
        {
            ContactViewModel contactVM = new ContactViewModel();
            QuoteViewService qvs = new QuoteViewService();
            RepUser userProfile = new RepUser();
            User repuser = new User();
            string dir = ConfigurationManager.AppSettings["ContactusImageDirectory"];

            User crtuserdetails = _Context.User.GetSingle(e => e.UserId == userID);
            if (UserVM != null)
            {
                if (UserVM.CRMModelProperties != null && !UserVM.CRMModelProperties.IsRepLoggedIN)
                {
                   // repuser = _Context.User.GetSingle(e => e.CustAutoID == crtuserdetails.CustAutoID);
                    repuser = crtuserdetails.Customer.CustomerRep.RepUser.User;
                }
                else
                {
                    repuser = _Context.User.GetSingle(e => e.UserId == userID);
                }
            }
            else
            {
                repuser = _Context.User.GetSingle(e => e.UserId == userID);
            }
            //int repID = repuser.RepID == null ? repuser.Customer.CustomerRep.RepID :(int) repuser.RepID;
            //int repUserId = _Context.User.GetSingle(e => e.RepID == repID).UserId;
            //userProfile = _Context.RepUser.GetSingle(e => e.RepID == UserVM.CRMModelProperties.RepID);
            if (repuser != null)
            {
                contactVM.RepName = repuser.FirstName + " " + repuser.LastName;
                contactVM.RepEmail = repuser.Email;
                contactVM.PhoneCustomerService = repuser.RepUsers.FirstOrDefault().PhoneCustomerService;
                contactVM.PhoneDirect = repuser.RepUsers.FirstOrDefault().PhoneDirect;
                contactVM.AboutRep = repuser.RepUsers.FirstOrDefault().Biodata;
                contactVM.RepImage = dir + repuser.RepUsers.FirstOrDefault().RepID + ".jpg";
                contactVM.Title = repuser.RepUsers.FirstOrDefault().Title;
            }
            contactVM.UserVM = UserVM;
            qvs.UserVM = UserVM;
            contactVM.UserVM.CurrentQuoteID = qvs.getCustomerSCQuoteID();
            return contactVM;
        }


        public void EditProfile(ContactViewModel contactModel)
        {
            //int RepId = Convert.ToInt32(UserViewModel.RepresentativeInfoObj.UserID);
            //int repUserId = UserVM.CRMModelProperties.RepID;
            RepUser userProfile = _Context.RepUser.GetSingle(e => e.RepID == UserVM.CRMModelProperties.RepID);
            if (userProfile != null)
            {
                userProfile.Biodata = contactModel.AboutRep;
                userProfile.PhoneCustomerService = contactModel.PhoneCustomerService;
                // userProfile.UserId = repUserId;
                _Context.RepUser.SaveChanges();
            }
        }
    }
}
