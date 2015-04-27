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
using System.Reflection;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;
using WebMatrix.WebData;

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]
    public class UserPreferenceController : BaseController
    {
        //
        // GET: /UserPreference/
     
        private readonly IUserPreferenceService _iUserPreferenceSrv;

        public UserPreferenceController(UserPreferenceService userPreferencesrv)
        {
            _iUserPreferenceSrv = userPreferencesrv;
          
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult UserPreferredColumns(string Type, string userPreferred)
        {
            _iUserPreferenceSrv.UserVM = UserVM;
            _iUserPreferenceSrv.SaveUserPreferences(Type, userPreferred);
            return null;
        }

    }
}
