using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core.Models;
using TPC.Web.Filters;
using TPC.Core;
using TPC.Common.Enumerations;
using TPC.Core.Interfaces;
using Microsoft.Practices.Unity;
using System.Reflection;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;
using System.Web.UI;
using WebMatrix.WebData;
using System.Web.UI.WebControls;
using System.IO;
using System.Configuration;

namespace TPC.Web.Controllers
{

    [TPCAuthorize]

    public class ItemContainerPartialController : BaseController
    {

        private readonly IItemContainerService _itemcontainerPartialSrv;
        private readonly IUserPreferenceService _iUserPreferenceSrv;

        public ItemContainerPartialController(ItemContainerService ItemContainerSrv)
        {
            _itemcontainerPartialSrv = ItemContainerSrv;
            _iUserPreferenceSrv = new UserPreferenceService();


        }
        //
        // GET: /ItemContainerPartial/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ViewEditQuote(string quoteDWID, string type)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            ViewData["QuoteID"] = quoteDWID;
            ViewData["QuoteType"] = _QuoteViewSrv.getQuoteTypeText(Convert.ToInt32(quoteDWID));
            ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(quoteDWID));
            return ItemContainer(quoteDWID, type);
        }

        public ActionResult ItemContainer(FormCollection activeQuoteVM)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            IShoppingCartService _shoppingCartSrv = new ShoppingCartService();
            IItemListViewService _iItemListViewService = new ItemListViewService();
            _QuoteViewSrv.UserVM = UserVM;

            string selectedQuoteID = string.Empty;
            string quotetypeText = "";

            if (activeQuoteVM != null)
            {
                if (activeQuoteVM.GetValues("assignChkBx") != null)
                {
                    selectedQuoteID = activeQuoteVM.GetValues("assignChkBx").FirstOrDefault().Split('%')[0];
                    quotetypeText = activeQuoteVM.GetValues("assignChkBx").FirstOrDefault().Split('%')[1];
                    string totalItems = activeQuoteVM.GetValues("assignChkBx").FirstOrDefault().Split('%')[2];
                    string price = activeQuoteVM.GetValues("assignChkBx").FirstOrDefault().Split('%')[3];
                    ViewData["QuoteType"] = quotetypeText;
                    ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(selectedQuoteID));
                    ViewData["QuoteID"] = selectedQuoteID;
                    ViewData["SearchCategory"] = quotetypeText;
                }
            }

            //Added for Exception when no Quotes are present
            if (selectedQuoteID == string.Empty)
            {
                selectedQuoteID = activeQuoteVM["DWQuoteID"];
            }

            //For Active Quote View
            if (activeQuoteVM["View"] != null)
            {
                return ItemContainer(selectedQuoteID, "View");
            }

            //For DW View
            if (activeQuoteVM["DWView"] != null)
            {
                _shoppingCartSrv.UserVM = UserVM;
                ShoppingCartViewModel scVM = _shoppingCartSrv.GetDWByClientID(UserVM.CRMModelProperties.LoggedINCustomerUserID.ToString(), Convert.ToInt32(selectedQuoteID));
                UserVM.SearchCategory = string.Empty;
                this.AssignUserVM(scVM.UserVM);
                return View("../TCPViews/RepoDecisionWizard", scVM);
            }
            //For Dw//Active Quote Filter View
            if (activeQuoteVM["Category"] != null)
            {
                return ItemContainer(selectedQuoteID, "Category");
            }
            //For Preview of DW
            if (activeQuoteVM["DetailList"] == Resources.TCPResources.DetailList)
            {
                ViewData["GroupName"] = activeQuoteVM["DetailList"];
                _iItemListViewService.UserVM = UserVM;
                _QuoteViewSrv.UserVM = UserVM;
                ItemListViewModel itmLstVM = _iItemListViewService.GetDWItemsList(Convert.ToInt32(ViewData["QuoteID"]), Resources.TCPResources.DetailList, "10", 0,"");
                itmLstVM.KPLItemListVM.ForEach(t => t.IsListView = true);
                itmLstVM.IsListView = true;
                this.AssignUserVM(itmLstVM.UserVM);
                return View("../TCPViews/CustDecisionWizardView", itmLstVM);
            }
            //Default is for KPL-Item List
            return ItemContainer(selectedQuoteID, "KPL");
        }

        //[OutputCache(CacheProfile = "Aggressive", VaryByHeader = "X-Requested-With", Location = OutputCacheLocation.Any, NoStore = true)]
        private ActionResult ItemContainer(string quoteID, string type)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            _QuoteViewSrv.UserVM = UserVM;
            _itemcontainerPartialSrv.UserVM = UserVM;

            if (type == "View")
            {
                ViewData["SearchCategory"] = _QuoteViewSrv.getQuoteTypeText(Convert.ToInt32(quoteID));
                QuoteViewModel qVM = _QuoteViewSrv.GetQuoteView(Convert.ToInt32(quoteID));
                UserVM.SearchCategory = string.Empty;
                this.AssignUserVM(qVM.UserVM);
                return View("../TCPViews/QuoteView", qVM);
            }

            ViewData["QuoteID"] = quoteID;
            if (type == "Category")
            {
                CategoriesItemContainerViewModel categoriesItemCVM = new CategoriesItemContainerViewModel();
                categoriesItemCVM.UserVM = UserVM;
                string noofItemsPerPage = "60";
                categoriesItemCVM = _itemcontainerPartialSrv.GetSelectedCollectionItem((int)GroupEnum.EntirePenworthyCollection, 1, noofItemsPerPage, Convert.ToInt32(quoteID), "");
                this.AssignUserVM(categoriesItemCVM.UserVM);
                return View("../TCPViews/CategoriesItemContainer", categoriesItemCVM);
            }

            KPLItemConatinerViewModel ciCVM = new KPLItemConatinerViewModel();
            ciCVM.UserVM = UserVM;
            ciCVM = _itemcontainerPartialSrv.FillItemDetails(quoteID, type);
            this.AssignUserVM(ciCVM.UserVM);

            return View("../TCPViews/KPLItemContainer", ciCVM);
        }


        public ActionResult ViewSelectedGroupItemList(int quoteID, string quoteType)
        {
            ViewData["QuoteType"] = quoteType;
            _itemcontainerPartialSrv.UserVM = UserVM;
            return View("../TCPViews/CategoriesItemContainer", _itemcontainerPartialSrv.GetItemContainerVM(quoteID));
        }

        public ActionResult ViewQuoteandDecisionWizard(string quoteID, string quoteType)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            _QuoteViewSrv.UserVM = UserVM;

            ViewData["QuoteType"] = quoteType;
            ViewData["QuoteID"] = quoteID;
            ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(quoteID));

            IShoppingCartService _shoppingCartSrv = new ShoppingCartService();
            _shoppingCartSrv.UserVM = UserVM;

            if (quoteType == @Resources.TCPResources.DecisionWizardliText)
            {
                UserVM.SearchCategory = string.Empty;
                ShoppingCartViewModel scVM = _shoppingCartSrv.GetDWByClientID(UserVM.CRMModelProperties.LoggedINCustomerUserID.ToString(), Convert.ToInt32(quoteID));
                this.AssignUserVM(scVM.UserVM);
                return View("../TCPViews/RepoDecisionWizard", scVM);
            }

            if (quoteType == @Resources.TCPResources.ShoppingCartliText)
            {
                return View("../TCPViews/ShoppingCart", _shoppingCartSrv.GetShoppingCartViewByClientID(UserVM.CRMModelProperties.LoggedINCustomerUserID));
            }

            int quoteid = Convert.ToInt32(quoteID);
            if (quoteID != null)
            {
                UserVM.SearchCategory = string.Empty;
                this.AssignUserVM(_QuoteViewSrv.UserVM);
                return View("../TCPViews/QuoteView", _QuoteViewSrv.GetQuoteView(quoteid));
            }
            else
            {
                UserVM.SearchCategory = string.Empty;
                this.AssignUserVM(_QuoteViewSrv.UserVM);
                return View("../TCPViews/QuoteView", _QuoteViewSrv.GetQuoteView(UserVM.CurrentQuoteID));
            }
        }


        [HttpPost]
        public string DeleteItemFromQuote(string quoteID, string itemID,string type="")
        {
            _itemcontainerPartialSrv.UserVM = UserVM;
            _itemcontainerPartialSrv.DeleteItemFromQuote(Convert.ToInt32(quoteID), itemID,type);
            this.AssignUserVM(_itemcontainerPartialSrv.UserVM);
            return itemID;
        }

        public ActionResult ItemContainerDetails(string quoteID, string quoteType, string type)
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            ViewData["QuoteType"] = _QuoteViewSrv.getQuoteTypeText(Convert.ToInt32(quoteID));
            ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(quoteID));
            ViewData["QuoteID"] = quoteID;
            ViewData["SearchCategory"] = ViewData["QuoteType"];
            return ItemContainer(quoteID, type);

        }
        [AllowAnonymous]
        public ActionResult GetSelectedCollectionPaginationItem(int groupID, int currentPageIndex, string noofItemsPerPage, string selectedPackageIdsList = "", int quoteID = 0)
        {
            CategoriesItemContainerViewModel categoriesItemCVM = FillCategoriesItemConatinerViewModel(groupID, currentPageIndex, noofItemsPerPage, selectedPackageIdsList, quoteID);
            return PartialView("../TCPViews/Partial/CategoriesItemViewPartial", categoriesItemCVM.CategoriesPVM);
        }
        [AllowAnonymous]
        public ActionResult GetSelectedCollectionItem(int groupID, int currentPageIndex, string noofItemsPerPage, string selectedPackageIdsList = "", int quoteID = 0, string searchText = "", string isLogin = "true")
        {
            if (groupID == 0)
            {
                ViewBag.Title = "Search";
            }
            ViewBag.isLogin = isLogin;
            if (WebSecurity.IsAuthenticated && UserVM!=null)
                UserVM.SearchCategory = searchText;
            CategoriesItemContainerViewModel categoriesItemCVM = FillCategoriesItemConatinerViewModel(groupID, currentPageIndex, noofItemsPerPage, selectedPackageIdsList, quoteID);
            categoriesItemCVM.UserVM = UserVM;
            return View("../TCPViews/Partial/CategoriesItemViewPartial", categoriesItemCVM.CategoriesPVM);
        }
        public ActionResult GetProducts(int groupID, int currentPageIndex, string noofItemsPerPage, string selectedPackageIdsList = "", int quoteID = 0, string searchText = "")
        {
            if (groupID == 0)
            {
                ViewBag.Title = "Search";
            }
            if (WebSecurity.IsAuthenticated && UserVM!=null)
                UserVM.SearchCategory = searchText;

            if (WebSecurity.IsAuthenticated)
            {

                QuoteViewService qvs = new QuoteViewService();
                qvs.UserVM = UserVM;
                quoteID = qvs.getCustomerSCQuoteID();
            }
            CategoriesItemContainerViewModel categoriesItemCVM = FillCategoriesItemConatinerViewModel(groupID, currentPageIndex, noofItemsPerPage, selectedPackageIdsList, quoteID);
            categoriesItemCVM.UserVM = UserVM;
            return View("../TCPViews/Partial/CategoriesItemViewPartial", categoriesItemCVM.CategoriesPVM);
        }

        public ActionResult SearchItems(int groupID, int currentPageIndex, string noofItemsPerPage, string selectedPackageIdsList = "", int quoteID = 0, string searchText = "")
        {
            return GetSelectedCollectionItem(groupID, currentPageIndex, noofItemsPerPage, selectedPackageIdsList, quoteID, searchText);
        }

        private CategoriesItemContainerViewModel FillCategoriesItemConatinerViewModel(int groupID, int currentPageIndex, string noofItemsPerPage, string selectedPackageIdsList = "", int quoteID = 0)
        {
            CategoriesItemContainerViewModel categoriesItemCVM = new CategoriesItemContainerViewModel();
            categoriesItemCVM.UserVM = UserVM;
            _itemcontainerPartialSrv.UserVM = UserVM;

            quoteID = UserVM == null ? 0 : ViewBag.Title == "Search" || quoteID == 0 ? UserVM.CurrentQuoteID : quoteID;

            categoriesItemCVM = _itemcontainerPartialSrv.GetSelectedCollectionItem(groupID, currentPageIndex, noofItemsPerPage, quoteID, selectedPackageIdsList);

            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            _QuoteViewSrv.UserVM = UserVM;
            if (selectedPackageIdsList != "")
            {
                ViewData["grpIds"] = selectedPackageIdsList;
            }

            if (UserVM != null && UserVM.CurrentQuoteID != 0)
            {
                ViewData["QuoteType"] = _QuoteViewSrv.getQuoteTypeText(UserVM.CurrentQuoteID);
                ViewData["QuoteID"] = UserVM.CurrentQuoteID;
                ViewData["QuoteTitle"] = _QuoteViewSrv.getQuoteTitleText(Convert.ToInt32(UserVM.CurrentQuoteID));
            }

            this.AssignUserVM(categoriesItemCVM.UserVM);
            return categoriesItemCVM;
        }


        public ActionResult ExportItemListToExcel()
        {
            IQuoteViewService _QuoteViewSrv = new QuoteViewService();
            _QuoteViewSrv.UserVM = UserVM;
            ViewData["QuoteType"] = _QuoteViewSrv.getQuoteTypeText(UserVM.CurrentQuoteID);
            KPLItemConatinerViewModel lstkplVM = new KPLItemConatinerViewModel();
            lstkplVM.UserVM = UserVM;
            _itemcontainerPartialSrv.UserVM = UserVM;
            lstkplVM = _itemcontainerPartialSrv.FillItemDetails(UserVM.CurrentQuoteID.ToString(), _QuoteViewSrv.getQuoteTypeText(UserVM.CurrentQuoteID).ToString());
           
            string userSelection = string.Empty;
            if (lstkplVM.UserVM.Preferences.Count() == 0)
            {
                lstkplVM.UserVM.Preferences.Add("TabularItemPartial", Resources.TCPResources.DefaultKPLValues);
            }
            foreach (KeyValuePair<string, string> item in lstkplVM.UserVM.Preferences)
            {
                userSelection = string.IsNullOrEmpty(item.Value) ? null : "IsChecked," + item.Value;
            }


            string[] preferences = string.IsNullOrEmpty(userSelection) ? null : userSelection.Split(',');
            var Controller = DependencyResolver.Current.GetService<TPC.Web.Controllers.TCPControllers.DefaultController>();
            System.Data.DataTable dt = Controller.ConvertToDataTable(lstkplVM.KPLBasedVM, preferences);
          
            
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            string headerText = string.Format(Resources.TCPResources.QuoteEmailExcelHeaderTable, ConfigurationManager.AppSettings["imgLogoPath"], string.Format("{0:d}", DateTime.Today), UserVM.CurrentQuoteID, UserVM.CRMModelProperties.CustNO, UserVM.CRMModelProperties.RepName, UserVM.CRMModelProperties.custName, UserVM.CRMModelProperties.RepEmail, UserVM.CRMModelProperties.CustFirstName + ' ' + UserVM.CRMModelProperties.CustLastName);
            sb.Append(headerText);
            sb.Append("<table style=\"border-collapse:collapse;\">");
            //write column headings
            sb.Append("<tr>");
            foreach (System.Data.DataColumn dc in dt.Columns)
            {

                if (dc.ColumnName.ToLower().Trim() == "series" || dc.ColumnName.ToLower().Trim() == "title" || dc.ColumnName.ToLower().Trim() == "primarycharacter")
                {
                    sb.Append("<td><b><font face=Arial size=" + "10px" + ">" + "X&nbsp;&nbsp;&nbsp;" + "</font></b></td>");
                }
                if (dc.ColumnName.ToLower().Trim() == "ischecked")
                    sb.Append("<td><font face=Arial size=" + "10px" + ">" + "Select" + "</font></td>");

                else
                    sb.Append("<td><b><font face=Arial size=" + "10px" + ">" + dc.ColumnName + "</font></b></td>");
            }
            sb.Append("</tr>");

            //write table data
            foreach (System.Data.DataRow dr in dt.Rows)
            {
                sb.Append("<tr>");
                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    if (dc.ToString().ToLower().Trim() == "title")
                    {
                        if (lstkplVM.KPLBasedVM.Single(x => x.ItemID == dr["ItemId"].ToString()).IsInCustomerTitles)
                            sb.Append("<td><font face=Arial size=" + "10px" + ">" + "X" + "</font></td>");
                        else
                            sb.Append("<td><font face=Arial size=" + "10px" + ">" + "" + "</font></td>");
                    }
                    else
                    {
                        //sb.Append("<td><font face=Arial size=" + "10px" + ">" + "" + "</font></td>");
                        if (dc.ToString().ToLower() == "primarycharacter")
                            if (lstkplVM.KPLBasedVM.Single(x => x.ItemID == dr["ItemId"].ToString()).CharecterBroughtBefore)
                                sb.Append("<td><font face=Arial size=" + "10px" + ">" + "X" + "</font></td>");
                            else
                                sb.Append("<td><font face=Arial size=" + "10px" + ">" + "" + "</font></td>");


                        if (dc.ToString().ToLower().Trim() == "series")
                            if (lstkplVM.KPLBasedVM.Single(x => x.ItemID == dr["ItemId"].ToString()).SeriesBroughtBefore)
                                sb.Append("<td><font face=Arial size=" + "10px" + ">" + "X" + "</font></td>");
                            else
                                sb.Append("<td><font face=Arial size=" + "10px" + ">" + "" + "</font></td>");

                    }
                    if (dc.ToString().ToLower().Trim() == "ischecked")
                        if (dr[dc].ToString() == "checked")
                            sb.Append("<td><font face=Arial size=" + "10px" + ">" + "X" + "</font></td>");
                        else
                            sb.Append("<td><font face=Arial size=" + "10px" + ">" + "" + "</font></td>");
                    else if (dc.ToString().ToLower().Trim() == "price")
                    {
                        sb.Append("<td>" + String.Format("{0:0.00}", dr[dc].ToString()) + "</td>");
                    }
                    else
                    {
                        sb.Append("<td><font face=Arial size=" + "10px" + ">" + dr[dc].ToString() + "&nbsp;</font></td>");
                    }
                }
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            this.Response.AddHeader("Content-Disposition", "attachment; filename=ItemList.xls");
            this.Response.ContentType = "application/vnd.ms-excel";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
           return File(buffer, "application/vnd.ms-excel");
        }

       
        [AllowAnonymous]
        public ActionResult GetSelectedCollectionItemByName(string groupName="")
        {
            IQuoteViewService _quoteViewServ = new QuoteViewService();
            int groupId = _quoteViewServ.getGroupId(groupName);
            return GetSelectedCollectionItem(groupId, 1, "60");
        }

        public JsonResult DeleteNoItemsByQuoteID(int quoteID)
        {
            IItemListViewService _iItemListViewService = new ItemListViewService();
            _iItemListViewService.UserVM = UserVM;
            List<string> itmLstVM = _iItemListViewService.DeleteNoItemsByQuoteID(quoteID);
            return Json(itmLstVM, JsonRequestBehavior.AllowGet);
        }
    }
}
