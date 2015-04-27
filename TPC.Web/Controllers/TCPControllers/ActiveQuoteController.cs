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
using TPC.Common.Enumerations;
using TPC.Core.Infrastructure;
using TPC.Common.Constants;
using System.Configuration;

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]
    public class ActiveQuoteController : BaseController
    {
        private readonly IActiveQuoteService _activeQuoteSrv;

        public ActiveQuoteController(ActiveQuoteService activeQuoteSrv)
        {
            _activeQuoteSrv = activeQuoteSrv;
            _activeQuoteSrv.UserVM = UserVM;

        }

        //
        // GET: /ActiveQuote/

        public ActionResult ActiveQuote()
        {
            _activeQuoteSrv.UserVM = UserVM;
            ActiveQuoteViewModel acVM = _activeQuoteSrv.GetActiveQuoteVM();
            this.AssignUserVM(acVM.UserVM);
            return View("../TCPViews/ActiveQuote", acVM);
        }


        [HttpPost]
        public ActionResult CreateQuote(string quoteText, int quotetypeID)
        {
            _activeQuoteSrv.UserVM = UserVM;
            QuoteModel quoteModel = new QuoteModel();
            quoteModel.CreatedDate = DateTime.UtcNow;
            quoteModel.QuoteTypeID = quotetypeID;
            quoteModel.QuoteText = quoteText;
            quoteModel.TotalItems = 0;
            quoteModel.TotalPrice = 0;
            quoteModel.UpdateDate = DateTime.UtcNow;
            quoteModel.StatusID = 1;
            quoteModel.CustUserID = UserVM.CRMModelProperties.LoggedINCustomerUserID;
            quoteModel.QuoteTypeText = _activeQuoteSrv.GetQuoteTypeTextByQuoteTypeID(quotetypeID);
            quoteModel.ARDivisionNo = UserVM.CRMModelProperties.DivNO;
            _activeQuoteSrv.CreateQuote(quoteModel);
            //return View("../TCPViews/ActiveQuote", _activeQuoteSrv.GetActiveQuoteVM());
            return Json(quoteModel);
        }

        [HttpPost]
        public JsonResult CreateDW(string dwName, int customerID)
        {
            _activeQuoteSrv.UserVM = UserVM;
            QuoteModel quoteModel = new QuoteModel();
            quoteModel.CreatedDate = DateTime.UtcNow;
            quoteModel.QuoteTypeID = (int)QuoteTypeEnum.DecisionWhizard;
            quoteModel.QuoteText = dwName;
            quoteModel.TotalItems = 0;
            quoteModel.TotalPrice = 0;
            quoteModel.UpdateDate = DateTime.UtcNow;
            quoteModel.StatusID = 1;
            quoteModel.CustUserID = customerID;
            quoteModel.ARDivisionNo = UserVM.CRMModelProperties.DivNO;
            _activeQuoteSrv.CreateQuote(quoteModel);
            return Json(quoteModel);
            //return View("../TCPViews/ActiveQuote", _activeQuoteSrv.GetActiveQuoteVM());
        }

        [HttpPost]
        public ActionResult CreateQuotePartial()
        {
            _activeQuoteSrv.UserVM = UserVM;
            CreateQuoteViewModel createQVM = _activeQuoteSrv.GetQuoteTypes();
            this.AssignUserVM(createQVM.UserVM);
            return PartialView("../TCPViews/Partial/Quote/CreateQuotePartial", createQVM);
        }

        [HttpPost]
        public ActionResult CreateDWPartial()
        {
            _activeQuoteSrv.UserVM = UserVM;
            CreateDecisionWizardViewModel dwVM = _activeQuoteSrv.GetDWInfo();
            this.AssignUserVM(dwVM.UserVM);

            return PartialView("../TCPViews/Partial/CreateDecisionWiZardPartial", dwVM);
        }

        [HttpPost]
        public ActionResult SubmitQuotePartialView(string quoteID)
        {
            _activeQuoteSrv.UserVM = UserVM;
            SubmitQuoteViewModel submitQVM = _activeQuoteSrv.GetUserAddressInfo(Convert.ToInt32(quoteID));
            this.AssignUserVM(submitQVM.UserVM);

            return PartialView("../TCPViews/Partial/Quote/SubmitQuotePartial", submitQVM);
        }

        [HttpPost]
        public bool DeleteQuote(int quoteID)
        {
            _activeQuoteSrv.UserVM = UserVM;
            QuoteModel qouteModel = new QuoteModel();
            qouteModel.QuoteID = quoteID;
            _activeQuoteSrv.DeleteQuote(qouteModel);
            return true;
        }


        [HttpPost]
        public JsonResult AddMergeQuotes(string quoteIDs, int customerID, string quoteType)
        {
            _activeQuoteSrv.UserVM = UserVM;
            List<int?> lstQuoteIds = quoteIDs.Split(',').Select(e => (int?)Convert.ToInt32(e)).ToList();
            QuoteModel quoteModel = new QuoteModel();
            quoteModel.CreatedDate = DateTime.UtcNow;
            quoteModel.QuoteTypeID = Convert.ToInt32(quoteType);
            quoteModel.QuoteText = "Merged Quote-" + (quoteIDs.Length > 25 ? quoteIDs.Substring(0, 25) : quoteIDs);
            quoteModel.UpdateDate = DateTime.UtcNow;
            quoteModel.StatusID = 1;
            quoteModel.CustUserID = customerID;
            quoteModel.ARDivisionNo = UserVM.CRMModelProperties.DivNO;
            _activeQuoteSrv.CreateQuote(quoteModel);
            QuoteModel mergeQuoteInfo = _activeQuoteSrv.InsertMergeQuoteDetails(lstQuoteIds, customerID, quoteModel.QuoteID);
            quoteModel.UserVM = UserVM;
            return Json(mergeQuoteInfo);
        }
        [HttpPost]
        public string UpdateRecentContactedByQuoteIDs(string quoteIDs, string selectedTemplate, string repComment)
        {
            _activeQuoteSrv.UserVM = UserVM;
            return _activeQuoteSrv.UpdateRecentContactedByQuoteIDs(quoteIDs, selectedTemplate, repComment);
        }

        [HttpPost]
        public void SaveData(string toAddress, int quoteId) 
        {
            EmailService emailsrvc = new EmailService();
            emailsrvc.UserVM=UserVM;
            emailsrvc.SaveMailHistory(ConfigurationManager.AppSettings["WareHouseEmail"], toAddress, "", "", quoteId, "Quote", UserVM.CRMModelProperties.CRMPersonID);
 
        }

        public ActionResult SubmitQuote(FormCollection submitQuote)
        {
            _activeQuoteSrv.UserVM = UserVM;

            SubmitQuoteViewModel sqvm = new SubmitQuoteViewModel();
            sqvm.AddInvRecipient = submitQuote.GetValues("ddlAddInvRecipent").ToList().FirstOrDefault();
            sqvm.BillingReference = submitQuote.GetValues("BillingReference").ToList().FirstOrDefault().Trim();
            sqvm.Comments = submitQuote.GetValues("Comments").ToList().FirstOrDefault().Trim();
            sqvm.DBValue = string.IsNullOrEmpty(submitQuote.GetValues("DBValue").ToList().FirstOrDefault()) ? 0 : Convert.ToDouble(submitQuote.GetValues("DBValue").ToList().FirstOrDefault().Replace("$", ""));
            sqvm.FutureBillingDate = string.IsNullOrEmpty(submitQuote.GetValues("FutureBillingDate").ToList().FirstOrDefault()) ? (DateTime?)null : Convert.ToDateTime(submitQuote.GetValues("FutureBillingDate").ToList().FirstOrDefault());
            sqvm.InvUserID = UserVM.CRMModelProperties.LoggedINCustomerUserID.ToString(); //submitQuote.GetValues("ddlInvoiceRecipient").ToList().FirstOrDefault();
            sqvm.LstSource = new List<ComboBase>();
            sqvm.LstSource.Add(new ComboBase() { ItemID = submitQuote.GetValues("ddlSource").ToList().FirstOrDefault(), Selected = true });
            sqvm.Payer = submitQuote["ddlPayer"] != null ? Convert.ToInt32(submitQuote.GetValues("ddlPayer").ToList().FirstOrDefault()) : 0;
            sqvm.PONo = submitQuote.GetValues("PONo").ToList().FirstOrDefault().Trim();
            sqvm.QuoteID = Convert.ToInt32(submitQuote.GetValues("QuoteID").ToList().FirstOrDefault());
            sqvm.ShipItemsTo = submitQuote["ddlShipItemsTo"] != null ? submitQuote.GetValues("ddlShipItemsTo").ToList().FirstOrDefault() : string.Empty;
            sqvm.Type = submitQuote.GetValues("ddlType").ToList().FirstOrDefault();
           // sqvm.InvoiceRecipient = submitQuote["ddlInvoiceRecipient"] != null ? submitQuote.GetValues("ddlInvoiceRecipient").ToList().FirstOrDefault() : string.Empty;
            sqvm.Division = submitQuote.GetValues("Division").ToList().FirstOrDefault().ToString();
            sqvm.ValidationStatus = new Dictionary<string, bool>();
            sqvm.ValidationStatus.Add(QuoteValidationConstants.Hold_Representative, submitQuote.GetValues("RepoHoldStatus").ToList().FirstOrDefault() == "true" ? true : false);
            sqvm.SourceType = submitQuote["ddlSource"] != null ? Convert.ToInt32(submitQuote.GetValues("ddlSource").ToList().FirstOrDefault()) : 0;
            string saveOrSubmit = submitQuote.GetValues("SubmitOrSave").ToList().FirstOrDefault() != null ? submitQuote.GetValues("SubmitOrSave").ToList().FirstOrDefault() : string.Empty;

            ActiveQuoteViewModel acVM = _activeQuoteSrv.SubmitQuote(sqvm, saveOrSubmit);
            return RedirectToAction("ActiveQuote", "ActiveQuote");
        }
        [HttpPost]
        public JsonResult RefreshValidationStatus(int quoteID)
        {
            _activeQuoteSrv.UserVM = UserVM;
            Dictionary<string, bool> dictQValidationFlagValues = _activeQuoteSrv.ValidateQuote(quoteID);
            return Json(dictQValidationFlagValues);
        }
        [HttpPost]
        public JsonResult UpdateRepoHoldStatus(int quoteID, bool isRepoHold)
        {
            _activeQuoteSrv.UserVM = UserVM;
            Dictionary<string, bool> dictQValidationFlagValues = _activeQuoteSrv.UpdateRepoHoldStatus(quoteID, isRepoHold);
            return Json(dictQValidationFlagValues);
        }

        public ActionResult GeneratePDF(int quoteID)
        {
            _activeQuoteSrv.UserVM = UserVM;
            OrderViewModel orderVM = _activeQuoteSrv.GeneratePDF(quoteID);

            var pdf = new RazorPDF.PdfResult(orderVM, "../TCPViews/GeneratePDF");
            pdf.ViewBag.Title = "Report Title";
            return pdf;
        }
        [HttpPost]
        public ActionResult CalTagPartialView(int quoteId)
        {
            _activeQuoteSrv.UserVM = UserVM;
            CalTagViewModel calTagVM = _activeQuoteSrv.GetCalTagInfo(quoteId);
            calTagVM.QuoteID = quoteId;
            return PartialView("../TCPViews/Partial/Quote/CallTagPartial", calTagVM);
        }
        [HttpPost]
        public ActionResult CalTagInfo(FormCollection calTagCollection)
        {
            CalTagViewModel calTagVM = new CalTagViewModel();
            calTagVM.QuoteID = Convert.ToInt32(calTagCollection.GetValues("quoteId").ToList().FirstOrDefault());
            calTagVM.LstSendOptions = new List<ComboBase>();
            string optID = calTagCollection.GetValues("LstSendOptions").ToList().FirstOrDefault() != null ? calTagCollection.GetValues("LstSendOptions").ToList().FirstOrDefault() : "";
            calTagVM.LstSendOptions.Add(new ComboBase { ItemID = optID });
            calTagVM.Email = calTagCollection.GetValues("txtEmail") != null ? calTagCollection.GetValues("txtEmail").ToList().FirstOrDefault() : "";
            calTagVM.FutureDate = calTagCollection.GetValues("FutureDate").ToList().FirstOrDefault() != null ? !string.IsNullOrEmpty(calTagCollection.GetValues("FutureDate").ToList().FirstOrDefault().ToString()) ? Convert.ToDateTime(calTagCollection.GetValues("FutureDate").ToList().FirstOrDefault()) : DateTime.UtcNow : DateTime.UtcNow;
            calTagVM.IsCataloging = calTagCollection.GetValues("IsCataloging").ToList().FirstOrDefault() != null ? string.IsNullOrEmpty(calTagCollection.GetValues("IsCataloging").ToList().FirstOrDefault()) ? Convert.ToBoolean(calTagCollection.GetValues("IsCataloging").ToList().FirstOrDefault()) : false : false;
            calTagVM.PONo = calTagCollection.GetValues("txtPoNo").ToList().FirstOrDefault() != null ? calTagCollection.GetValues("txtPoNo").ToList().FirstOrDefault() : "";
            calTagVM.InvoiceTo = calTagCollection.GetValues("ddlInvoiceTo").ToList().FirstOrDefault() != null ? calTagCollection.GetValues("ddlInvoiceTo").ToList().FirstOrDefault() : "";
            _activeQuoteSrv.UserVM = UserVM;
            _activeQuoteSrv.InsertCalTagInfo(calTagVM);
            return RedirectToAction("ActiveQuote", "ActiveQuote");
        }

        [HttpPost]
        public ActionResult RapidEntryPartial(int quoteID)
        {
            return PartialView("../TCPViews/Partial/RapidEntryPartial",_activeQuoteSrv.getRapidEntry(quoteID));
        }


    }
}
