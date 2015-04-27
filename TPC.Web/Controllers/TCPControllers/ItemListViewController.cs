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
using System.Web.Security;
using System.Web.UI;
using WebMatrix.WebData;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Configuration;
using System.Text;
using TPC.Common.Enumerations;
using System.Diagnostics;
using TPC.Web.Infrastructure;

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]

    public class ItemListViewController : BaseController
    {
        //
        // GET: /ItemListView/

        public readonly IItemListViewService _iItemListViewService;

        public IItemService _itemservice;
        QuoteViewService _quoteViewService = new QuoteViewService();

        public ItemListViewController(ItemListViewService itemListViewService)
        {
            _iItemListViewService = itemListViewService;
            _itemservice = new ItemService();
            _itemservice.UserVM = UserVM;
        }

        public ActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        public ActionResult ViewSingleItem(string itemID, string QuoteID, string type = "")
        {
            _itemservice.UserVM = UserVM;
            if (WebSecurity.IsAuthenticated)
            {
                _quoteViewService.UserVM = UserVM;
                UserVM.CurrentQuoteID = QuoteID != "0" ? Convert.ToInt32(QuoteID) : _quoteViewService.getCustomerSCQuoteID();
            }
            ItemDetailedViewModel itDVM = _itemservice.GetItemByID(itemID, QuoteID, type);
            this.AssignUserVM(_itemservice.UserVM);
            return PartialView("../TCPViews/Partial/ItemDetailedPartial", itDVM);
        }
        //to view popup in singleItemDetailedView

        [HttpPost]
        public ActionResult ViewSingleItemByISBN(string ISBN, string QuoteID)
        {
            //string lstItemIds = _itemservice.GetItemIDsListByIsbn(ISBN, QuoteID);
            _itemservice.UserVM = UserVM;
            //  if (lstItemIds != null && lstItemIds != string.Empty)
            // {
            //     _itemservice.selectedOptions(lstItemIds, QuoteID, WebSecurity.CurrentUserId);
            //  }
            ItemDetailedViewModel itemDVM = _itemservice.GetItemByISBN(ISBN, QuoteID);
            //  itemDVM.UserVM = UserVM;
            //  this.AssignUserVM(_itemservice.UserVM);
            if (itemDVM.ItemID != null)
            {
                return PartialView("../TCPViews/Partial/ItemDetailedPartial", itemDVM);
            }
            else
            {
                return null;
            }
        }


        [HttpPost]
        public ActionResult ViewSingleDetailedParialView(string itemID, string QuoteID, string type = "")
        {
            _itemservice.UserVM = UserVM;
            ItemDetailedViewModel itemDVM = _itemservice.GetItemByID(itemID, QuoteID, type);
            itemDVM.UserVM = UserVM;
            return PartialView("../TCPViews/Partial/SingleItemDetailPartial", itemDVM);
        }
        //showing pdf and downloading excel
        [AllowAnonymous]
        public ActionResult ViewListOfItems()
        {

            int quoteID = Convert.ToInt32(Request.QueryString["quoteDWID"]);
            bool isPdf = Convert.ToBoolean(Request.QueryString["isPdf"]);
            bool invoicePdf = Convert.ToBoolean(Request.QueryString["invoicePdf"]);
            bool isExcel = Convert.ToBoolean(Request.QueryString["isExcel"]);

            if (isPdf || isExcel)
            {
                ActiveQuoteService _activeQuoteSrv = new ActiveQuoteService();
                _activeQuoteSrv.UserVM = UserVM != null ? UserVM : null;
                OrderViewModel orderVM = _activeQuoteSrv.GeneratePDF(quoteID);
                orderVM.InvoiceModel.FederalID = ConfigurationManager.AppSettings["FederalID"];
                var pdf = new RazorPDF.PdfResult(orderVM, "../TCPViews/GeneratePDF");
                if (isExcel)
                {
                    StringBuilder sb = new StringBuilder();
                    string headerText = string.Format(Resources.TCPResources.QuoteEmailExcelHeaderTable, ConfigurationManager.AppSettings["imgLogoPath"], string.Format("{0:d}", DateTime.Today), orderVM.QuoteID, orderVM.CustNumber, orderVM.RepoAddress.RepName, orderVM.CustomerName, orderVM.RepoAddress.RepEmail, orderVM.RepoAddress.CustFirstName + ' ' + orderVM.RepoAddress.CustLastName);
                    sb.Append(headerText);
                    sb.Append(ConvertToString(orderVM.CartListView));
                    double totalPrice = orderVM.CartListView.Sum(e => e.Price);
                    double taxPrice = 0;
                    if (orderVM.SalesTax > 0)
                    {
                        taxPrice = (totalPrice * (double)orderVM.SalesTax);
                    }
                    string footerText = string.Format(Resources.TCPResources.QuoteEmailExcelFooterTable, String.Format("{0:C}", taxPrice + totalPrice), String.Format("{0:C}", orderVM.ShippingCharge), String.Format("{0:C}", taxPrice + totalPrice));
                    sb.Append(footerText);
                    byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + orderVM.CustNumber + "_" + orderVM.QuoteID + ".xls");
                    Response.ContentType = "application//vnd.ms-excel";
                    Response.Buffer = true;
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.BinaryWrite(buffer);
                    Response.End();
                    Response.Close();
                }
                else if (isPdf)
                {
                    orderVM.Type = "Export";

                    //added by faraaz
                    if (invoicePdf)
                        orderVM.Type = "invoicePdf";

                    Response.ContentType = "application/pdf";
                    if (invoicePdf)
                        Response.AddHeader("Content-Disposition", "inline;" + orderVM.CustNumber + "_" + orderVM.QuoteID + ".pdf");
                    else
                        Response.AddHeader("Content-Disposition", "attachment;" + orderVM.CustNumber + "_" + orderVM.QuoteID + ".pdf");
                }
                pdf.ViewBag.Title = "Report Title";
                return pdf;
            }
            else
            {
                _iItemListViewService.UserVM = UserVM;
                ItemListViewModel itemVM = _iItemListViewService.GetListOfItems(Convert.ToInt32(Request.QueryString["quoteDWID"]), "", "30", 1, "");
                this.AssignUserVM(_iItemListViewService.UserVM);
                ViewData["QuoteType"] = itemVM.QuoteType;
                ViewData["QuoteTitle"] = itemVM.QuoteTitle;
                return View("../TCPViews/ItemListView", itemVM);
            }

        }
        private StringBuilder ConvertToString<T>(IList<T> data)
        {
            System.ComponentModel.PropertyDescriptorCollection properties =
              System.ComponentModel.TypeDescriptor.GetProperties(typeof(T));
            StringBuilder sb = new StringBuilder();
            List<PropertyInfo> property = typeof(T).GetProperties().ToList();
            var lstOrderList = property.Where(e => ((DisplayAttribute)e.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()) != null && ((DisplayAttribute)e.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Order != -1).OrderBy(e => ((DisplayAttribute)e.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Order).ToList();
            sb.Append("<table style=\"border-collapse:collapse;text-align:left\">");
            //write column headings
            sb.Append("<tr>");
            foreach (var prop in lstOrderList)
                if (prop.Name != "AcRcLevelText")
                    sb.Append("<td><b><font face=Arial size=" + "10px" + ">" + prop.Name + "</font></b></td>");
                else
                    sb.Append("<td><b><font face=Arial size=" + "10px" + "></font></b></td>");
            sb.Append("</tr>");
            foreach (T item in data)
            {
                sb.Append("<tr>");
                foreach (var prop in lstOrderList)
                    sb.Append("<td><font face=Arial size=" + "10px" + ">" + prop.GetValue(item) + "&nbsp; </font></td>");
                sb.Append("</tr>");
            }
            return sb;
        }
        [HttpPost]

        public ActionResult ViewSearchSingleItem(string itemID, string QuoteID, string SearchType)
        {
            ViewData["SearchCategory"] = SearchType;
            return PartialView("../TCPViews/Partial/ItemDetailedPartial", _itemservice.GetItemByID(itemID, QuoteID));
        }

        [HttpPost]
        public ActionResult selectedItem(string selectbtnid, string itemID, string quoteID, string ViewName = "")
        {
            _itemservice.UserVM = UserVM;
            _itemservice.selectedOptions(itemID, quoteID, WebSecurity.CurrentUserId);
            this.AssignUserVM(_itemservice.UserVM);
            return View("../TCPViews/ItemListDetailedView", _itemservice.GetItemListDetailByID(itemID, quoteID));
        }

        [HttpPost]
        public JsonResult UpdateStatusofAllItems(string selectbtnid, string itemID, string quoteID, string type = "")
        {
            _itemservice.UserVM = UserVM;
            ItemListViewModel itemListVM = _itemservice.selectedOptions(itemID, quoteID, WebSecurity.CurrentUserId, type);
            this.AssignUserVM(_itemservice.UserVM);
            return Json(itemListVM);
        }

        [HttpPost]
        public JsonResult AddCollectionsToQuotDWCartbyGroupID(int groupID, string quoteID, string selectedFilters)
        {
            _iItemListViewService.UserVM = UserVM;
            string itemIDs = _iItemListViewService.GetListItemIDsbyGroupID(groupID, Convert.ToInt32(quoteID), selectedFilters);
            _itemservice.UserVM = UserVM;
            ItemListViewModel itemListVM = _itemservice.selectedOptions(itemIDs, quoteID, WebSecurity.CurrentUserId);
            this.AssignUserVM(_itemservice.UserVM);
            return Json(itemListVM);
        }

        [HttpPost]
        public JsonResult AddSeriesByItemID(string itemID, int quoteID)
        {
            _itemservice.UserVM = UserVM;
            ItemListViewModel itemListVM = _itemservice.AddSeriesByItemID(itemID, quoteID, WebSecurity.CurrentUserId);
            this.AssignUserVM(_itemservice.UserVM);
            return Json(itemListVM);
        }

        [HttpPost]
        public ActionResult UpdateQuote(string ItemID)
        {
            return View("../TCPViews/ItemContainer", _itemservice.UpdateQuoteDetails(ItemID));

        }

        [HttpPost]
        public string UpdateKPLBuilderQuote(string ItemID)
        {
            _itemservice.UserVM = UserVM;
            _itemservice.UpdateQuoteDetails(ItemID);
            return ItemID;
        }

        [HttpPost]
        public string UpdateQuoteListView(string ItemID)
        {
            return _itemservice.UpdateQuoteDetails(ItemID).ToString();

        }

        [HttpPost]
        public ActionResult UpdateDW(int DWID, int Qdid, string Itemid, string selectionStatus, string ddlSelectedValue, string pgno)
        {
            KPLBasedCommonViewModel kpl = new KPLBasedCommonViewModel();
            kpl.QuoteID = Qdid;
            kpl.ItemID = Itemid;
            kpl.DWSelectionStatus = Convert.ToString(DWID);

            return View("../TCPViews/ItemListView", _iItemListViewService.UpdateDW(kpl, selectionStatus, ddlSelectedValue, Convert.ToInt32(pgno)));

        }

        [HttpPost]
        public JsonResult UpdateDWSelectionStatus(int DWID, int Qdid, string Itemid, string selectionStatus, string ddlSelectedValue, string pgno)
        {

            KPLBasedCommonViewModel kpl = new KPLBasedCommonViewModel();
            kpl.QuoteID = Qdid;
            kpl.ItemID = Itemid;
            kpl.DWSelectionStatus = Convert.ToString(DWID);
            _iItemListViewService.UserVM = UserVM;
            ItemListViewModel itemListVM = _iItemListViewService.UpdateDW(kpl, selectionStatus, ddlSelectedValue, Convert.ToInt32(pgno));
            //  this.AssignUserVM(itemListVM.UserVM);
            return Json(itemListVM);

        }

        public ActionResult GetItemListInDetail()
        {
            return View("../TCPViews/ItemListDetailedView", _itemservice.GetItemDetailedListDetailByClientID(UserVM.CRMModelProperties.CustAutoID));
        }

        public ActionResult GetItemList()
        {
            _iItemListViewService.UserVM = UserVM;
            return View("../TCPViews/ItemListView", _iItemListViewService.GetItemListDetailByClientID(UserVM.CRMModelProperties.CustAutoID));
        }


        [HttpPost]
        public ActionResult GetItemListBySelection(string selectionStatus, string ddlSelectedValue, string pgno)
        {

            ViewData["SelectedStatus"] = selectionStatus;
            return View("../TCPViews/ItemListView", _iItemListViewService.GetListOfItemsBySelection(UserVM.CRMModelProperties.CustAutoID, selectionStatus, Convert.ToInt32(ddlSelectedValue), Convert.ToInt32(pgno)));
        }

        [HttpPost]
        public ActionResult GetSingleItemListBySelectionByDW(int quoteId, string selectionStatus, string ddlSelectedValue, string pgno)
        {
            if (string.IsNullOrEmpty(ddlSelectedValue))
            {
                ddlSelectedValue = "10";
            }
            ViewData["SelectedStatus"] = selectionStatus;
            ViewBag.Title = "ItemListView";
            _iItemListViewService.UserVM = UserVM;
            return PartialView("../TCPViews/Partial/SingleItemListViewPartial", _iItemListViewService.GetSingleListOfItems(quoteId, ddlSelectedValue, Convert.ToInt32(pgno), selectionStatus));
        }

        public ActionResult ViewSingleDetailedItem(string itemID, string QuoteID, string groupID)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            _QuoteViewSrv.UserVM = UserVM;
            if (Roles.IsUserInRole("Repo") || Roles.IsUserInRole("AdminRep"))
            {
                ViewData["GroupID"] = groupID;
                ViewData["GroupName"] = _iItemListViewService.getGroupName(Convert.ToInt32(groupID));
                ViewData["QuoteType"] = _QuoteViewSrv.getQuoteTypeText(Convert.ToInt32(QuoteID));
            }
            ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(QuoteID));
            return View("../TCPViews/SingleItemDetailedView", _itemservice.GetSingleItemDetailsWithSets(itemID, QuoteID));
        }


        //Method for updating the status of DW in SingleItemListView
        [HttpPost]
        public ActionResult UpdateDWinSingleItemDetailedView(int DWID, int Qdid, string Itemid)
        {
            KPLBasedCommonViewModel kpl = new KPLBasedCommonViewModel();
            kpl.QuoteID = Qdid;
            kpl.ItemID = Itemid;
            kpl.DWSelectionStatus = Convert.ToString(DWID);
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(Qdid));
            return View("../TCPViews/SingleItemDetailedView", _itemservice.UpdateDWSingleItemDetails(kpl));
        }

        public ActionResult GetItemListView(int iD, string type)
        {
            if (type == "DefaultDW")
            {
                if (UserVM != null)
                {
                    if (UserVM.DWDetails.Count > 1)
                    {
                        return RedirectToAction("GetProducts", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = 60, quoteID = 0 });
                    }
                    else
                    {
                        UserVM.CurrentQuoteID = UserVM.DWDetails.FirstOrDefault().Key;
                    }
                }
            }
            if (type == "Group")
            {
                ViewData["GroupName"] = _iItemListViewService.getGroupName(Convert.ToInt32(iD));
                ViewData["QuoteType"] = type;
            }
            else
            {
                IQuoteViewService _QuoteViewSrv = new QuoteViewService();
                _QuoteViewSrv.UserVM = UserVM;
                ViewData["QuoteType"] = type;
                ViewData["GroupName"] = Resources.TCPResources.AllText;
                ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(iD));
            }
            _iItemListViewService.UserVM = UserVM;
            ItemListViewModel ivm = _iItemListViewService.GetListOfItems(Convert.ToInt32(iD), type, "10", 1, "5");
            ViewData["QuoteTitle"] = ivm.QuoteTitle;
            this.AssignUserVM(ivm.UserVM);
            return View("../TCPViews/ItemListView", ivm);
        }

        public ActionResult GetCustomerDWView(int iD, string type)
        {
            if (type == "DefaultDW" || type == "LiveCustDW")
            {
                if (UserVM != null)
                {
                    ShoppingCartService shoppingcartsrv = new ShoppingCartService();
                    int noftitles = shoppingcartsrv.GetNoofBooksCountbyQuoteID(UserVM.DWDetails.FirstOrDefault().Key);
                    if ((UserVM.DWDetails.Count > 1 || noftitles == 0) && type == "DefaultDW")
                    {
                        return RedirectToAction("GetProducts", "ItemContainerPartial", new { groupID = (int)GroupEnum.EntirePenworthyCollection, currentPageIndex = 1, noofItemsPerPage = 60, quoteID = 0 });
                    }
                    else
                    {
                        UserVM.CurrentQuoteID = UserVM.DWDetails.FirstOrDefault().Key;

                    }
                }
            }
            ItemListViewModel iListVM = GetDWItemsList(iD, type, "0", "0", "");
            return View("../TCPViews/CustDecisionWizardView", iListVM);
        }
        public ActionResult GetItemListPartial(int iD, string type, string ddlSelectedValue, string pgno, string selectionStatus, bool IsSingleView = false)
        {
            ItemListViewModel iListVM = GetDWItemsList(iD, type, ddlSelectedValue, pgno, selectionStatus, IsSingleView);
            if (IsSingleView)
            {
                return PartialView("../TCPViews/Partial/ItemListViewPartial", iListVM);
            }
            else
            {
                return PartialView("../TCPViews/Partial/CustDecisionWizardPartialView", iListVM);
            }
        }

        private ItemListViewModel GetDWItemsList(int iD, string type, string ddlSelectedValue, string pgno, string selectionStatus, bool IsSingleView = false)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            _QuoteViewSrv.UserVM = UserVM;

            if (type == "Group")
            {
                ViewData["GroupName"] = _iItemListViewService.getGroupName(Convert.ToInt32(iD));
                ViewData["QuoteType"] = type;
            }
            else
            {
                ViewData["QuoteType"] = type;
                ViewData["GroupName"] = Resources.TCPResources.AllText;
                ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(iD));
            }
            ViewData["SelectedStatus"] = selectionStatus;

            _iItemListViewService.UserVM = UserVM;
            ItemListViewModel iListVM = _iItemListViewService.GetDWItemsList(iD, type, ddlSelectedValue, Convert.ToInt32(pgno), selectionStatus, IsSingleView);

            if (IsSingleView == false)
            {
                iListVM.IsListView = true;
                iListVM.KPLItemListVM.ToList().ForEach(t => t.IsListView = true);
            }

            this.AssignUserVM(iListVM.UserVM);

            return iListVM;
        }

        [HttpPost]
        public JsonResult insertItem(string itemID, string quoteID, int quantity)
        {
            _itemservice.UserVM = UserVM;
            bool itemidstatus = true;
            bool itemExist = false;
            ItemContainerService _itemContainerSrv = new ItemContainerService();
            if (_iItemListViewService.CheckItemInMas(itemID))
            {
                itemExist = true;
                List<string> lstPreviewableItemIds = _itemContainerSrv.GetPreviewableItemIDs();

                QuoteViewService _QuoteViewService = new QuoteViewService();
                int quotetypeID = _QuoteViewService.getQuoteTypeId(Convert.ToInt32(quoteID));

                if (quotetypeID == (int)QuoteTypeEnum.Preview)
                { itemidstatus = lstPreviewableItemIds.Contains(itemID); }

                if (itemidstatus)
                {
                    _itemservice.selectedOptions(itemID, quoteID, WebSecurity.CurrentUserId, "", quantity);
                    this.AssignUserVM(_itemservice.UserVM);
                }
            }
            ActiveQuoteService aqs = new ActiveQuoteService();
            RapidEntryModel rem = aqs.getRapidEntry(Convert.ToInt32(quoteID));
            rem.ItemStatus = itemidstatus;
            rem.ItemExist = itemExist;
            return Json(rem);
        }

        [HttpPost]
        public JsonResult rapidEntryItemByISBN(string ISBN, string QuoteID, int quantity)
        {
            string lstItemIds = _itemservice.GetItemIDsListByIsbn(ISBN, QuoteID);
            bool itemidstatus = false;
            _itemservice.UserVM = UserVM;
            if (lstItemIds != null && lstItemIds != string.Empty)
            {
                itemidstatus = true;
                _itemservice.selectedOptions(lstItemIds, QuoteID, WebSecurity.CurrentUserId, "", quantity);
            }
            ActiveQuoteService aqs = new ActiveQuoteService();
            RapidEntryModel rem = aqs.getRapidEntry(Convert.ToInt32(QuoteID));
            rem.ItemStatus = itemidstatus;
            return Json(rem);
        }

        [AllowAnonymous]
        public ActionResult HelpDecisionWizard()
        {
            BaseViewModel BaseViewModel = new BaseViewModel();
            BaseViewModel.UserVM = UserVM;
            return View("../TCPViews/HelpwithyourDecisionWizardView", BaseViewModel);
        }

        [AllowAnonymous]
        public ActionResult PlayVideoInfo()
        {
            return new VideoResult();
        }

    }
}
