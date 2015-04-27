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
using TPC.Core.Models.Models;
using TPC.Common.Enumerations;
using TPC.Core.Models.ViewModels;
using System.Configuration;
using System.Text;
using TPC.Web.Infrastructure;
using System.IO;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;


namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]

    public class ShoppingCartController : BaseController
    {
        private readonly IShoppingCartService _shoppingCartSrv;
        public IActiveQuoteService _activeQuoteSrv;
        private readonly HtmlViewRenderer _htmlViewRenderer = new HtmlViewRenderer();
        private readonly StandardPdfRenderer _standardPdfRenderer = new StandardPdfRenderer();
        public EmailService _emailservice = new EmailService();

        public ShoppingCartController(ShoppingCartService shoppingCartSrv)
        {
            _shoppingCartSrv = shoppingCartSrv;
            _activeQuoteSrv = new ActiveQuoteService();
            _shoppingCartSrv.UserVM = UserVM;
            _activeQuoteSrv.UserVM = UserVM;
        }

        public ActionResult ShoppingCartByClientID(int custUserID)
        {
            UserVM.SearchCategory = Resources.TCPResources.ShoppingCartText1;
            ViewData["QuoteType"] = Resources.TCPResources.ShoppingCartliText;
            _shoppingCartSrv.UserVM = UserVM;
            _activeQuoteSrv.UserVM = UserVM;
            ViewBag.CatalogStatus = _activeQuoteSrv.CheckCatalogInfoforCustomer(UserVM.CRMModelProperties.CustAutoID);//Used to display catalog information for Customer
            ViewBag.CatalogStatus = ViewBag.CatalogStatus == Resources.TCPResources.InComplete ? Resources.TCPResources.CatIncompleteText : Resources.TCPResources.CatCompleteText;
            ShoppingCartViewModel scVM = _shoppingCartSrv.GetShoppingCartViewByClientID(custUserID);

            this.AssignUserVM(scVM.UserVM);
            return View("../TCPViews/ShoppingCart", scVM);
        }



        //public ActionResult DWByClientID()
        //{
        //    ViewData["SearchCategory"] = "DecisionWizard";
        //    return View("../TCPViews/RepoDecisionWizard", _shoppingCartSrv.GetDWByClientID(UserViewModel.CustomerAccountInfoObj.UserID));
        //}


        // Changing price based on quantity
        [HttpPost]
        public JsonResult QuantityPrice(int currentQuoteId, int quantity, string itemID,string type="")
        {
            ViewData["QuoteType"] = Resources.TCPResources.ShoppingCartliText;
            _shoppingCartSrv.UserVM = UserVM;
            this.AssignUserVM(_shoppingCartSrv.UserVM);
            return Json(_shoppingCartSrv.UpdateQuantity(currentQuoteId, quantity, itemID,type));
        }

        [HttpPost]
        public JsonResult UpdateQuantityByQuoteId(int currentQuoteId, int quantity)
        {
            ViewData["QuoteType"] = new QuoteViewService().getQuoteTypeText(currentQuoteId);
            _shoppingCartSrv.UserVM = UserVM;

            this.AssignUserVM(_shoppingCartSrv.UserVM);
            return Json(_shoppingCartSrv.UpdateQuantityByQuoteId(currentQuoteId, quantity));
        }

        [HttpPost]
        public int PlaceOrder(int quoteID, string POText, string comments)
        {
            _shoppingCartSrv.UserVM = UserVM;
            UserVM.SearchCategory = Resources.TCPResources.ShoppingCartliText;
            this.AssignUserVM(_shoppingCartSrv.UserVM);
            int orderedQuoteId = _shoppingCartSrv.CreateOrder(quoteID, POText, comments);
            return orderedQuoteId;
        }

        [HttpPost]
        public List<string> DeleteItem(int quoteID, string item, int QuoteTypeID = 0)
        {
            _activeQuoteSrv.UserVM = UserVM;
            _shoppingCartSrv.UserVM = UserVM;
            List<string> lstSCCountPrice = _shoppingCartSrv.DeleteItem(quoteID, item, QuoteTypeID);
            _shoppingCartSrv.UserVM.SCCount = Convert.ToInt32(lstSCCountPrice[0]);
            _shoppingCartSrv.UserVM.SCPrice = Convert.ToDecimal(lstSCCountPrice[1]);
            ViewData["SearchCategory"] = Resources.TCPResources.ShoppingCartText1;
            this.AssignUserVM(_shoppingCartSrv.UserVM);
            _activeQuoteSrv.CheckCataalogInfo(UserVM.CRMModelProperties.CustAutoID);//Used to display catalog information for Customer
            return lstSCCountPrice;
        }

        [HttpPost]
        public JsonResult GetlstScDetailsbyQuoteID(int QuoteId,string type="")
        {
            _shoppingCartSrv.UserVM = UserVM;
            List<string> shoppingCartdata = _shoppingCartSrv.GetlstScDetailsbyQuoteID(QuoteId,type);
            this.AssignUserVM(_shoppingCartSrv.UserVM);
            return Json(shoppingCartdata);
        }

        public ActionResult CartDWPdfGeneration(int cartDWID)
        {
            var pdf = new RazorPDF.PdfResult();
            _shoppingCartSrv.UserVM = UserVM;
            CartDWPdfModel orderVM = _shoppingCartSrv.GetQuotePdfDetails(cartDWID);
            pdf = new RazorPDF.PdfResult(orderVM, "../TCPViews/DWCartPdfGeneration");
            pdf.ViewBag.Title = "Attachment";
            //CartDWPdfGenerationforEmail(cartDWID);
            return pdf;
        }

        public void CartDWPdfGenerationforEmail(int cartDWID)
        {
            // Render the view html to a string.
            _shoppingCartSrv.UserVM = UserVM;
            CartDWPdfModel orderVM = _shoppingCartSrv.GetQuotePdfDetails(cartDWID);
            string htmlText = (this._htmlViewRenderer.RenderViewToString(this, "../TCPViews/DWCartPdfGenerationWithHTML", orderVM));
            Document document = new Document(PageSize.A4, 40f, 30f, 20f, 30f);
            string fname = cartDWID + ".pdf";
            PdfWriter.GetInstance(document, new FileStream(ConfigurationManager.AppSettings["PdfPath"] + fname, FileMode.Create));
            document.Open();
            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(Server.MapPath("~/Images/PenWorthyLogo.jpg"));
            pdfImage.ScaleToFit(150, 70);
            pdfImage.Alignment = iTextSharp.text.Image.UNDERLYING; pdfImage.SetAbsolutePosition(200, 600);
            document.Add(pdfImage);
            HTMLWorker hw = new HTMLWorker(document);
            hw.Parse(new StringReader(htmlText));
            document.Close();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Disposition", "inline;filename=" + fname);
            Response.ContentType = "application/pdf";
            Response.WriteFile(ConfigurationManager.AppSettings["PdfPath"] + fname);
            Response.Flush();
            Response.Clear();
            string toEmail = UserVM != null ? UserVM.CRMModelProperties.CustEmail : string.Empty;
            List<string> FilePath = Directory.GetFiles(ConfigurationManager.AppSettings["PdfPath"], fname).ToList();
            _emailservice.SendMail("Pdf Attachment", "Thank you for your Order", true, true, FilePath, null, null, toEmail, null, cartDWID);
            // _emailservice.SendTestMail("Pdf Attachment", "Thank you for your Order", true, true, null, null, null, toEmail, null, cartDWID, null, 0, null);
            Response.End();
        }

        public void Export(int QuoteID)
        {
            IQuoteViewService _quoteviewservice = new QuoteViewService();
            int Quotetypeid = _quoteviewservice.getQuoteTypeId(QuoteID);
            string userSelection = null;
            List<CartDWPdfModel> orderVM = new List<CartDWPdfModel>();
            _shoppingCartSrv.UserVM = UserVM;
            orderVM.Add(_shoppingCartSrv.GetQuotePdfDetails(QuoteID));
            int shpId = Convert.ToInt32(TPC.Common.Enumerations.QuoteTypeEnum.ShoppingCart);
            int DWId = Convert.ToInt32(TPC.Common.Enumerations.QuoteTypeEnum.DecisionWhizard);
            if (Quotetypeid == shpId)
            {
                userSelection = Resources.TCPResources.ShoppingCartuserSelection;
            }
            if (Quotetypeid == DWId)
            {
                userSelection = Resources.TCPResources.DWuserSelection;
            }
            if (Quotetypeid == Convert.ToInt32(TPC.Common.Enumerations.QuoteTypeEnum.Direct) || Quotetypeid == Convert.ToInt32(TPC.Common.Enumerations.QuoteTypeEnum.Preview) || Quotetypeid == Convert.ToInt32(TPC.Common.Enumerations.QuoteTypeEnum.Web))
            { userSelection = Resources.TCPResources.QuoteuserSelection; }
            string[] preferences = string.IsNullOrEmpty(userSelection) ? null : userSelection.Split(',');
            var Controller = DependencyResolver.Current.GetService<DefaultController>();
            System.Data.DataTable dt = new System.Data.DataTable();
            if (preferences != null)
            {
                dt = Controller.ConvertToDataTable(orderVM.FirstOrDefault().CartListView, preferences);
            }
            StringBuilder sb = new StringBuilder();
            string headerText = string.Format(Resources.TCPResources.QuoteEmailExcelHeaderTable, ConfigurationManager.AppSettings["imgLogoPath"], string.Format("{0:d}", DateTime.Today), UserVM.CurrentQuoteID, UserVM.CRMModelProperties.CustNO, UserVM.CRMModelProperties.RepName, UserVM.CRMModelProperties.custName, UserVM.CRMModelProperties.RepEmail, UserVM.CRMModelProperties.CustFirstName + ' ' + UserVM.CRMModelProperties.CustLastName);
            sb.Append(headerText);
            sb.Append("<table style=\"border-collapse:collapse;\">");
            sb.Append("<tr>");
            foreach (System.Data.DataColumn dc in dt.Columns)
            {
                if (dc.ToString().Trim() == "DwstatusID")
                { sb.Append("<td><b><font face=Arial size=2>" + "Sts" + "</font></b></td>"); }
                else if (dc.ToString().Trim() == "Price")
                { sb.Append("<td><b><font face=Arial size=2>" + "Total" + "</font></b></td>"); }
                else if (dc.ToString().ToLower().Trim() == "itemid")
                {
                    sb.Append("<td style='width:10px;text-align: center;'><font face=Arial size=" + "14px" + ">" + "ItemID" + "</font></td>");
                }
                else if (dc.ToString().ToLower().Trim() == "ar")
                {
                    sb.Append("<td style='width:10px;text-align: center;'><font face=Arial size=" + "14px" + ">" + "AR" + "</font></td>");
                }
                else if (dc.ToString().ToLower().Trim() == "lexile")
                {
                    sb.Append("<td style='width:10px;text-align: center;'><font face=Arial size=" + "14px" + ">" + "Lexile" + "</font></td>");
                }
                else if (dc.ColumnName.ToLower().Trim() != "type")
                    sb.Append("<td><b><font face=Arial size=2>" + dc.ColumnName + "</font></b></td>");
            }
            sb.Append("</tr>");
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (System.Data.DataColumn dc in dt.Columns)
                {

                    if (dc.ToString().Trim() == "DwstatusID")
                    {
                        string DWstatus = Convert.ToInt32(dr[dc]) == (int)DecisionWhizardStatusEnum.New ? DecisionWhizardStatusEnum.New.ToString() :
                            Convert.ToInt32(dr[dc]) == (int)DecisionWhizardStatusEnum.Yes ? DecisionWhizardStatusEnum.Yes.ToString() :
                             Convert.ToInt32(dr[dc]) == (int)DecisionWhizardStatusEnum.No ? DecisionWhizardStatusEnum.No.ToString() : DecisionWhizardStatusEnum.MayBe.ToString();
                        sb.Append("<td><font face=Arial size=" + "14px" + ">" + DWstatus + "</font></td>");
                    }
                    else if (dc.ToString().Trim() == "ISBN")
                    { sb.Append("<td><font face=Arial size=2>" + dr[dc] + "&nbsp;&nbsp;</font></td>"); }
                    else if (dc.ColumnName.ToLower().Trim() == "title" && dr["Type"] != "Catalog")
                    {
                        sb.Append("<td><b><font face=Arial size=" + "10px" + "><a href =" + ConfigurationManager.AppSettings["imgLogoPath"] + ">" + dr[dc] + "</a></font></b></td>");

                    }
                    else if (dc.ToString().ToLower().Trim() == "price")
                    {
                        sb.Append("<td>" + String.Format("{0:C}", dr[dc]) + "</td>");
                    }
                    else if (dc.ToString().ToLower().Trim() == "itemprice")
                    {
                        sb.Append("<td>" + String.Format("{0:C}", dr[dc]) + "</td>");
                    }
                    else if (dc.ToString().ToLower().Trim() == "itemid" || dc.ToString().ToLower().Trim() == "ar" || dc.ToString().ToLower().Trim() == "lexile")
                    {
                        sb.Append("<td style='width:10px;text-align: center;'><font face=Arial size=" + "14px" + ">" + dr[dc].ToString() + "</font></td>");
                    }
                    else if (dc.ColumnName.ToLower().Trim() != "type")
                    {
                        sb.Append("<td><font face=Arial size=" + "14px" + ">" + dr[dc].ToString() + "</font></td>");
                    }
                }
                sb.Append("</tr>");

            }
            double totalPrice = orderVM.FirstOrDefault().CartListView.Sum(e => e.Price);
            double SalesTax = 0;
            if (orderVM.FirstOrDefault().SalesTax > 0)
            {
                SalesTax = (totalPrice * (double)orderVM.FirstOrDefault().SalesTax);
            }
            string footerText = string.Format(Resources.TCPResources.QuoteEmailExcelFooter, preferences.Length - 2, String.Format("{0:C}", SalesTax), String.Format("{0:C}", SalesTax + totalPrice));
            sb.Append(footerText);

            sb.Append("</table>");
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());

            Response.Clear();
            Response.ClearHeaders();
            Response.ClearContent();
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment; filename=download.xls");
            Response.BinaryWrite(buffer);
            Response.End();
            Response.Close();
        }
    }
}
