using System.Web;
using System.Web.Http.Filters;
using System.Web.Mvc;
using TPC.Web.Filters;

namespace TPC.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new AuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
            
        }

        public static void RegisterHttpFilters(HttpFilterCollection filters)
        {
            filters.Add(new ElmahErrorAttribute());
        }
    }
}