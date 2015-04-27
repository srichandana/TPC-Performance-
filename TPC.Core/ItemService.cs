using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Context;
using TPC.Context.Interfaces;
using TPC.Core.Mapping;
using AutoMapper;
using TPC.Context.EntityModel;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using TPC.Common.Enumerations;
using TPC.Common.Constants;
namespace TPC.Core
{
    public class ItemService : ServiceBase<IItemModel>, IItemService
    {
        public ItemDetailedViewModel GetItemByID(string itemID, string Quoteid, string type = "")
        {
            ItemDetailedViewModel kplVM = new ItemDetailedViewModel();
            kplVM.LstItemParentVM = new List<ItemParentViewModel>();

            int quoteid = UserVM != null ? UserVM.CurrentQuoteID : Convert.ToInt32(Quoteid);
            Item item = _Context.Item.GetSingle(e => e.ItemID == itemID);

            ItemListViewService itemListViewSrv = new ItemListViewService();

            int? seriesID = item.Series;
            int? originalSetID = null;
            originalSetID = item != null ? item.SetID : (int?)null;

            QuoteViewService QuoteViewService = new QuoteViewService();

            Item itVM = null;
            List<QuoteDetail> lstQuoteDetailsType = null;
            List<Item> itemList = new List<Item>();
            Item setItemInfo = null;


            if (originalSetID != null)
            {
                string setID = Convert.ToString(originalSetID);
                setItemInfo = _Context.Item.GetSingle(e => e.ItemID == setID);
                itemList = itemListViewSrv.GetActiveItemList().Where(e => e.SetID == originalSetID).ToList();
            }

            itemList = itemListViewSrv.SortingAlgorithim(itemList);
            itemList.Remove(item);
            itemList.Insert(0, item);
            kplVM.LstItemParentVM.Add(new ItemParentViewModel());
            kplVM.LstItemParentVM.Add(new ItemParentViewModel());
            Quote currentQuote = new Quote();
            string quotetypetext = "";
            Quote scQuote = GetQuoteByLoggedIn(quoteid, type);

            if (quoteid != 0)
            {
                currentQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid); //GetQuoteByLoggedIn(quoteid, type);

                lstQuoteDetailsType = scQuote.QuoteDetails.ToList();
                quotetypetext = currentQuote.QuoteType.QuoteTypeText;
            }
            kplVM.QuoteTypeText = quotetypetext;

            ItemContainerService itemContainerService = new ItemContainerService();
            itemContainerService.UserVM = UserVM;
            List<string> lstTitlesBroughtBeforeItemIDs = itemContainerService.GetTitlesBroughtBeforeItemIDs();
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();
            if (UserVM != null && UserVM.CRMModelProperties != null)
            {
                lstSeriesBroughtBeforeItemIDs = itemContainerService.GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
                lstCharacterBroughtBeforeItemIDs = itemContainerService.GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            }

            kplVM.LstItemParentVM[0].ListItemVM = itemList.Select(e => new ItemViewModel
            {
                ItemID = e.ItemID,
                Title = e.Title,
                IPrice = (double)e.Price,
                ISBN = e.ISBN,
                Description = e.Description,
                Lexile = e.Lexile,
                ARLevel = e.ARLevel,
                Author = e.Author != null ? e.Author.AuthorName : string.Empty,
                IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(e.ItemID) ? true : false,
                SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(e.ItemID) ? true : false,
                CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(e.ItemID) ? true : false,
                //ProductLine =e.ProductLine
            }).ToList();
            if (quoteid != 0)
            {
                kplVM.LstItemParentVM[0].ListItemVM = UpdateIsScDWQuoteStatus(currentQuote.QuoteTypeID, lstQuoteDetailsType,
                    kplVM.LstItemParentVM[0].ListItemVM);
                kplVM.LstItemParentVM[0].ListItemVM.ToList().ForEach(e => e.QuoteTypeText = quotetypetext);
                kplVM.LstItemParentVM[0].ListItemVM.ToList().ForEach(e => e.QuoteFlag = type);
                if (currentQuote.QuoteTypeID == (int)QuoteTypeEnum.Preview)
                {
                    List<string> lstPreviewableItemIds = itemContainerService.GetPreviewableItemIDs();
                    kplVM.LstItemParentVM[0].ListItemVM.Where(e => lstPreviewableItemIds.Contains(e.ItemID)).ToList().ForEach(e => e.IsPreviewItem = true);
                }
            }
            if (kplVM.LstItemParentVM[0].ListItemVM.Count > 0)
            {
                kplVM.LstItemParentVM[0].ListItemVM.Where(e => e.ItemID == itemID).FirstOrDefault().IsSelected = true;
            }

            kplVM.SetItemsTotalPrice = Convert.ToDouble(kplVM.LstItemParentVM[0].ListItemVM.Sum(e => e.IPrice));
            itVM = item;
            string strempty = string.Empty;
            kplVM.Binding = "Penworthy " + itVM.Type == null ? strempty : itVM.Type + " Book";
            kplVM.InterestGrade = itVM.InterestGrade == null ? strempty : itVM.InterestGrade == ((int)InterestGradeEnums.Grade2to3).ToString() ? QuoteValidationConstants.Grade2to3 : itVM.InterestGrade == ((int)InterestGradeEnums.Grade4Above).ToString() ? QuoteValidationConstants.Grade4Above : itVM.InterestGrade == QuoteValidationConstants.Preschooltograde1Text ? QuoteValidationConstants.Preschooltograde1 : string.Empty;
            kplVM.ItemID = itVM.ItemID;
            kplVM.ISBN = itVM.ISBN == null ? string.Empty : itVM.ISBN;
            kplVM.ReviewSource = (itVM.SLJ == "Y" ? QuoteValidationConstants.SLJ + "," : string.Empty) + (itVM.PW == "Y" ? QuoteValidationConstants.PW + "," : string.Empty) + (itVM.Horn == "Y" ? QuoteValidationConstants.HORN + "," : string.Empty) + (itVM.Kirkus == "Y" ? QuoteValidationConstants.KIRKUS + "," : string.Empty) + (itVM.LJ == "Y" ? QuoteValidationConstants.LJ : string.Empty);
            kplVM.Subject = itVM.Subject == null ? strempty : itVM.Subject;
            kplVM.Primarycharacter = itVM.SeriesAndCharacter == null ? strempty : itVM.SeriesAndCharacter.SCText;
            kplVM.SecondCharacter = itVM.SecondaryCharacter == null ? strempty : itVM.SecondaryCharacter;
            kplVM.Format = itVM.Format == null ? strempty : itVM.Format;
            kplVM.Series = itVM.SeriesAndCharacter1 == null ? strempty : itVM.SeriesAndCharacter1.SCText;
            //kplVM.Quantity = itVM.QuoteDetails.FirstOrDefault().Quantity == null ? 1 : itVM.QuoteDetails.FirstOrDefault().Quantity;
            QuoteDetail quote = lstQuoteDetailsType != null ? lstQuoteDetailsType.Where(e => e.ItemID == itVM.ItemID).FirstOrDefault() : null;
            if (quote != null)
            {
                kplVM.Quantity = quote.Quantity;
            }
            else { kplVM.Quantity = 1; }
            kplVM.Size = itVM.Size == null ? strempty : itVM.Size;
            kplVM.Fiction = ClassificationEnums.Fiction.ToString();
            kplVM.Dewey = itVM.Dewery == null ? strempty : itVM.Dewery;
            kplVM.ARLevel = itVM.ARLevel == null ? strempty : itVM.ARLevel;
            kplVM.ARQuiz = itVM.ARQuiz;
            kplVM.Barcode = itVM.Barcode == null ? strempty : itVM.Barcode;
            kplVM.Classification = itVM.Classification == null ? strempty : itVM.Classification;
            kplVM.CopyRight = itVM.Copyright == null ? 0 : Convert.ToInt32(itVM.Copyright);
            kplVM.Description = itVM.Description == null ? strempty : itVM.Description;
            kplVM.Illustrator = itVM.Illustrator == null ? strempty : itVM.Illustrator;
            kplVM.ISBN = itVM.ISBN == null ? strempty : itVM.ISBN;
            kplVM.Lexile = itVM.Lexile == null ? strempty : itVM.Lexile;
            kplVM.OnListDate = itVM.OnListDate == null ? strempty : string.Format("{0:d}", itVM.OnListDate);
            kplVM.Pages = itVM.Pages;
            kplVM.Price = itVM.Price;
            kplVM.RCLevel = itVM.RCLevel == null ? strempty : itVM.RCLevel;
            kplVM.RCQuiz = itVM.RCQuiz;
            kplVM.Title = itVM.Title == null ? string.Empty : itVM.Title;
            kplVM.SetTitle = setItemInfo != null ? setItemInfo.Title == null ? string.Empty : setItemInfo.Title : string.Empty;
            kplVM.SetDescription = setItemInfo != null ? setItemInfo.Description == null ? string.Empty : setItemInfo.Description : string.Empty;
            kplVM.Type = itVM.ProductLine == null ? string.Empty : itVM.ProductLine.Trim().ToString();
            kplVM.Publisher = itVM.PublisherID == null ? strempty : _Context.Publisher.GetSingle(e => e.PublisherID == itVM.PublisherID).PublisherName;
            kplVM.Author = itVM.AuthorID == null ? strempty : _Context.Author.GetSingle(e => e.AuthorID == itVM.AuthorID).AuthorName;
            kplVM.ProductLine = itVM.ProductLine == "" ? strempty : itVM.ProductLine;
            kplVM.QuoteFlag = type;

            QuoteDetail quoteDetails = null;
            quoteDetails = _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteid && e.ItemID == itemID);
            if (quoteid != 0 && originalSetID != null)
            {
                kplVM.QuoteID = Convert.ToInt32(quoteid);
                kplVM.QuoteTypeID = (Int32)_Context.Quote.GetSingle(e => e.QuoteID == quoteid).QuoteTypeID;

                if (quoteDetails != null && lstQuoteDetailsType != null)
                {
                    kplVM.IsInSCDWQuote = kplVM.LstItemParentVM[0].ListItemVM.Where(e => e.ItemID == itemID).FirstOrDefault().IsInSCDWQuote;
                    kplVM.DWSelectionStatus = _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteid && e.ItemID == itemID) != null ? _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteid && e.ItemID == itemID).DWSelectionID.ToString() : "";
                }
                else
                {
                    kplVM.IsInSCDWQuote = kplVM.LstItemParentVM[0].ListItemVM.Where(e => e.ItemID == itemID).FirstOrDefault().IsInSCDWQuote;
                    kplVM.DWSelectionStatus = kplVM.IsInSCDWQuote ? "1" : "5";
                }
            }

            if (Quoteid != "0" && originalSetID == null)
            {
                // QuoteDetail quoteDetails = quoteDetails// _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteid && e.ItemID == itemID);
                if (quoteDetails != null)
                {
                    kplVM.IsInSCDWQuote = kplVM.LstItemParentVM[0].ListItemVM.Where(e => e.ItemID == itemID).FirstOrDefault().IsInSCDWQuote;
                    kplVM.DWSelectionStatus = Convert.ToString(quoteDetails.DWSelectionID);
                }
            }

            kplVM.UserVM = UserVM;
            return kplVM;
        }

        public Quote GetQuoteByLoggedIn(int quoteId, string type = "")
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


        public List<ItemViewModel> UpdateIsScDWQuoteStatus(int? quoteType, List<QuoteDetail> lstQuoteDetailsType, List<ItemViewModel> list)
        {
            List<ItemViewModel> lstUpdated = new List<ItemViewModel>();
            foreach (ItemViewModel ivm in list)
            {
                if (quoteType == (int)QuoteTypeEnum.ShoppingCart)
                {
                    if (lstQuoteDetailsType.Any(e => e.ItemID == ivm.ItemID))
                    {
                        ivm.IsInSCDWQuote = true;
                        lstUpdated.Add(ivm);
                    }
                    else
                    {
                        lstUpdated.Add(ivm);
                    }
                }
                else
                {
                    if (lstQuoteDetailsType.Any(e => e.ItemID == ivm.ItemID))
                    {
                        ivm.IsInSCDWQuote = true;
                        lstUpdated.Add(ivm);
                    }
                    else
                    {
                        lstUpdated.Add(ivm);
                    }
                }

            }

            return lstUpdated;
        }


        //Method to return selected item details and its sets
        public SingleItemDetailedModel GetSingleItemDetailsWithSets(string itemID, string quoteDWID)
        {

            SingleItemDetailedModel singleItemDetails = new SingleItemDetailedModel();
            singleItemDetails.ItemGroupVm = new ItemGroupViewModel();
            singleItemDetails.KPLViewModel = new KPLBasedCommonViewModel();
            singleItemDetails.ItemGroupVm.ItemPVM = new ItemParentViewModel();
            ItemListViewService itemListViewSrv = new ItemListViewService();

            Item item = _Context.Item.GetSingle(e => e.ItemID == itemID);//Getting particular selected item details
            //Getting shoppingcart details

            Quote scQuote = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
            List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();

            //binding sets
            int? originalSetID = item != null ? item.SetID : (int?)null;
            if (originalSetID != null)
            {
                List<Item> itemList = itemListViewSrv.GetActiveItemList().Where(e => e.SetID == originalSetID).ToList();

                itemList.Remove(item);
                itemList.Insert(0, item);


                singleItemDetails.ItemGroupVm.GroupName = itemList.FirstOrDefault().SetName;
                singleItemDetails.ItemGroupVm.GroupItemCount = itemList.Count;

                singleItemDetails.ItemGroupVm.ItemPVM.ListItemVM = AutoMapper.Mapper.Map<IList<Item>, IList<ItemViewModel>>(itemList).ToList();

                singleItemDetails.ItemGroupVm.ItemPVM.ListItemVM.Where(e => lstSCQuoteDetails.Any(x => x.ItemID == e.ItemID)).ToList().ForEach(it => it.IsInSCDWQuote = true);

                if (singleItemDetails.ItemGroupVm.ItemPVM.ListItemVM.Count > 0)
                {
                    singleItemDetails.ItemGroupVm.ItemPVM.ListItemVM.Where(e => e.ItemID == itemID).FirstOrDefault().IsSelected = true;
                }
                //binding KPL
                int qDWID = Convert.ToInt32(quoteDWID);
                //  Item ItemDetails = itemList.Where(e => e.ItemID == itemNo).FirstOrDefault();
                singleItemDetails.KPLViewModel.ARLevel = item.ARLevel == null ? string.Empty : item.ARLevel;
                singleItemDetails.KPLViewModel.ARQuiz = item.ARQuiz;
                singleItemDetails.KPLViewModel.Barcode = item.Barcode == null ? string.Empty : item.Barcode;
                singleItemDetails.KPLViewModel.Classification = item.Classification == null ? string.Empty : item.Classification;
                singleItemDetails.KPLViewModel.CopyRight = item.Copyright == null ? 0 : Convert.ToInt32(item.Copyright);
                singleItemDetails.KPLViewModel.Description = item.Description == null ? string.Empty : item.Description;
                singleItemDetails.KPLViewModel.Format = item.Format == null ? string.Empty : item.Format;
                singleItemDetails.KPLViewModel.Illustrator = item.Illustrator == null ? string.Empty : item.Illustrator;
                singleItemDetails.KPLViewModel.InterestGrade = item.InterestGrade == null ? string.Empty : item.InterestGrade;
                singleItemDetails.KPLViewModel.ISBN = item.ISBN == null ? string.Empty : item.ISBN;
                singleItemDetails.KPLViewModel.ItemID = item.ItemID;
                singleItemDetails.KPLViewModel.Lexile = item.Lexile == null ? string.Empty : item.Lexile;
                singleItemDetails.KPLViewModel.OnListDate = item.OnListDate == null ? string.Empty : string.Format("{0:d}", item.OnListDate);
                singleItemDetails.KPLViewModel.Primarycharacter = item.SeriesAndCharacter == null ? string.Empty : item.SeriesAndCharacter.SCText;
                singleItemDetails.KPLViewModel.SecondCharacter = item.SecondaryCharacter == null ? string.Empty : item.SecondaryCharacter;
                singleItemDetails.KPLViewModel.Pages = item.Pages;
                singleItemDetails.KPLViewModel.Price = item.Price;
                singleItemDetails.KPLViewModel.RCLevel = item.RCLevel == null ? string.Empty : item.RCLevel;
                singleItemDetails.KPLViewModel.RCQuiz = item.RCQuiz;
                singleItemDetails.KPLViewModel.ReviewSource = item.ReviewSource == null ? string.Empty : item.ReviewSource;
                singleItemDetails.KPLViewModel.Series = item.SeriesAndCharacter1 == null ? string.Empty : item.SeriesAndCharacter1.SCText;
                singleItemDetails.KPLViewModel.Title = item.Title == null ? string.Empty : item.Title;
                singleItemDetails.KPLViewModel.Type = item.ProductLine == null ? string.Empty : item.ProductLine.Trim().ToString();
                singleItemDetails.KPLViewModel.Publisher = item.PublisherID == null ? string.Empty : _Context.Publisher.GetSingle(e => e.PublisherID == item.PublisherID).PublisherName;
                singleItemDetails.KPLViewModel.Author = item.AuthorID == null ? string.Empty : _Context.Author.GetSingle(e => e.AuthorID == item.AuthorID).AuthorName;
                List<QuoteDetail> lstcurrentQuoteDetails = _Context.QuoteDetail.GetAll(e => e.QuoteID == qDWID).ToList();
                singleItemDetails.noOfNewCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New && e.QuoteID == qDWID).Count();
                singleItemDetails.noOfYesCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes && e.QuoteID == qDWID).Count();
                singleItemDetails.noOfNoCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No && e.QuoteID == qDWID).Count();
                singleItemDetails.noOfMaybeCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe && e.QuoteID == qDWID).Count();

                if (string.IsNullOrEmpty(item.QuoteDetails.Where(e => e.QuoteID == qDWID && e.ItemID == item.ItemID).ToString()) || item.QuoteDetails.Count == 0)
                {
                    singleItemDetails.KPLViewModel.DWSelectionStatus = "5";
                }
                else
                {
                    singleItemDetails.KPLViewModel.DWSelectionStatus = item.QuoteDetails.FirstOrDefault().DWSelectionID.ToString();
                }

                singleItemDetails.KPLViewModel.QuoteID = qDWID;
            }

            return singleItemDetails;
        }
        //Update the DWStatus in SingleItemDetailedView
        public SingleItemDetailedModel UpdateDWSingleItemDetails(KPLBasedCommonViewModel itemViewModel)
        {
            #region Private Variables
            int currentDWStatus = Convert.ToInt32(itemViewModel.DWSelectionStatus);
            int DWYes = (int)DecisionWhizardStatusEnum.Yes;
            int DWNo = (int)DecisionWhizardStatusEnum.No;
            int DWMayBe = (int)DecisionWhizardStatusEnum.MayBe;

            #endregion
            QuoteDetail dwquoteDetail = new QuoteDetail();

            Quote scQuote = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
            Quote DWQuote = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
            List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();
            if (lstSCQuoteDetails != null)//item is in shoppingcart
            {
                QuoteDetail scQuoteDetail = lstSCQuoteDetails.Where(e => e.ItemID == itemViewModel.ItemID).FirstOrDefault();
                dwquoteDetail = _Context.QuoteDetail.GetSingle(e => e.QuoteID == itemViewModel.QuoteID && e.ItemID == itemViewModel.ItemID);
                if (scQuoteDetail != null)
                {
                    if (currentDWStatus == DWNo || currentDWStatus == DWMayBe)
                    {
                        //delete from sc
                        _Context.QuoteDetail.Delete(scQuoteDetail);
                    }
                }
                else
                {
                    if (currentDWStatus == DWYes)
                    {
                        //Insert In to Sc
                        _Context.QuoteDetail.Add(new QuoteDetail
                        {
                            ItemID = itemViewModel.ItemID,
                            Quantity = 1,
                            CreatedDate = DateTime.Now,
                            QuoteID = scQuote.QuoteID,
                            DWSelectionID = (int)DecisionWhizardStatusEnum.Yes,
                            UpdateDate = DateTime.Now
                        });
                        //Insert In to DW
                        if (dwquoteDetail == null)
                        {
                            _Context.QuoteDetail.Add(new QuoteDetail
                            {
                                ItemID = itemViewModel.ItemID,
                                Quantity = 1,
                                CreatedDate = DateTime.Now,
                                QuoteID = DWQuote.QuoteID,
                                DWSelectionID = (int)DecisionWhizardStatusEnum.Yes,
                                UpdateDate = DateTime.Now
                            });
                        }
                    }

                }
                if (dwquoteDetail != null)
                {
                    dwquoteDetail.DWSelectionID = Convert.ToInt32(itemViewModel.DWSelectionStatus);
                }
            }

            _Context.QuoteDetail.SaveChanges();

            //To get next element in list when we select either of yes no may be in singleItemDetailedview
            List<QuoteDetail> quoteDetailList = new List<QuoteDetail>();
            ItemListViewModel _itemList = new ItemListViewModel();
            _itemList.KPLItemListVM = new List<KPLBasedCommonViewModel>();
            quoteDetailList = _Context.QuoteDetail.GetAll(e => e.QuoteID == itemViewModel.QuoteID).ToList();
            List<Item> lstItem = quoteDetailList.Select(e => e.Item).ToList();
            _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();
            int index = _itemList.KPLItemListVM.FindIndex(e => e.ItemID == itemViewModel.ItemID);
            if (index < _itemList.KPLItemListVM.Count() - 1)
            {
                itemViewModel.ItemID = _itemList.KPLItemListVM[index + 1].ItemID;
            }
            SingleItemDetailedModel singleItemDetailes = GetSingleItemDetailsWithSets(itemViewModel.ItemID.ToString(), itemViewModel.QuoteID.ToString());

            return singleItemDetailes;
        }

        //ItemListDetailed view method for returning list of items excluding yes status(list contains only no,maybeor new items)
        public ItemListDetailedViewModel GetItemListDetailByID(string itemID, string quoteID)
        {

            List<QuoteDetail> quoteDetailListExcludingYes = new List<QuoteDetail>();
            ItemListDetailedViewModel ItemLstDetailVM = new ItemListDetailedViewModel();

            int quoteDWID = Convert.ToInt32(quoteID);
            quoteDetailListExcludingYes = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteDWID && (e.DWSelectionID != (int)DecisionWhizardStatusEnum.Yes)).ToList();

            if (quoteDetailListExcludingYes != null)
            {
                if (quoteDetailListExcludingYes.Count > 0)
                {
                    itemID = quoteDetailListExcludingYes.FirstOrDefault().ItemID.ToString();
                    ItemLstDetailVM.ItemDVM = GetItemByID(itemID, quoteID);
                }
            }
            ItemLstDetailVM.noOfYesCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Count();
            ItemLstDetailVM.noOfNoCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).Count();
            ItemLstDetailVM.noOfMaybeCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe).Count();
            ItemLstDetailVM.noOfNewCount = _Context.QuoteDetail.GetAll(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New).Count();

            return ItemLstDetailVM;


        }

        public ItemListDetailedViewModel GetItemDetailedListDetailByClientID(int custUserID)
        {

            Quote quote = _Context.Quote.GetSingle(e => e.UserID == custUserID && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard);

            return GetItemListDetailByID(string.Empty, quote.QuoteID.ToString());
        }

        //addToCart,YesNoMayBE,AddToSet
        public ItemListViewModel selectedOptions(string lstiID, string quoteID, int userID, string type = "", int quantity = 0)
        {
            string[] itemList = lstiID.Split(',');
            int quoteid = Convert.ToInt32(quoteID);
            int currentquote = Convert.ToInt32(quoteID);
            Quote CurrentQuote = null;

            Quote quoteToUpdate = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            User loggedInUser = _Context.User.GetSingle(e => e.UserId == userID);
            int rollID = loggedInUser.webpages_Roles.FirstOrDefault().RoleId;
            if ((rollID == (int)UserRolesEnum.Repo || rollID == (int)UserRolesEnum.AdminRep) && CurrentQuote == null)
            {
                CurrentQuote = type == "Preview" ? _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID) : _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            }
            else if ((rollID == (int)UserRolesEnum.CustomerShipTo) && CurrentQuote == null)
            {
                CurrentQuote = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
            }
            int dwselectionID = CurrentQuote.QuoteType.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ? (int)DecisionWhizardStatusEnum.Yes : (int)DecisionWhizardStatusEnum.New;
            List<KPLBasedCommonViewModel> lstKplViewModel = new List<KPLBasedCommonViewModel>();
            List<string> lstitemIdsToUI = new List<string>();
            _Context.Quote.SetAutoDetectChangesEnabled(false);

            foreach (string Itemid in itemList)
            {
                if (Itemid != "")
                {
                    KPLBasedCommonViewModel tempKplViewModel = new KPLBasedCommonViewModel();
                    // int itemNum = Convert.ToInt32(Itemid);
                    QuoteDetail quoteDetail = CurrentQuote.QuoteDetails.Where(e => e.ItemID == Itemid).FirstOrDefault();
                    QuoteDetail quoteDetailToUpdate = null;
                    if (quoteid != 0)
                        quoteDetailToUpdate = quoteToUpdate.QuoteDetails.SingleOrDefault(e => e.ItemID == Itemid);
                    if (quoteDetail == null || (quoteDetail.DWSelectionID != (int)DecisionWhizardStatusEnum.Yes) && (quoteDetail.DWSelectionID != (int)DecisionWhizardStatusEnum.New))
                    {
                        lstitemIdsToUI.Add(Itemid);
                    }
                    if (quoteDetail == null)
                    {
                        _Context.QuoteDetail.Add(new QuoteDetail
                        {
                            ItemID = Itemid,
                            QuoteID = CurrentQuote.QuoteID,
                            DWSelectionID = dwselectionID,
                            Quantity = quantity == 0 ? 1 : quantity,
                            CreatedDate = System.DateTime.Now,
                            UpdateDate = System.DateTime.Now
                        });
                    }

                    //Updating Penworthy Updated Date
                    if (quoteDetailToUpdate != null)
                    {
                        quoteDetailToUpdate.DWSelectionID = dwselectionID;
                        quoteDetailToUpdate.UpdateDate = System.DateTime.Now;
                    }
                    if (quoteDetail != null)
                    {
                        quoteDetail.DWSelectionID = dwselectionID;
                        quoteDetail.UpdateDate = System.DateTime.Now;
                        // quoteDetail.Quantity = 1;
                    }
                    if (rollID == (int)UserRolesEnum.Repo || rollID == (int)UserRolesEnum.AdminRep)
                    {
                        CurrentQuote.PenworthyUpdatedDate = System.DateTime.UtcNow;
                    }
                    else if (rollID == (int)UserRolesEnum.CustomerShipTo)
                    {
                        CurrentQuote.CustomerUpdatedDate = System.DateTime.UtcNow;
                    }
                    CurrentQuote.UpdateDate = System.DateTime.UtcNow;

                }
            }
            _Context.Quote.SetAutoDetectChangesEnabled(true);
            _Context.Quote.SaveChanges();

            lstKplViewModel.AddRange(_Context.Item.GetAll(e => lstitemIdsToUI.Contains(e.ItemID)).Select(item => new KPLBasedCommonViewModel()
             {
                 ItemID = item.ItemID,
                 ISBN = item.ISBN,
                 Price = item.Price,
                 Series = item.SeriesAndCharacter1 == null ? string.Empty : item.SeriesAndCharacter1.SCText,
                 Title = item.Title,
                 Author = item.Author == null ? string.Empty : item.Author.AuthorName,
                 Lexile = item.Lexile,
                 ARLevel = item.ARLevel,
                 RCLevel = item.RCLevel,
                 ProductLine = item.ProductLine.Trim(),
                 DWDate = string.Format("{0:MM/dd/yy}" ,System.DateTime.UtcNow)
             }).ToList());

            ItemListViewModel itemListVm = new ItemListViewModel();
            itemListVm.KPLItemListVM = lstKplViewModel;
            if (quoteToUpdate != null)
            {
                itemListVm.noOfYesCount =
                    quoteToUpdate.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes)
                        .Sum(e => e.Quantity);
                itemListVm.noOfNoCount =
                    quoteToUpdate.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No)
                        .Sum(e => e.Quantity);
                itemListVm.noOfMaybeCount =
                    quoteToUpdate.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe)
                        .Sum(e => e.Quantity);
                itemListVm.noOfNewCount =
                    quoteToUpdate.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New)
                        .Sum(e => e.Quantity);
                itemListVm.YesTotalPrice = (decimal)quoteToUpdate.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(e => e.Item.Price);
            }
            itemListVm.SelectionCount = itemListVm.noOfYesCount + itemListVm.noOfNoCount + itemListVm.noOfMaybeCount + itemListVm.noOfNewCount;
            itemListVm.SCItemsCount = CurrentQuote.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(e => e.Quantity);
            string taxschuduleID = CurrentQuote.User.Customer != null ? CurrentQuote.User.Customer.TaxScheduleID : null;
            decimal SalesTax = 0;
            if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                SalesTax = 0;
            else
                SalesTax = Convert.ToDecimal(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);
            decimal? price = CurrentQuote.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(e => e.Quantity * e.Item.Price);
            itemListVm.SCItemsPrice = price + (price * SalesTax);

            itemListVm.ItemIDS = lstiID;
            UserVM.SCCount = itemListVm.SCItemsCount;
            UserVM.SCPrice = itemListVm.SCItemsPrice;
            return itemListVm;
        }

        public bool UpdateQuoteDetails(string ItemID)
        {
            Quote quote_Cust = null;
            if (UserVM.CurrentQuoteID != 0)
            {
                quote_Cust = _Context.Quote.GetSingle(e => e.QuoteID == UserVM.CurrentQuoteID);
            }
            else
            {
                quote_Cust = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);

            }

            string[] itemList = ItemID.Split(',');


            foreach (string Itemid in itemList)
            {
                // int itemNum = Convert.ToInt32(Itemid);
                QuoteDetail qd = _Context.QuoteDetail.GetSingle(e => e.ItemID == Itemid && e.QuoteID == quote_Cust.QuoteID);
                if (qd != null)
                {
                    //code to update data

                    _Context.QuoteDetail.SaveChanges();

                }
                else
                {
                    //code to insert item with quoteid
                    QuoteDetail qdetail = new QuoteDetail();
                    qdetail.ItemID = ItemID;
                    qdetail.QuoteID = quote_Cust.QuoteID;
                    if (UserVM.SearchCategory == "ShoppingCart")
                    {
                        qdetail.DWSelectionID = (int)DecisionWhizardStatusEnum.Yes;//for shopping cart--1
                    }
                    else
                    {
                        qdetail.DWSelectionID = (int)DecisionWhizardStatusEnum.New;//Defulat New--5
                    }
                    qdetail.Quantity = 1;
                    qdetail.CreatedDate = System.DateTime.Now;
                    qdetail.UpdateDate = System.DateTime.Now;
                    _Context.QuoteDetail.Add(qdetail);
                    _Context.QuoteDetail.SaveChanges();


                }



            }
            //Updating Penworthy Updated Date
            Quote quote = _Context.Quote.GetSingle(a => a.QuoteID == quote_Cust.QuoteID);
            quote.PenworthyUpdatedDate = System.DateTime.UtcNow;
            quote.UpdateDate = System.DateTime.UtcNow;
            _Context.Quote.SaveChanges();

            return true;
        }

        public List<ItemViewModel> GetItemList()
        {
            return new List<ItemViewModel>();
        }



        public ItemDetailedViewModel GetItemByISBN(string ISBN, string QuoteID)
        {
            ItemListViewService _itemListViewSrv = new ItemListViewService();
            Item item = _itemListViewSrv.GetActiveItemList().Where(e => e.ISBN == ISBN).FirstOrDefault();
            ItemDetailedViewModel kplVM = new ItemDetailedViewModel();
            if (item != null)
            {
                if (ISBN != null)
                {
                    string itemID = Convert.ToString(item.ItemID);
                    kplVM = GetItemByID(itemID, QuoteID);
                    return kplVM;
                }
            }
            return kplVM;
        }

        public string GetItemIDsListByIsbn(string ISBN, string QuoteID)
        {
            ItemListViewService _itemListViewSrv = new ItemListViewService();
            ItemContainerService _itemContainerSrv = new ItemContainerService();
            string ItemIDs = string.Empty;
            List<string> lstPreviewableItemIds = _itemContainerSrv.GetPreviewableItemIDs();
            // int? setID = _Context.Item.GetSingle(x => x.ISBN == ISBN).SetID;
            Item item = _itemListViewSrv.GetActiveItemList().Where(e => e.ISBN == ISBN).FirstOrDefault();
            int quoteid = Convert.ToInt32(QuoteID);
            int quotetypeID = new QuoteViewService().getQuoteTypeId(quoteid);

            if (item != null)
            {
                ItemIDs = item.ItemID;
                int? setID = item.SetID;
                if (quotetypeID == (int)QuoteTypeEnum.Preview)
                {
                    if (!lstPreviewableItemIds.Contains(ItemIDs)) ItemIDs = string.Empty;
                }
                if (setID != null)
                {
                    ItemIDs = string.Empty;
                    if (quotetypeID == (int)QuoteTypeEnum.Preview)
                    {
                        ItemIDs = string.Join(",", _itemListViewSrv.GetActiveItemList().Where(e => e.SetID == setID && lstPreviewableItemIds.Contains(e.ItemID)).Select(e => Convert.ToString(e.ItemID)).ToList());
                    }
                    else
                    {
                        ItemIDs = string.Join(",", _itemListViewSrv.GetActiveItemList().Where(e => e.SetID == setID).Select(e => Convert.ToString(e.ItemID)).ToList());
                    }
                }
            }

            return ItemIDs;
        }




        public ItemListViewModel AddSeriesByItemID(string itemID, int quoteID, int currentUserId)
        {
            int? seriesid = _Context.Item.GetSingle(e => e.ItemID == itemID).Series;
            ItemListViewService _itemListViewSrv = new ItemListViewService();
            ItemListViewModel itemListVM = selectedOptions(string.Join(",", _itemListViewSrv.GetActiveItemList().Where(e => e.Series == seriesid).Select(e => Convert.ToString(e.ItemID)).ToArray()), quoteID.ToString(), currentUserId);
            return itemListVM;
        }

        public string getItemIdByISBN(string ISBN, string QuoteID)
        {
            int quoteid = Convert.ToInt32(QuoteID);
            int quotetypeID = new QuoteViewService().getQuoteTypeId(quoteid);
            if (quotetypeID == (int)QuoteTypeEnum.Preview)
            {
                string prevStatusenum = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
                string itemid = _Context.Item.GetSingle(e => e.ISBN == ISBN && e.Status == prevStatusenum) != null ? _Context.Item.GetSingle(e => e.ISBN == ISBN && e.Status == prevStatusenum).ItemID : string.Empty;
                return itemid;
            }
            return _Context.Item.GetSingle(e => e.ISBN == ISBN).ItemID;

        }
    }
}
