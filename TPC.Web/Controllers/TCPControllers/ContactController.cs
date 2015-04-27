using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core.Models;
using TPC.Web.Filters;
using TPC.Core;
using TPC.Core.Interfaces;
using Microsoft.Practices.Unity;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using System.Reflection;
using System.Configuration;
using System.IO;
using WebMatrix.WebData;

namespace TPC.Web.Controllers.TCPControllers
{

    public class ContactController : BaseController
    {
        //
        // GET: /Contact/
        private readonly IContactService _contactSrv;

        public ContactController(ContactService contactSrv)
        {
            _contactSrv = contactSrv;
            _contactSrv.UserVM = UserVM;
        }

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult ContactUs()
        {
            _contactSrv.UserVM = UserVM;
            return View("../TCPViews/ContactPenworthy", _contactSrv.getPenworthyContactDetails(Convert.ToInt32(WebSecurity.CurrentUserId)));
        }

        public ActionResult GetImage(string imgPath)
        {
            return File(imgPath, "image/jpeg");
        }


        public ActionResult EditProfile(FormCollection values)
        {
            ContactViewModel cVM = new ContactViewModel();

            cVM.AboutRep = values["AboutRep"];
            cVM.PhoneCustomerService = values["Phone"];

            foreach (string upload in Request.Files)
            {
                // if (Request.Files[upload].FileName==string.Empty) continue;
                string path = ConfigurationManager.AppSettings["ContactusImageDirectory"];
                string filename1 = Path.GetFileName(Request.Files[upload].FileName);
                string filename = UserVM.CRMModelProperties.LoggedINUserID + ".jpg";
                Request.Files[upload].SaveAs(Path.Combine(path, filename));
            }

            _contactSrv.EditProfile(cVM);
            _contactSrv.UserVM = UserVM;
            return View("../TCPViews/ContactPenworthy", _contactSrv.getPenworthyContactDetails(WebSecurity.CurrentUserId));
        }


        //public ActionResult Upload()
        //{
        //    foreach (string upload in Request.Files)
        //    {
        //     // if (Request.Files[upload].FileName==string.Empty) continue;
        //        string path = ConfigurationManager.AppSettings["ContactusImageDirectory"];
        //        string filename1 = Path.GetFileName(Request.Files[upload].FileName);
        //        string filename = UserViewModel.RepresentativeInfoObj.ID + ".jpg";
        //        Request.Files[upload].SaveAs(Path.Combine(path, filename));
        //    }
        //  return  RedirectToAction("ContactUs");

        //}

    }
}
