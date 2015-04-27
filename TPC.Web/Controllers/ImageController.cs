using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;

namespace TPC.Web.Controllers
{
    public class ImageController : Controller
    {
        [AllowAnonymous]
        public ActionResult GetImage(string Id)
        {
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();

            string imagePath = ConfigurationManager.AppSettings["CommonRepository"];
            if (!Id.Contains(imagePath))
                imagePath = imagePath + Id;
            else
                imagePath = Id;
            if (!System.IO.File.Exists(imagePath))
            {
                string ext = imagePath.Substring(imagePath.LastIndexOf(".") + 1);
                imagePath = imagePath.Substring(0, imagePath.LastIndexOf("."));
                imagePath = imagePath + ".jpeg";
            }

            return File(imagePath, "image/jpeg");
        }

        [AllowAnonymous]
        public ActionResult GetPdf(string path)
        {
            HttpContext.Response.Cache.SetCacheability(HttpCacheability.Public);
            HttpContext.Response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            HttpContext.Response.Cache.SetLastModifiedFromFileDependencies();
            
            if (!System.IO.File.Exists(path))
            {
                path=path.Replace("_DUP","");
            }
            return File(path, "application/pdf");
        }
    }
}
