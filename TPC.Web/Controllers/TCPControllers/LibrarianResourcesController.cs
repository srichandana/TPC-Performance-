using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core;
using TPC.Core.Interfaces;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers
{

    [AllowAnonymous]
    public class LibrarianResourcesController : BaseController
    {
        private readonly ILibrarianResourcesService _librarianresources;
        public LibrarianResourcesController(LibrarianResourcesService librarianresources)
        {
            _librarianresources = librarianresources;

            _librarianresources.UserVM = UserVM;
        }

        public ActionResult Librarian()
        {
            _librarianresources.UserVM = UserVM;
            return View("../TCPViews/LibrarianResources", _librarianresources.GetLibraryResource());
        }

        //  [HttpPost]
        public FileResult pdfLoad(string path)
        {
            string pdfpath = path.Replace(".jpg", ".pdf");
            Stream fReadstreal = System.IO.File.OpenRead(pdfpath);
            byte[] bte = new byte[fReadstreal.Length];
            int ofst = 0;
            fReadstreal.Read(bte, ofst, bte.Length);
            fReadstreal.Close();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BinaryWrite(bte);
            Response.AddHeader("Content-Disposition", "inline;filename=" + "Libarary.pdf");
            Response.ContentType = "application/pdf";
            Response.Flush();
            Response.Clear();
            Response.End();
            return null;
        }

        //public ActionResult GetImage(string imgPath)
        //{
        //    return File(imgPath, "image/jpeg");
        //}

        public ActionResult GetPdf(string pdfPath)
        {
            return File(pdfPath, "application/pdf");
        }
    }
}
