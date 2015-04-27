using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TPC.Common.Enumerations;
using TPC.Core;
using TPC.Core.Models.ViewModels;
using WebMatrix.WebData;

namespace TPC.Web.Filters
{
    public class TPCAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
           if (WebSecurity.IsAuthenticated)
            {
                if (filterContext.HttpContext.Session["userVM"] != null)
                {
                    UserViewModel userVM = null;
                    userVM = (UserViewModel)filterContext.HttpContext.Session["userVM"];
                    if (!userVM.CRMModelProperties.IsReqPropFilled)
                    {
                        LoginService loginSrv = new LoginService();
                        userVM = loginSrv.FillUserVMByUserID(WebSecurity.CurrentUserId, userVM);
                        filterContext.HttpContext.Session["userVM"] = userVM;
                    }
                }
                else
                {
                    WebSecurity.Logout();
                   // filterContext.HttpContext.Response.Redirect("~/Default/GetProductDetails?quoteID=0&quoteType =Products");
                    return;
                }
            }
        }
    }
}