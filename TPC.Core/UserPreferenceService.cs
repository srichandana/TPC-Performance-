using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Context;
using TPC.Context.Interfaces;
using TPC.Core.Mapping;
using AutoMapper;
using TPC.Context.EntityModel;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using TPC.Common.Enumerations;

namespace TPC.Core
{
    public class UserPreferenceService : ServiceBase<IUserPreferenceModel>, IUserPreferenceService
    {
      
        public bool SaveUserPreferences(string type, string preferred)
        {
            int loggedInUserID = UserVM.CRMModelProperties.LoggedINUserID;
            UserPreference userPreferencesNew = new UserPreference();
            UserPreference userPreferencesOld = _Context.UserPreference.GetSingle(e => e.UserID == loggedInUserID && e.PreferredType == type);

            userPreferencesNew.UserID = loggedInUserID;
            userPreferencesNew.PreferredType = type;
            userPreferencesNew.Preferred = preferred;
            userPreferencesNew.CreatedDate = System.DateTime.Now;
            userPreferencesNew.UpdatedDate = System.DateTime.Now;
            if (userPreferencesOld != null)
            {
                userPreferencesOld.UserID = loggedInUserID;
                userPreferencesOld.PreferredType = type;
                userPreferencesOld.Preferred = preferred;
                userPreferencesOld.CreatedDate = System.DateTime.Now;
                userPreferencesOld.UpdatedDate = System.DateTime.Now;
                
            }
            else
            {
                _Context.UserPreference.Add(userPreferencesNew);
            }

            _Context.UserPreference.SaveChanges();
            return true;
        }

    }
}
