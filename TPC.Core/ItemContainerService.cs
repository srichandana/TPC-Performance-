using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context;
using TPC.Context.Interfaces;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;
using TPC.Context.EntityModel;
using TPC.Common.Enumerations;
using System.ComponentModel.DataAnnotations;
using TPC.Core.Infrastructure;
using TPC.Common.Constants;
using System.Data.SqlClient;
using System.Reflection;

namespace TPC.Core
{
    public class ItemContainerService : ServiceBase<IItemContainerModel>, IItemContainerService
    {

        //public List<FilterModel> lstMainupdatedFM = new List<FilterModel>();
        //private IContextBase _Context;

        //public ItemContainerService()
        //    : this(new ContextBase())
        //{

        //}

        //public ItemContainerService(IContextBase contextBase)
        //{
        //    _Context = contextBase ?? new ContextBase();

        //}


        public ItemContainerViewModel GetItemContainerVM(int custUserID)
        {
            ItemContainerViewModel icVM = new ItemContainerViewModel();
            List<FilterModel> lstfmodel = FillSearchCriteria(0, "Home");
            icVM.ListFilterVM = lstfmodel;
            Quote quote = null;
            int custID = custUserID; // For Home in quoteid parameter we are passing Customer ID
            if (custID != 0)
            {
                quote = _Context.Quote.GetSingle(e => e.UserID == custID && e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart);
            }
            icVM.QuoteID = quote == null ? Convert.ToInt32(custUserID) : quote.QuoteID;
            icVM.ListItemGropVM = FillHomeandProductsItemsList(Convert.ToInt32(icVM.QuoteID), lstfmodel);
            if (UserVM != null)
            {
                UserVM.CurrentQuoteID = Convert.ToInt32(icVM.QuoteID);
                icVM.UserVM = UserVM;
            }
            return icVM;
        }

        public ItemContainerViewModel GetFilters()
        {
            ItemContainerViewModel icVM = new ItemContainerViewModel();
            List<FilterModel> lstfmodel = FillSearchCriteria(0, "Home");
            icVM.ListItemGropVM = FillHomeandProductsItemsList(Convert.ToInt32(icVM.QuoteID), lstfmodel);

            return icVM;
        }

        private List<FilterModel> FillSearchCriteria(int groupId, string type, string groupIdsList = "")
        {
            string[] grpIdsList = groupIdsList.Split('_').ToArray();
            List<FilterModel> lstFM = new List<FilterModel>();
            ItemListViewService _itemlistViewSrv = new ItemListViewService();
            bool isReploggedIn = UserVM == null ? true : UserVM.CRMModelProperties.IsRepLoggedIN;
            int roleID = isReploggedIn ? (int)UserRolesEnum.Repo : (int)UserRolesEnum.CustomerShipTo;
            List<int> lstpageToDisplayID = new List<int>();
            FilterModel filterModel = new FilterModel();

            if (type == PageToDisplayEnums.Home.ToString())
            {
                lstpageToDisplayID.Add((int)PageToDisplayEnums.HomeProducts);
            }
            else
            {
                lstpageToDisplayID.Add((int)PageToDisplayEnums.Prodcuts);
                lstpageToDisplayID.Add((int)PageToDisplayEnums.HomeProducts);
            }

            List<Group> lstGroups = _Context.Group.GetAll(e => (e.UserRoleId == null || e.UserRoleId == roleID) && lstpageToDisplayID.Contains((int)e.PageToDisplay)).OrderBy(a => a.Priority).ToList();
            List<string> lstItemIds = GetItemList().Select(e => e.ItemID).ToList();
            //List<ComboBase> priority = _Context.Group.GetAll().Select(c => new ComboBase() { ItemID = c.Priority.ToString(), ItemValue = c.Priority.ToString(), Selected =  c.GroupID == groupId ? true : false }).ToList();
            lstFM = lstGroups.Select(e => new FilterModel
            {
                GroupID = e.GroupID,
                groupPackageItems = e.GroupPackageItems.Where(x => lstItemIds.Contains(x.ItemID)).Count(),
                GroupName = e.GroupName,
                Style = e.GroupStyles.Count > 0 ? e.GroupStyles.FirstOrDefault().StyleHtml : string.Empty,
                PackagePriority = (int)e.Priority,
                IsSelected = false,
                ChildGroups = e.GroupPackages.Where(g => g.GroupID == (groupId == 0 ? (int)e.GroupID : groupId)).OrderBy(a => a.Package.Priority).Select(c => new FilterModel
                {
                    GroupID = (int)c.PackageID,
                    GroupName = c.Package.PackageName,
                    IsSelected = false,
                    PackagePriority = c.Package.Priority,
                    ChildGroups = c.Package.PackageSubPackages == null ? null : c.Package.PackageSubPackages.Select(subchildP => new FilterModel { GroupID = subchildP.SubPackageID, GroupName = subchildP.Package1.PackageName, IsSelected = groupIdsList == "" ? false : grpIdsList.Contains(subchildP.SubPackageID.ToString()) ? true : false, PackagePriority = subchildP.Package.Priority }).ToList(),
                }).ToList(),
            }).ToList();
            //if(lstFM.Count > 0)
            //{
            //    lstFM.ForEach(e=>e.ddlPriority = priority);
            //}
            //lstFM.RemoveAll(e => e.ChildGroups.Count == 0);
            return lstFM;
        }

        private List<ItemGroupViewModel> FillHomeandProductsItemsList(int quoteID, List<FilterModel> lstFilterM)
        {
            List<ItemGroupViewModel> lstgroupVM = new List<ItemGroupViewModel>();
            Quote scQuote = null;
            if (quoteID != 0)
            {
                scQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            }
            lstgroupVM = PrepareGroupItemsList(lstFilterM, scQuote);
            lstgroupVM.RemoveAll(e => e.ItemPVM.ListItemVM.Count == 0);

            return lstgroupVM;
        }


        private List<ItemGroupViewModel> PrepareGroupItemsList(List<FilterModel> lstFilterM, Quote SCQuote)
        {
            ItemListViewService itemListviewsrv = new ItemListViewService();
            List<ItemGroupViewModel> lstGroupVM = new List<ItemGroupViewModel>();
            List<Item> lstiVM = null;
            ItemGroupViewModel itemGVM = new ItemGroupViewModel();
            ItemGroupViewModel itemAllGVM = new ItemGroupViewModel();
            List<Item> lstAllPackageItems = new List<Item>();
            List<string> lstTitlesBroughtBeforeItemIDs = GetTitlesBroughtBeforeItemIDs();
            List<string> lstPreviewitemIDs = GetPreviewableItemIDs();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();

            itemListviewsrv.UserVM = UserVM;

            if (UserVM != null && UserVM.CRMModelProperties != null)
            {
                lstSeriesBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
                lstCharacterBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            }
            //   lstFilterM = type == "Home" ? lstFilterM.Where(e => e.GroupID == (int)GroupEnum.Packages).ToList() : lstFilterM;
            foreach (FilterModel fm in lstFilterM)
            {
                lstGroupVM.AddRange(PrepareGroupItemListbyGroupID(fm, SCQuote, lstTitlesBroughtBeforeItemIDs, lstPreviewitemIDs, lstSeriesBroughtBeforeItemIDs, lstCharacterBroughtBeforeItemIDs));

            }
            lstGroupVM = lstGroupVM.OrderBy(e => e.GroupPriority).ToList();
            return lstGroupVM;
        }

        private List<ItemGroupViewModel> PrepareGroupItemListbyGroupID(FilterModel parentfM, Quote SCQuote, List<string> lstTitlesBroughtBeforeItemIDs, List<string> lstPreviewitemIDs, List<string> lstSeriesBroughtBeforeItemIDs, List<string> lstCharacterBroughtBeforeItemIDs)
        {
            ItemListViewService itemListviewsrv = new ItemListViewService();
            ItemGroupViewModel itemGVM = new ItemGroupViewModel();
            List<Item> lstiVM = null;
            List<ItemGroupViewModel> lstGroupVM = new List<ItemGroupViewModel>();
            itemListviewsrv.UserVM = UserVM;
            //foreach (FilterModel childFM in parentfM.ChildGroups)
            //{
            lstiVM = itemListviewsrv.GetItemsByGroupCritreria(parentfM.GroupID);
            if (lstiVM.Count() != 0)
            {
                parentfM.IsSelected = true;
                //childFM.IsSelected = true;
                itemGVM = new ItemGroupViewModel
                {
                    GroupID = parentfM.GroupID,
                    GroupName = parentfM.GroupName,
                    GroupItemCount = lstiVM.Count(),
                    GroupPriority = parentfM.PackagePriority,
                    ItemPVM = new ItemParentViewModel
                    {
                        ListItemVM = lstiVM.Select(c => new ItemViewModel
                        {
                            ItemID = c.ItemID,
                            ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                            Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                            Title = (c.Title == null ? string.Empty : c.Title),
                            IsSelected = false,
                            IsInSCDWQuote = SCQuote != null ? (SCQuote.QuoteDetails.Any(x =>
                                x.ItemID == c.ItemID && x.DWSelectionID == (SCQuote.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ?
                                (int)DecisionWhizardStatusEnum.Yes : SCQuote.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard ?
                                (int)SCQuote.QuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID : (int)DecisionWhizardStatusEnum.New)) == false ? false : true) : false,
                            QuoteTypeText = SCQuote != null ? SCQuote.QuoteType.QuoteTypeText : string.Empty,
                            IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            IsPreviewItem = SCQuote != null ? (SCQuote.QuoteTypeID == (int)QuoteTypeEnum.Preview ? (lstPreviewitemIDs.Contains(c.ItemID) ? true : false) : true) : true,
                            SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false
                        }).ToList()
                    },

                };
                //if (childFM.ChildGroups != null && childFM.ChildGroups.Count() > 0)
                //{
                //    itemGVM.lstChildItemGVM = PrepareGroupItemListbyGroupID(childFM, SCQuote, lstTitlesBroughtBeforeItemIDs, lstPreviewitemIDs, lstSeriesBroughtBeforeItemIDs, lstCharacterBroughtBeforeItemIDs);
                //}
                lstGroupVM.Add(itemGVM);
            }
            //}

            return lstGroupVM;
        }

        private ItemGroupViewModel PrepareAllGroupItems(List<Item> lstAllPackageItems, Quote SCQuote)
        {
            ItemGroupViewModel itemGVM = new ItemGroupViewModel();
            List<string> lstTitlesBroughtBeforeItemIDs = GetTitlesBroughtBeforeItemIDs();
            List<string> lstPreviewitemIDs = GetPreviewableItemIDs();
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();
            if (UserVM != null && UserVM.CRMModelProperties != null)
            {
                lstSeriesBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
                lstCharacterBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            }
            itemGVM = new ItemGroupViewModel
            {
                GroupID = 0,
                GroupName = "All",
                GroupItemCount = lstAllPackageItems.Distinct().Count(),
                ItemPVM = new ItemParentViewModel
                {
                    ListItemVM = lstAllPackageItems.Distinct().Select(c => new ItemViewModel
                    {
                        ItemID = c.ItemID,
                        ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                        Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                        Title = (c.Title == null ? string.Empty : c.Title),
                        IsSelected = false,
                        IsInSCDWQuote = SCQuote != null ? (SCQuote.QuoteDetails.Any(x =>
                                      x.ItemID == c.ItemID && x.DWSelectionID == (SCQuote.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ?
                                      (int)DecisionWhizardStatusEnum.Yes : SCQuote.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard ?
                                      (int)SCQuote.QuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID : (int)DecisionWhizardStatusEnum.New)) == false ? false : true) : false,
                        QuoteTypeText = SCQuote != null ? SCQuote.QuoteType.QuoteTypeText : string.Empty,
                        SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                        IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                        IsPreviewItem = SCQuote != null ? (SCQuote.QuoteTypeID == (int)QuoteTypeEnum.Preview ? (lstPreviewitemIDs.Contains(c.ItemID) ? true : false) : true) : true,
                        CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false
                    }).ToList()
                },
            };
            return itemGVM;
        }

        public CategoriesItemContainerViewModel GetSelectedCollectionItem(int groupID, int pageIndex, string noofItemsPerPage, int quoteID, string selectedPackageIdsList)
        {
            // int noofItemsperPage = 60;
            int noofItemsforPage = 0;
            if (noofItemsPerPage != "All") noofItemsforPage = Convert.ToInt32(noofItemsPerPage);
            int upperLimit = pageIndex == 0 ? 0 : noofItemsforPage * pageIndex;
            int lowerLimit = pageIndex == 1 ? 0 : noofItemsforPage * (pageIndex - 1);
            string paginationVal = "All";
            CategoriesItemContainerViewModel catItemCVM = new CategoriesItemContainerViewModel();
            CategoriesPartialViewModel catPVM = new CategoriesPartialViewModel();
            ItemGroupViewModel iGVM = new ItemGroupViewModel();
            catPVM.SelectedGroupID = groupID;
            catPVM.currentPageIndex = pageIndex;
            catPVM.QuoteID = quoteID;
            catPVM.UserVM = UserVM;
            catPVM.SelectedTitleText = groupID == 0 ? "Search Results for \"" + UserVM != null ? UserVM.SearchCategory : string.Empty + "\"" : GetGroupNamebyGroupID(groupID);
            catPVM.GroupDescription = groupID == 0 ? "Search Results for \"" + UserVM != null ? UserVM.SearchCategory : string.Empty + "\"" : GetGroupTypebyGroupID(groupID);
            catPVM.LstFilterModel = FillSearchCriteria(groupID, "Products", selectedPackageIdsList);
            iGVM = FillItembyGroupID(groupID, quoteID, selectedPackageIdsList, catPVM);
            catPVM.SelectedTitlesCount = iGVM.GroupItemCount;// catPVM.ItemGropVM.Where(e => e.GroupID == packageID).FirstOrDefault().KPLBasedVM != null && catPVM.LstFilterModel.Where(e => e.GroupID == packageID).FirstOrDefault().KPLBasedVM.Count() > 0 ? catPVM.LstFilterModel.Where(e => e.GroupID == packageID).FirstOrDefault().KPLBasedVM.FirstOrDefault().TotalItems : 0;
            catPVM.SelectedTitlePrice = iGVM.GroupItemCount > 0 ? System.Math.Round(iGVM.ItemPVM.ListItemVM.Sum(z => (double)z.IPrice), 2) : (double)0;// catPVM.LstFilterModel.Where(e => e.GroupID == packageID).FirstOrDefault().KPLBasedVM != null && catPVM.LstFilterModel.Where(e => e.GroupID == packageID).FirstOrDefault().KPLBasedVM.Count() > 0 ? System.Math.Round(catPVM.LstFilterModel.Where(e => e.GroupID == packageID).FirstOrDefault().KPLBasedVM.FirstOrDefault().TotalPrice, 2) : (double)0;
            if (iGVM.GroupItemCount > 0)
            {
                if (noofItemsPerPage == "All")
                {
                    noofItemsforPage = iGVM.GroupItemCount;
                }
                if (noofItemsforPage != iGVM.GroupItemCount)
                {
                    paginationVal = Convert.ToString(noofItemsforPage);
                    iGVM.ItemPVM.ListItemVM.RemoveRange(0, lowerLimit);
                    if (iGVM.ItemPVM.ListItemVM.Count > upperLimit)
                    {
                        iGVM.ItemPVM.ListItemVM.RemoveRange(upperLimit - lowerLimit, iGVM.ItemPVM.ListItemVM.Count - (upperLimit - lowerLimit));
                    }
                    if (iGVM.ItemPVM.ListItemVM.Count > (upperLimit - lowerLimit) && iGVM.ItemPVM.ListItemVM.Count != (upperLimit - lowerLimit))
                    {
                        iGVM.ItemPVM.ListItemVM = iGVM.ItemPVM.ListItemVM.GetRange(0, noofItemsforPage).ToList();
                    }
                }

            }
            catPVM.ItemGroupVM = iGVM;
            if (UserVM != null)
            {
                UserVM.CurrentQuoteID = quoteID;
            }
            catPVM.pageDenomination = Pagenation(paginationVal);
            catItemCVM.UserVM = UserVM;
            catItemCVM.CategoriesPVM = catPVM;

            return catItemCVM;
        }

        public List<ComboBase> Pagenation(string noofitems)
        {
            List<ComboBase> pagenamtion = new List<ComboBase>();
            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "20", ItemValue = "View 20 Titles", Selected = noofitems == "0" || noofitems == "20" ? true : false });
            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "40", ItemValue = "View 40 Titles", Selected = noofitems == "40" ? true : false });
            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "60", ItemValue = "View 60 Titles", Selected = noofitems == "60" ? true : false });
            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "All", ItemValue = "View All Titles", Selected = noofitems == "All" ? true : pagenamtion.Where(e => e.Selected == true).FirstOrDefault() == null ? true : false });

            return pagenamtion;
        }

        private string GetPackageNamebypackageID(int packageID)
        {
            return _Context.Package.GetSingle(e => e.PackageID == packageID).PackageName;
        }

        public string GetGroupNamebyGroupID(int groupID)
        {
            return _Context.Group.GetSingle(e => e.GroupID == groupID).GroupName;
        }

        private string GetGroupTypebyGroupID(int groupID)
        {
            return _Context.Group.GetSingle(e => e.GroupID == groupID).GroupType;
        }


        private ItemGroupViewModel FillItembyGroupID(int groupID, int quoteID, string selectedPackageIdsList = "", CategoriesPartialViewModel catPVM = null)
        {

            ItemListViewService itemListviewsrv = new ItemListViewService();
            List<Item> lstiVM = null;
            ItemGroupViewModel groupVM = new ItemGroupViewModel();
            List<Item> lstiVMnewitems = null;
            List<string> lstTitlesBroughtBeforeItemIDs = GetTitlesBroughtBeforeItemIDs();
            List<string> lstPreviewitemIDs = GetPreviewableItemIDs();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();
            Quote scQuote = null;
            if (quoteID != 0)
            {
                scQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            }
            string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
            string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
            if (scQuote != null)
            {
                catPVM.SelectedQuoteItemCount = scQuote.QuoteDetails != null && scQuote.QuoteDetails.Count > 0 ? scQuote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).Count() : 0;
                catPVM.SelectedQuoteItemPrice = scQuote.QuoteDetails != null && scQuote.QuoteDetails.Select(e => e.Item).Count() > 0 ?
                    (scQuote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).Sum(z => (double)z.Item.Price * z.Quantity)) : (double)0;
            }
            itemListviewsrv.UserVM = UserVM;

            if (UserVM != null && UserVM.CRMModelProperties != null && UserVM.CRMModelProperties.CustAutoID > 0)
            {
                lstSeriesBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
                lstCharacterBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            }
            Package package = _Context.Package.GetSingle(e => e.PackageID == groupID);
            //getting the list Item by GroupID
            if (groupID != 0)
            {
                lstiVM = itemListviewsrv.GetItemsByGroupCritreria(groupID);
            }
            else
            {
                SearchService searchService = new SearchService();
                searchService.UserVM = UserVM;
                lstiVM = searchService.GetDetails();
                int newGroupId = _Context.Group.GetAll(e => e.GroupName.Contains("I Love My Library")).OrderByDescending(d => d.CreatedDate).Select(e => e.GroupID).ToList().FirstOrDefault();
                lstiVMnewitems = itemListviewsrv.GetItemsByGroupCritreria(newGroupId);

            }
            List<Item> lstSelectedGrpItems = new List<Item>();
            if (!string.IsNullOrEmpty(selectedPackageIdsList))
            {
                string[] packageIds = selectedPackageIdsList.Split('_').ToArray();
                foreach (string selectedPackageId in packageIds)
                {
                    List<Item> lstGrpItems = new List<Item>();
                    lstGrpItems = itemListviewsrv.GetItemsByPackageCriteria(int.Parse(selectedPackageId));
                    if (lstGrpItems != null && lstGrpItems.Count > 0)
                    {
                        lstSelectedGrpItems.AddRange(lstGrpItems);
                        //Removing the items from the List which does not conatins the items in the selected Groups
                        //lstiVM.RemoveAll(e => !lstGrpItems.Contains(e));
                    }
                }
                lstiVM.RemoveAll(e => !lstSelectedGrpItems.Contains(e));
            }
            if (lstiVM.Count() != 0)
            {
                groupVM = new ItemGroupViewModel
                {
                    GroupID = groupID,
                    GroupName = package != null ? package.PackageName : "All",
                    GroupItemCount = lstiVM.Count(),
                    GroupPriority = package != null ? package.Priority : 1,
                    ItemPVM = new ItemParentViewModel
                    {
                        ListItemVM = lstiVM.Select(c => new ItemViewModel
                        {
                            ItemID = c.ItemID,
                            ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                            Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                            Title = (c.Title == null ? string.Empty : c.Title),
                            IsSelected = false,
                            IsInSCDWQuote = scQuote != null ? (scQuote.QuoteDetails.Any(x =>
                                x.ItemID == c.ItemID && x.DWSelectionID == (scQuote.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ?
                                (int)DecisionWhizardStatusEnum.Yes : (scQuote.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard || scQuote.QuoteTypeID == (int)QuoteTypeEnum.Web) ?
                                (int)scQuote.QuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID : (int)DecisionWhizardStatusEnum.New)) == false ? false : true) : false,
                            QuoteTypeText = scQuote != null ? scQuote.QuoteType.QuoteTypeText : string.Empty,
                            IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            IsPreviewItem = scQuote != null ? (scQuote.QuoteTypeID == (int)QuoteTypeEnum.Preview ? (lstPreviewitemIDs.Contains(c.ItemID) ? true : false) : true) : true,
                            SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            IPrice = (double)c.Price,
                            // Grade = string.IsNullOrEmpty(c.InterestGrade) ? "" : c.InterestGrade != null ? " ,Grade " + c.InterestGrade : string.Empty
                            ARLevel = string.IsNullOrEmpty(c.ARLevel) ? "" : c.ARLevel != null ? "AR" + c.ARLevel : string.Empty,
                            RCLevel = string.IsNullOrEmpty(c.RCLevel) ? "" : c.RCLevel != null ? " ,RC" + c.RCLevel : string.Empty,
                            DWDate = scQuote != null ? scQuote.QuoteDetails.Any(x => x.ItemID == c.ItemID) != false ? string.Format("{0:MM/dd/yy}", scQuote.QuoteDetails.Where(x => x.ItemID == c.ItemID).FirstOrDefault().CreatedDate) : string.Empty : string.Empty
                        }).ToList()
                    },

                };
            }

            if (lstiVMnewitems != null)
            {
                groupVM.SearchIGVM = new ItemGroupViewModel
                {
                    ItemPVM = new ItemParentViewModel
                    {
                        ListItemVM = lstiVMnewitems.Select(c => new ItemViewModel
                        {
                            ItemID = c.ItemID,
                            ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                            Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                            Title = (c.Title == null ? string.Empty : c.Title),
                            IsSelected = false,
                            IsInSCDWQuote = scQuote != null ? (scQuote.QuoteDetails.Any(x =>
                                x.ItemID == c.ItemID && x.DWSelectionID == (scQuote.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ?
                                (int)DecisionWhizardStatusEnum.Yes : scQuote.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard ?
                                (int)scQuote.QuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID : (int)DecisionWhizardStatusEnum.New)) == false ? false : true) : false,
                            QuoteTypeText = scQuote != null ? scQuote.QuoteType.QuoteTypeText : string.Empty,
                            IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            IsPreviewItem = scQuote != null ? (scQuote.QuoteTypeID == (int)QuoteTypeEnum.Preview ? (lstPreviewitemIDs.Contains(c.ItemID) ? true : false) : true) : true,
                            SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            IPrice = (double)c.Price,
                            // Grade = string.IsNullOrEmpty(c.InterestGrade) ? "" : c.InterestGrade != null ? " ,Grade " + c.InterestGrade : string.Empty
                            ARLevel = string.IsNullOrEmpty(c.ARLevel) ? "" : c.ARLevel != null ? "AR" + c.ARLevel : string.Empty,
                            RCLevel = string.IsNullOrEmpty(c.RCLevel) ? "" : c.RCLevel != null ? " ,RC" + c.RCLevel : string.Empty
                        }).ToList()
                    },

                };
            }

            return groupVM;
        }


        public KPLItemConatinerViewModel FillItemDetails(string quoteID, string type)
        {
            KPLItemConatinerViewModel kplCVM = new KPLItemConatinerViewModel();

            List<KPLBasedCommonViewModel> lstkplVM = null;
            lstkplVM = GetKPLItemList(quoteID);
            // UpdateBroughtBeforeStatus(lstkplVM);
            //  UpdatePreviewableItemStatus(lstkplVM, quoteID);
            kplCVM.KPLBasedVM = lstkplVM;
            kplCVM.KPLBasedVM.FirstOrDefault().QuoteID = Convert.ToInt32(quoteID);
            LoginService _loginsrvc = new LoginService();
            _loginsrvc.FillPreferences(UserVM);

            UserVM.CurrentQuoteID = Convert.ToInt32(quoteID);
            kplCVM.UserVM = UserVM;

            #region OrderTypes
            var values = string.Empty;
            string DefaultKPLValues = "ItemID,Price,Type,Title,Primarycharacter,Series";
            if (kplCVM.UserVM != null)
            {
                if (kplCVM.UserVM.Preferences != null && kplCVM.UserVM.Preferences.Count() > 0)
                {
                    if (kplCVM.UserVM.Preferences["TabularItemPartial"].Count() > 1)
                    {
                        values = kplCVM.UserVM.Preferences["TabularItemPartial"];
                    }
                    else
                    {
                        values = DefaultKPLValues;
                    }
                }
                else
                {
                    values = DefaultKPLValues;
                }
            }
            else
            {
                values = DefaultKPLValues;
            }
            kplCVM.Columns = values.Split(',').ToList<string>();
            #endregion

            return kplCVM;
        }

        private void UpdatePreviewableItemStatus(List<KPLBasedCommonViewModel> lstkplVM, string quoteID)
        {
            int qID = Convert.ToInt32(quoteID);
            Quote qt = _Context.Quote.GetSingle(e => e.QuoteID == qID);
            if (qt != null && qt.QuoteTypeID == (int)QuoteTypeEnum.Preview)
            {
                List<string> lstPreviewableItemIds = GetPreviewableItemIDs();
                lstkplVM.Where(e => lstPreviewableItemIds.Contains(e.ItemID)).ToList().ForEach(e => e.IsPreviewItem = true);
            }
            else
            {
                lstkplVM.ForEach(e => e.IsPreviewItem = true);
            }
        }
        //old method
        //private List<KPLBasedCommonViewModel> GetKPLItemList(List<Item> lstiVM, string quoteID)
        //{
        //    List<string> lstSCDWQuoteItemIds = GetListItemIDsFromQuoteDWSCbyUserID();
        //    List<string> lstQuoteItemIds = GetListItemIDsFromQuotebyQuoteID(quoteID);
        //    int quoteIDInt = Convert.ToInt32(quoteID);
        //    string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
        //    string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
        //    //List<Item> setItemInfo = _Context.Item.GetAll().Where(e => e.Status == statusenumB || e.Status == statusenumD).ToList();
        //    List<Item> lstSetItem = _Context.Item.GetAll().ToList();
        //    //lstiVM.RemoveAll(e => e.CreatedDate >= DateTime.Now.AddDays(-4));
        //    List<KPLBasedCommonViewModel> lstkplVM = lstiVM.Select(c => new KPLBasedCommonViewModel
        //    {
        //        Type = c.ProductLine,
        //        ItemID = c.ItemID,
        //        Title = c.Title,
        //        SetName = c.SetID == null ? string.Empty : lstSetItem.Where(a => a.ItemID == c.SetID.ToString()).FirstOrDefault() != null ? lstSetItem.Where(a => a.ItemID == c.SetID.ToString()).FirstOrDefault().Title : string.Empty,
        //        CopyRight = c.Copyright,
        //        Publisher = c.Publisher != null ? c.Publisher.PublisherName : string.Empty,
        //        Author = c.Author != null ? c.Author.AuthorName : string.Empty,
        //        Illustrator = c.Illustrator,
        //        Classification = c.Classification,
        //        Subject = c.Subject,
        //        ReviewSource = (c.SLJ == "Y" ? QuoteValidationConstants.SLJ + "," : string.Empty) + (c.PW == "Y" ? QuoteValidationConstants.PW + "," : string.Empty) + (c.Horn == "Y" ? QuoteValidationConstants.HORN + "," : string.Empty) + (c.Kirkus == "Y" ? QuoteValidationConstants.KIRKUS + "," : string.Empty) + (c.LJ == "Y" ? QuoteValidationConstants.LJ : string.Empty),
        //        Price = c.Price,
        //        ISBN = c.ISBN,
        //        OnListDate = string.Format("{0:d}", c.OnListDate),
        //        Pages = c.Pages,
        //        ARLevel = c.ARLevel,
        //        ARQuiz = c.ARQuiz,
        //        RCLevel = c.RCLevel,
        //        RCQuiz = c.RCQuiz,
        //        Lexile = c.Lexile,
        //        Description = (string.IsNullOrEmpty(c.Description) ? string.Empty : c.Description.Substring(0, 22) + "..."),
        //        Series = c.SeriesAndCharacter1 != null ? c.SeriesAndCharacter1.SCText : string.Empty,
        //        Primarycharacter = c.SeriesAndCharacter != null ? c.SeriesAndCharacter.SCText : string.Empty,
        //        Format = c.Format,
        //        Size = c.Size,
        //        InterestGrade = c.InterestGrade == ((int)InterestGradeEnums.Grade2to3).ToString() ? QuoteValidationConstants.Grade2to3 : c.InterestGrade == ((int)InterestGradeEnums.Grade4Above).ToString() ? QuoteValidationConstants.Grade4Above : c.InterestGrade == QuoteValidationConstants.Preschooltograde1Text ? QuoteValidationConstants.Preschooltograde1 : string.Empty,
        //        Dewery = c.Dewery,
        //        QuoteID = quoteIDInt,
        //        IsInQuoteType = lstSCDWQuoteItemIds.Contains(c.ItemID) ? true : false,
        //        IsChecked = lstQuoteItemIds.Contains(c.ItemID) ? "checked" : "",
        //    }).OrderBy(c => c.Title).ToList();

        //    return lstkplVM;
        //}
        //new method
        private List<KPLBasedCommonViewModel> GetKPLItemList(string quoteID)
        {
            SqlParameter[] parameter = {
                                           new SqlParameter("@quoteid", quoteID),
                                       new SqlParameter("@custAutoId", UserVM.CRMModelProperties.CustAutoID)
                                       };

            List<KPLItem> itemListwithSp = _Context.Item.ExecSpandReturnList("SP_getitemlistByQuoteID @quoteid,@custAutoId", parameter);
            List<KPLBasedCommonViewModel> lstkplVM = AutoMapper.Mapper.Map<IList<KPLItem>, IList<KPLBasedCommonViewModel>>(itemListwithSp).ToList();

            return lstkplVM;
        }
        private string GetSetTitleBySetID(string setID)
        {
            Item setItemInfo = _Context.Item.GetSingle(e => e.ItemID == setID);
            if (setItemInfo != null)
            {
                return setItemInfo.Title;
            }
            return string.Empty;
        }
        private void UpdateBroughtBeforeStatus(List<KPLBasedCommonViewModel> lstKPLVM)
        {
            List<string> lstTitlesBroughtBeforeItemIDs = GetTitlesBroughtBeforeItemIDs();
            List<string> lstSeriesBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
            List<string> lstCharacterBroughtBeforeItemIDs = GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            lstKPLVM.Where(e => lstTitlesBroughtBeforeItemIDs.Contains(e.ItemID)).ToList().ForEach(e => e.IsInCustomerTitles = true);
            lstKPLVM.Where(e => lstSeriesBroughtBeforeItemIDs.Contains(e.ItemID)).ToList().ForEach(e => e.SeriesBroughtBefore = true);
            lstKPLVM.Where(e => lstCharacterBroughtBeforeItemIDs.Contains(e.ItemID)).ToList().ForEach(e => e.CharecterBroughtBefore = true);
        }
        public List<string> GetPreviewableItemIDs()
        {
            string prevStatusenum = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
            return GetActiveItemList().Where(e => e.Status == prevStatusenum).Select(e => e.ItemID).ToList();
        }

        public List<string> GetTitlesBroughtBeforeItemIDs()
        {
            List<string> lstItemIds = null;
            if (UserVM != null && UserVM.CRMModelProperties != null)
            {
                lstItemIds = _Context.CustomerTitle.GetAll(e => e.CustAutoID == UserVM.CRMModelProperties.CustAutoID).Select(e => e.ItemID).ToList();
            }
            //List<string> lstISBNs = _Context.CustomerTitle.GetAll(e => e.CustomerID == 128 && e.ShipTO == 1).Select(e => e.ISBN).ToList();
            List<string> lstItemIDs = lstItemIds != null ? GetActiveItemList().Where(e => lstItemIds.Contains(e.ItemID)).Select(e => e.ItemID).ToList() : new List<string>();
            return lstItemIDs;
        }

        /// <summary>
        /// Preparing the listof Series,Customer Brough Before by CustomerID.
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public List<string> GetListSeriesCharacterbyCustomerID(int customerID, string type)
        {
            string custno = _Context.Customer.GetSingle(e => e.CustAutoID == customerID).CustomerNO;
            List<Customer> lstcustomer = _Context.Customer.GetAll(e => e.CustomerNO == custno).ToList();
            List<int?> lstSeriesCharacterIDS = new List<int?>();
            foreach (Customer cust in lstcustomer)
            {
                lstSeriesCharacterIDS.AddRange(cust.CustomerSeriesCharacters.Where(x => x.SCType == type).Select(e => e.SCID));
            }
            //   _Context.CustomerSeriesAndCharacter.GetAll(e => e.CusAutoID == customerID && e.SCType == type).Select(e => e.SCID).Distinct().ToList();

            List<string> lstItemIDs = null;
            if (type == "SE")
            {
                lstItemIDs = GetActiveItemList().Where(e => e.Series != null && lstSeriesCharacterIDS.Contains(e.Series)).Select(e => e.ItemID).ToList();
            }
            else
            {
                lstItemIDs = GetActiveItemList().Where(e => lstSeriesCharacterIDS.Contains(e.PrimeryCharacter)).Select(e => e.ItemID).ToList();
            }
            return lstItemIDs;
        }

        /// <summary>
        ///  Preparing the list of ItemID's which are present in QuoteID.
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        private List<string> GetListItemIDsFromQuotebyQuoteID(string quoteID)
        {
            int quoteIDInt = Convert.ToInt32(quoteID);
            return _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteIDInt).Select(e => e.ItemID).ToList();
        }

        /// <summary>
        ///  Preparing the list of ItemID's which are present in Customer Quote, DW and SC.
        /// </summary>
        /// <param name="quoteID"></param>
        /// <returns></returns>
        private List<string> GetListItemIDsFromQuoteDWSCbyUserID()
        {
            return _Context.QuoteDetail.GetAll(e =>
                e.Quote.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID).Select(e => e.ItemID).ToList();
        }

        /// <summary>
        /// This Method retuns the current Active Items from Item Table and Filter the Items whose SetID is NULL and ISBN is NULL
        /// </summary>
        /// <returns></returns>
        string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
        string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
        string statusN = Convert.ToString((char)ItemStatusEnum.NotOnListDoNotDisplay);
        string statusR = Convert.ToString((char)ItemStatusEnum.ReadyToDisplay);
        private List<Item> GetActiveItemList()
        {

            return _Context.Item.GetAll(e => e.ISBN != null && e.SetProfile != "Y" && e.IsInMas == true && (e.Status == statusenumB || e.Status == statusenumD)).ToList();
        }
        private List<Item> GetItemList()
        {

            return _Context.Item.GetAll(e => e.ISBN != null && e.SetProfile != "Y" && e.IsInMas == true && (e.Status == statusenumB || e.Status == statusenumD || e.Status == statusN || e.Status == statusR)).ToList();
        }

        private List<Item> GetItemListWithoutFilter()
        {
            return _Context.Item.GetAll(e => e.ISBN != null && e.SetProfile != "Y" && e.IsInMas == true).ToList();
        }

        public bool DeleteItemFromQuote(int quoteID, string itemID,string type="")
        {
            string[] itemList = itemID.Split(',');
            Quote scQuote = GetQuoteByLoggedIn(quoteID, type);

            foreach (string itemid in itemList)
            {
                QuoteDetail Quotedetail = null;
                if (scQuote != null)
                {
                    Quotedetail = _Context.QuoteDetail.GetSingle(e => e.QuoteID == scQuote.QuoteID && e.ItemID == itemid);
                    QuoteDetail dwQuotedetail = _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteID && e.ItemID == itemid);
                    if (dwQuotedetail != null)
                    {
                        dwQuotedetail.DWSelectionID = (int)DecisionWhizardStatusEnum.No;
                        _Context.QuoteDetail.SaveChanges();
                    }

                }
                else
                {
                    Quotedetail = _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteID && e.ItemID == itemid);
                }
                if (Quotedetail != null)
                {
                    _Context.QuoteDetail.Delete(Quotedetail);
                    _Context.QuoteDetail.SaveChanges();
                }
            }

            if (UserVM != null)
            {
                Quote UpdatedQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
                if (UpdatedQuote != null)
                {
                    ItemListViewService lstviewsvc = new ItemListViewService();
                    lstviewsvc.UserVM = UserVM;
                    lstviewsvc.UpdatedDateTime(quoteID);
                    UserVM.SCCount = UpdatedQuote.QuoteDetails.Sum(e => e.Quantity);
                    UserVM.SCPrice = UpdatedQuote.QuoteDetails.Sum(e => e.Item.Price);
                }

            }
            return true;
        }
        private Quote GetQuoteByLoggedIn(int quoteId, string type = "")
        {
            Quote quote = null;
            if (UserVM != null && !UserVM.CRMModelProperties.IsRepLoggedIN)
            {
                quote = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
            }
            else
            {
                quote = type == "Preview" ? _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID) : _Context.Quote.GetSingle(e => e.QuoteID == quoteId);
            }

            return quote;
        }

        public List<FilterModel> getAllGroups()
        {

            return FillSearchCriteria(0, "");
        }

        public FilterModel getCollectionDetailsByID(int id)
        {

            FilterModel filterModel = new FilterModel();
            // bool reports = _Context.Group.GetSingle(e => e.GroupID == id) != null ? _Context.Group.GetSingle(e => e.GroupID == id).Priority != 0 ? true : false : false;
            List<ComboBase> priority = _Context.Group.GetAll().Select(c => new ComboBase() { ItemID = c.Priority.ToString(), ItemValue = c.Priority.ToString(), Selected = c.GroupID == id ? true : false }).OrderBy(e => e.ItemID).ToList();
            filterModel.ddlPriority = priority;
            Group group = _Context.Group.GetSingle(e => e.GroupID == id);
            filterModel.GroupID = id;
            filterModel.PackagePriority = Convert.ToInt32(group.Priority);
            filterModel.GroupName = group.GroupName;
            filterModel.Style = group.GroupStyles.Count > 0 ? group.GroupStyles.First().StyleHtml : null;
            filterModel.Description = group.GroupType;
            filterModel.ChildGroups = group.GroupPackages.Select(c => new FilterModel
            {
                GroupID = (int)c.PackageID,
                GroupName = c.Package.PackageName,
                ChildGroups = c.Package.PackageSubPackages == null ? null : c.Package.PackageSubPackages.Select(subchildP => new FilterModel { GroupID = subchildP.SubPackageID, GroupName = subchildP.Package1.PackageName, IsSelected = false, PackagePriority = subchildP.Package.Priority }).ToList(),
            }).ToList();

            return filterModel;
        }

        public List<PackageModel> getAllpackage(int groupID)
        {
            List<PackageModel> lstPackage = _Context.Package.GetAll().Select(c => new PackageModel
            {
                PackageID = c.PackageID,
                PackageName = c.PackageName,
                IsInGroup = c.GroupPackages.Count > 0 ? c.GroupPackages.FirstOrDefault().GroupID == groupID ? "Checked" : null : null,
            }).ToList();
            return lstPackage;
        }

        public List<KPLBasedCommonViewModel> getitemlistByGruopId(int groupID)
        {
            List<Item> lstItem = GetItemList();
            string titleCount = _Context.GroupPackageItem.GetAll(e => e.GroupID == groupID).Count().ToString();
            List<Item> lstSetItem = _Context.Item.GetAll(e => e.SetID == null).ToList();
            List<KPLBasedCommonViewModel> lstCvm = lstItem.Select(c => new KPLBasedCommonViewModel
            {
                ItemID = c.ItemID,
                Title = c.Title,
                ProductLine = c.ProductLine,
                Price = c.Price,
                Series = c.SeriesAndCharacter1 == null ? "" : c.SeriesAndCharacter1.SCText,
                Primarycharacter = c.SeriesAndCharacter != null ? c.SeriesAndCharacter.SCText : string.Empty,
                SecondCharacter = c.SeriesAndCharacter != null ? c.SeriesAndCharacter.SCText : string.Empty,//SecondaryCharacter not mapped
                Author = c.Author == null ? string.Empty : c.Author.AuthorName,
                CopyRight = c.Copyright,
                SetName = c.SetID == null ? null : lstSetItem.Where(a => a.ItemID == c.SetID.ToString()).FirstOrDefault() != null ? lstSetItem.Where(a => a.ItemID == c.SetID.ToString()).FirstOrDefault().Title : string.Empty,
                Lexile = c.Lexile,
                ARLevel = c.ARLevel,
                RCLevel = c.RCLevel,
                Pages = c.Pages,
                Size = c.Size,
                OnListDate = c.OnListDate != null ? c.OnListDate.ToString() : string.Empty,
                SetProfile = c.SetProfile,
                Publisher = c.Publisher != null ? c.Publisher.PublisherName : string.Empty,
                Status = c.Status,
                Dewery = c.Dewery,
                BookList = c.Booklst,
                Horn = c.Horn,
                Kirkus = c.Kirkus,
                Lj = c.LJ,
                Pw = c.PW,
                Slj = c.SLJ,
                Subject = c.Subject,
                Classification = c.Classification,
                Format = c.Format,
                InterestGrade = c.InterestGrade == ((int)InterestGradeEnums.Grade2to3).ToString() ?
                    QuoteValidationConstants.Grade2to3 : c.InterestGrade == ((int)InterestGradeEnums.Grade4Above).ToString() ?
                    QuoteValidationConstants.Grade4Above : c.InterestGrade == QuoteValidationConstants.Preschooltograde1Text ?
                    QuoteValidationConstants.Preschooltograde1 : string.Empty,
                IsInQuoteType = c.GroupPackageItems.Count > 0 ? c.GroupPackageItems.Where(e => e.GroupID == groupID).FirstOrDefault() != null ? true : false : false,
                ItemCount = titleCount,
            }).OrderByDescending(e => e.IsInQuoteType).ToList();
            lstCvm.FirstOrDefault().lstFilterModel = getAllGroups();
            return lstCvm;
        }

        public bool AddNewOrUpdateCollection(FilterModel filtermodel)
        {


            //FilterModel filtermodel = new FilterModel();
            Group group = _Context.Group.GetSingle(e => e.GroupID == filtermodel.GroupID);
            GroupStyle groupStyle = new GroupStyle();
            if (group != null)
            {
                group.GroupName = filtermodel.GroupName;
                group.GroupType = filtermodel.Description;
                group.Priority = filtermodel.PackagePriority;
                group.Updatedate = DateTime.UtcNow;
                group.PageToDisplay = 3;
                group.UserRoleId = null;

                groupStyle = group.GroupStyles.FirstOrDefault();
                if (groupStyle == null)
                {
                    _Context.GroupStyle.Add(new GroupStyle
                    {
                        GroupID = filtermodel.GroupID,
                        StyleHtml = filtermodel.Style,
                        CreatedDate = System.DateTime.UtcNow,
                        Updatedate = System.DateTime.UtcNow
                    });
                }
                else
                {
                    groupStyle.StyleHtml = filtermodel.Style;
                    groupStyle.Updatedate = System.DateTime.UtcNow;
                }
            }
            else
            {
                Group grp = new Group();
                grp.GroupName = filtermodel.GroupName;
                grp.GroupType = filtermodel.Description;
                grp.Priority = filtermodel.PackagePriority;
                grp.CreatedDate = DateTime.UtcNow;
                grp.Updatedate = DateTime.UtcNow;
                grp.UserRoleId = null;
                grp.PageToDisplay = 3;
                groupStyle.StyleHtml = filtermodel.Style;
                groupStyle.CreatedDate = DateTime.UtcNow;
                groupStyle.Updatedate = DateTime.UtcNow;
                grp.GroupStyles.Add(groupStyle);
                _Context.Group.Add(grp);
            }
            _Context.Group.SaveChanges();
            return true;
        }

        public bool AddNewPackage(PackageModel Model)
        {
            Package packages = _Context.Package.GetSingle(e => e.PackageID == Model.PackageID);
            if (packages != null)
            {
                packages.PackageID = Model.PackageID;
                packages.PackageName = Model.PackageName;
                packages.ModifiedDate = DateTime.UtcNow;
                packages.UserRoleId = 2;
            }
            else
            {
                Package package = new Package();
                package.PackageName = Model.PackageName;
                package.CreatedDate = DateTime.UtcNow;
                package.ModifiedDate = DateTime.UtcNow;
                package.UserRoleId = 2;
                package.Priority = Model.priority;
                _Context.Package.Add(package);
            }
            _Context.Group.SaveChanges();
            return true;
        }

        public bool updatepackage(List<string> lstpackageids, int groupID)
        {

            List<GroupPackage> lstOldPackageids = _Context.GroupPackage.GetAll(e => e.GroupID == groupID).ToList();
            foreach (var grouppackage in lstpackageids)
            {
                int currentid = Convert.ToInt32(grouppackage);

                GroupPackage newGroupPackage = lstOldPackageids.Where(e => e.PackageID == currentid).FirstOrDefault();
                if (newGroupPackage == null)
                {
                    GroupPackage grouppackageid = new GroupPackage();
                    grouppackageid.PackageID = Convert.ToInt32(grouppackage);
                    grouppackageid.GroupID = groupID;
                    grouppackageid.CreatedDate = DateTime.UtcNow;
                    grouppackageid.Updatedate = DateTime.UtcNow;
                    _Context.GroupPackage.Add(grouppackageid);
                }
                else
                {
                    lstOldPackageids.Remove(newGroupPackage);
                }
            }
            foreach (var grouppackage in lstOldPackageids)
            {
                _Context.GroupPackage.Delete(grouppackage);

            }
            _Context.GroupPackage.SaveChanges();
            return true;
        }

        public Dictionary<string, string> updateCollectionItems(List<string> Groupids, List<string> lstItemIDs, List<string> lstUncheckItemIDs, int currentGroupID)
        {
            List<string> lstSelectedItemIDs = lstItemIDs;
            lstSelectedItemIDs.RemoveAll(e => e == string.Empty);
            List<string> lstUnCheckedItemIDs = lstUncheckItemIDs;

            List<GroupPackageItem> lstCurrentGroupPackageItem = _Context.GroupPackageItem.GetAll(e => e.GroupID == currentGroupID).ToList();
            //getting the list of checked and unchecked based on the current collection ItemsList
            lstSelectedItemIDs.RemoveAll(e => lstCurrentGroupPackageItem.Select(g => g.ItemID).Contains(e));
            lstUnCheckedItemIDs.RemoveAll(e => !lstCurrentGroupPackageItem.Select(x => x.ItemID).Contains(e));

            Dictionary<string, string> dctGroupList = new Dictionary<string, string>();
            foreach (string ID in Groupids)
            {
                int groupID = Convert.ToInt32(ID);
                List<GroupPackageItem> lstGrpPackegItem = _Context.GroupPackageItem.GetAll(e => e.GroupID == groupID).ToList();
                //Deleting The items which are unchecked
                foreach (string itemID in lstUnCheckedItemIDs)
                {
                    GroupPackageItem grppckagItem = _Context.GroupPackageItem.GetSingle(e => e.ItemID == itemID && e.GroupID == groupID);
                    if (grppckagItem != null)
                    {
                        _Context.GroupPackageItem.Delete(grppckagItem);
                    }
                }
                _Context.GroupPackageItem.SaveChanges();

                //Adding the items which are checked newly
                foreach (string itemID in lstSelectedItemIDs)
                {
                    if (lstGrpPackegItem.Where(e => e.ItemID == itemID).FirstOrDefault() == null)
                    {
                        _Context.GroupPackageItem.Add(new GroupPackageItem
                        {
                            GroupID = groupID,
                            ItemID = itemID,
                            CreatedDate = System.DateTime.Now,
                            ModifiedDate = System.DateTime.Now
                        });
                    }
                }
                _Context.GroupPackageItem.SaveChanges();
                int titleCount = _Context.GroupPackageItem.GetAll(e => e.GroupID == groupID).Count();
                dctGroupList.Add(ID, titleCount.ToString());
            }
            return dctGroupList;
        }

        public bool DeleteCollection(int groupID)
        {
            //FilterModel model =new FilterModel();
            var deleteitem = _Context.Group.GetSingle(e => e.GroupID == groupID);
            var packageitems = _Context.GroupPackage.GetAll().Where(e => e.GroupID == groupID);
            foreach (var i in packageitems)
            {
                var eachrecord = _Context.GroupPackage.GetSingle(e => e.GroupID == i.GroupID);
                _Context.GroupPackage.Delete(eachrecord);
            }

            List<GroupPackageItem> grouppackageitems = _Context.GroupPackageItem.GetAll().Where(e => e.GroupID == groupID).ToList();
            foreach (GroupPackageItem x in grouppackageitems)
            {
                GroupPackageItem eachrecord = _Context.GroupPackageItem.GetSingle(e => e.GroupID == x.GroupID);
                _Context.GroupPackageItem.Delete(eachrecord);
            }

            GroupStyle groupstyle = _Context.GroupStyle.GetSingle(e => e.GroupID == groupID);
            if (groupstyle != null)
            {
                _Context.GroupStyle.Delete(groupstyle);
            }

            _Context.Group.Delete(deleteitem);
            _Context.Group.SaveChanges();
            return true;
        }
        public bool DeletePackage(int packageid)
        {
            List<GroupPackage> deleteitem = _Context.GroupPackage.GetAll(e => e.PackageID == packageid).ToList();
            foreach (var item in deleteitem)
            {
                _Context.GroupPackage.Delete(item);
                _Context.GroupPackage.SaveChanges();
            }
            List<PackageSubPackage> lstdeleteitem = _Context.PackageSubPackage.GetAll(e => e.PackageID == packageid).ToList();
            foreach (var item in lstdeleteitem)
            {
                _Context.PackageSubPackage.Delete(item);
                _Context.PackageSubPackage.SaveChanges();
            }
            Package packageitem = _Context.Package.GetSingle(e => e.PackageID == packageid);
            _Context.Package.Delete(packageitem);
            _Context.Package.SaveChanges();
            return true;

        }

        public List<PackageModel> DeleteExistPackage(int id)
        {
            List<PackageModel> lstpackage = null;

            List<Package> packgenames = _Context.Package.GetAll().ToList();
            lstpackage = packgenames.Select(c => new PackageModel
            {
                PackageName = c.PackageName,
                PackageID = c.PackageID,

            }).ToList();
            if (id != 0)
            {
                DeletePackage(id);
            }
            FilterModel FilterModel = new FilterModel();
            FilterModel.lstpackages = lstpackage;
            return lstpackage;

        }

    }

}
