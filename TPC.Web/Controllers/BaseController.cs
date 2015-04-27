using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using TPC.Core;
using TPC.Core.Models.ViewModels;
using WebMatrix.WebData;

namespace TPC.Web.Controllers
{
    public class BaseController : Controller
    {
        public UserViewModel UserVM
        {
            get
            {
                if (HttpContext != null)
                {
                    return (UserViewModel)HttpContext.Session["userVM"];
                }
                else
                {

                    return null;
                }
            }
            set
            {
                HttpContext.Session["userVM"] = value;
            }
        }

        public void AssignUserVM(UserViewModel updateduVM)
        {
            UserVM = updateduVM;
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {
                string actionName = filterContext.RouteData.Values["action"].ToString();
                Type controllerType = filterContext.Controller.GetType();
                var method = controllerType.GetMethod(actionName);
                var returnType = method.ReturnType;
                if (returnType.Equals(typeof(ActionResult))
                || (returnType).IsSubclassOf(typeof(ActionResult)))
                {
                    filterContext.Result = new ViewResult
                    {
                        ViewName = "~/Views/Shared/Error.cshtml"
                    };
                }
            }
            string conn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            Elmah.SqlErrorLog ErrorLog = new Elmah.SqlErrorLog(conn);
            ErrorLog.Log(new Elmah.Error(new Exception(filterContext.Exception.ToString())));
            filterContext.ExceptionHandled = true;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //if (filterContext.HttpContext.Request.IsAuthenticated && filterContext.HttpContext.Session != null)
            //{
            //    if (filterContext.HttpContext.Session.IsNewSession && filterContext.HttpContext.Session["userVM"] == null)
            //    {
            //        string cookie = filterContext.HttpContext.Request.Headers["Cookie"];
            //        if ((cookie != null) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
            //        {
            //            if (string.IsNullOrEmpty(Request.QueryString["DCDWLID"]))
            //            {
            //                WebSecurity.Logout();
            //                filterContext.Result = new RedirectResult("~/Default/GetProductDetails?quoteID=0&quoteType =Products");
            //            }
            //            return;
            //        }
            //    }
            //}
            //base.OnActionExecuting(filterContext);
            
            if (filterContext.HttpContext.Request.IsAuthenticated && filterContext.HttpContext.Session.IsNewSession)
            {
                string cookie = filterContext.HttpContext.Request.Headers["Cookie"];
                if ((cookie != null) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    if (string.IsNullOrEmpty(Request.QueryString["PersID"]) || string.IsNullOrEmpty(Request.QueryString["DCDWLID"]))
                    {
                        WebSecurity.Logout();
                        filterContext.Result = new RedirectResult("~/Default/GetProductDetails?quoteID=0&quoteType =Products");
                    }
                    return;
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}
