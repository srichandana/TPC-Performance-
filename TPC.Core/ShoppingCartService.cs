using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPC.Context;
using TPC.Context.Interfaces;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Context.EntityModel;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;
using TPC.Common.Enumerations;

namespace TPC.Core
{
    public class ShoppingCartService : ServiceBase<IShoppingCartModel>, IShoppingCartService
    {

        public ShoppingCartViewModel GetShoppingCartView(int quoteid)
        {
            ShoppingCartViewModel scVM = new ShoppingCartViewModel();
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            if (quote != null)
            {
                scVM.QuoteID = quoteid;
                scVM.IncludeCatalogStatus = (bool)quote.IncludeCatalogStatus;
                scVM.QuoteTypeID = (int)quote.QuoteTypeID;
                scVM.CustUserID = quote.UserID;


                scVM.CartListView = quote.QuoteDetails.Select(c => new CartViewModel
                {
                    Title = c.Item.Title,
                    Author = c.Item.Author == null ? "" : c.Item.Author.AuthorName,
                    ISBN = c.Item.ISBN,
                    AR = c.Item.ARLevel,
                    Lexile = c.Item.Lexile,
                    ItemPrice = (double)c.Item.Price,
                    ItemId = c.Item.ItemID,
                    Quantity = c.Quantity,
                    ProductLine = c.Item.ProductLine,
                    RC = c.Item.RCLevel,
                    Series = c.Item.SeriesAndCharacter1 == null ? "" : c.Item.SeriesAndCharacter1.SCText,
                    QuoteDetailID = c.QuoteDetailID,
                    Type = string.Empty,
                    LevelType = string.Empty,
                    ItemStatus = c.Item.Status
                }).OrderBy(e => e.Title).ToList();

                List<FlatFileDetailModel> lstCatalogInfo = GetCatalogInfoData(UserVM.CRMModelProperties.CustAutoID, quoteid, "Cart");

                scVM.CartListView.AddRange(lstCatalogInfo.Select(c => new CartViewModel
                {
                    Title = c.ItemNumber,
                    Author = string.Empty,
                    ISBN = string.Empty,
                    AR = null,
                    Lexile = string.Empty,
                    ItemId = c.ItemCode,
                    Quantity = c.Quantity,
                    ItemPrice = c.ItemPrice,
                    Price = c.Price,
                    ProductLine = c.ProductLine,
                    IncludeCatalog = c.ItemNumber == "Special Bulk Charge" ? false : true,
                    Type = "Catalog",
                    LevelType = c.LevelType,
                }).ToList());
                string taxschuduleID = quote.User.Customer != null ? quote.User.Customer.TaxScheduleID : null;
                scVM.CustomerName = quote.User != null ? quote.User.Customer != null ? quote.User.Customer.CustomerName : string.Empty : string.Empty;
                if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                    scVM.SalesTax = 0;
                else

                    scVM.SalesTax = Convert.ToDecimal(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);
            }
            scVM.UserVM = UserVM;
            //scVM.UserVM.CRMModelProperties.LoggedINCustomerUserID = quote.UserID;
            scVM.UserVM.SCCount = quote.QuoteDetails.Sum(e => e.Quantity);
            scVM.UserVM.SCPrice = quote.QuoteDetails.Sum(e => e.Quantity * e.Item.Price);
            CustomerShipToAddress customerShipToAddress = _Context.CustomerShipToAddress.GetSingle(e => e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            if (customerShipToAddress != null)
            {
                scVM.ShippingAddress = customerShipToAddress.ShipToAddress1 != null ? customerShipToAddress.ShipToAddress1 : customerShipToAddress.ShipToAddress2;
                scVM.ShipTo = customerShipToAddress.ShipToName;
                scVM.ShipToCity = customerShipToAddress.ShipToCity;
                scVM.State = customerShipToAddress.ShipToState;
                scVM.ZipCode = customerShipToAddress.ShipToZipCode;
                scVM.Country = customerShipToAddress.ShipToCountryCode;
            }

            return scVM;
        }

        public List<FlatFileDetailModel> GetCatalogInfoData(int custAutoID, int quoteID, string strInsertionType)
        {
            List<FlatFileDetailModel> lstCatalogInfo = new List<FlatFileDetailModel>();
            int noofBooksCount = GetNoofBooksCountbyQuoteID(quoteID);
            IFileCreationService _fileCreationSrv = new FileCreationService();
            _fileCreationSrv.UserVM = UserVM;
            if (noofBooksCount > 0)
            {
                List<FlatFileDetailModel> lstCatalogInfoValues = _fileCreationSrv.FillCatalogInfovalues(noofBooksCount, custAutoID, strInsertionType, false, string.Empty, quoteID);
                //FlatFileDetailModel CatalogProtectorInfo = _fileCreationSrv.FillCatalogProtectorValues(noofBooksCount, custAutoID, strInsertionType);
                //FlatFileDetailModel catalogShelfReadyInfo = _fileCreationSrv.FillCatalogShelfReadyValues(noofBooksCount, custAutoID, strInsertionType);
                List<FlatFileDetailModel> lstCatalogSpecialInfovalues = _fileCreationSrv.FillCatalogSpecialChargeValues(noofBooksCount, custAutoID, strInsertionType);
                //if (CatalogProtectorInfo != null) lstCatalogInfo.Add(CatalogProtectorInfo);
                //if (catalogShelfReadyInfo != null) lstCatalogInfo.Add(catalogShelfReadyInfo);
                if (lstCatalogInfoValues != null && lstCatalogInfoValues.Count() > 0) lstCatalogInfo.AddRange(lstCatalogInfoValues);
                if (lstCatalogSpecialInfovalues != null && lstCatalogSpecialInfovalues.Count() > 0) lstCatalogInfo.AddRange(lstCatalogSpecialInfovalues);
            }
            return lstCatalogInfo;
        }

        public ShoppingCartViewModel GetDWView(int quoteid)
        {
            ShoppingCartViewModel scVM = new ShoppingCartViewModel();
            scVM.QuoteID = quoteid;
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);

            scVM.CartListView = quote.QuoteDetails.Select(c => new CartViewModel
            {
                Title = c.Item.Title,
                Author = c.Item.Author == null ? "" : c.Item.Author.AuthorName,
                AR = c.Item.ARLevel,
                Lexile = c.Item.Lexile,
                ISBN = c.Item.ISBN,
                ItemPrice = (double)c.Item.Price,
                ItemId = c.ItemID,
                Quantity = c.Quantity,
                DwstatusID = c.DWSelectionID.ToString(),
                Series = c.Item.SeriesAndCharacter1 == null ? "" : c.Item.SeriesAndCharacter1.SCText,
                QuoteDetailID = c.QuoteDetailID
            }).OrderBy(e => e.Title).ToList();
            if (quote != null)
            {
                scVM.QuoteTypeID = (int)quote.QuoteTypeID;
                string taxschuduleID = quote.User.Customer != null ? quote.User.Customer.TaxScheduleID : null;
                if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                    scVM.SalesTax = 0;
                else

                    scVM.SalesTax = Convert.ToDecimal(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);

            }
            scVM.UserVM = UserVM;
            scVM.CustomerName = quote.User != null ? quote.User.Customer != null ? quote.User.Customer.CustomerName : string.Empty : string.Empty;
            return scVM;
        }

        public ShoppingCartViewModel GetShoppingCartViewByClientID(int custUserID)
        {
            ShoppingCartViewModel scVM = new ShoppingCartViewModel();

            Quote quote = _Context.Quote.GetSingle(e => e.UserID == custUserID && e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart);
            if (quote != null)
            {
                UserVM.CurrentQuoteID = quote.QuoteID;
                return GetShoppingCartView(quote.QuoteID);
            }
            else
            {
                scVM.UserVM = UserVM;
                return scVM;
            }

        }

        public ShoppingCartViewModel GetDWByClientID(string custID, int quoteId)
        {
            ShoppingCartViewModel scVM = new ShoppingCartViewModel();
            //Quote quote = _Context.Quote.GetSingle(e => e.CustomerID == custID && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard);
            if (quoteId != 0)
            {
                UserVM.CurrentQuoteID = quoteId;
                return GetDWView(quoteId);
            }
            else
            {
                scVM.UserVM = UserVM;
                return scVM;
            }
        }



        public List<string> UpdateQuantity(int currentQuoteId, int quantity, string itemID, string type = "")
        {
            ItemService itemsrvc = new ItemService();
            itemsrvc.UserVM = UserVM;
            Quote currentQuote = itemsrvc.GetQuoteByLoggedIn(currentQuoteId, type);
            //   Quote currentQuote = _Context.Quote.GetSingle(e => e.QuoteID == currentQuoteId);
            List<string> lstPriceQuantity = new List<string>();
            List<QuoteDetail> lstQD = currentQuote.QuoteDetails.ToList();
            decimal? totalPrice = 0;
            if (lstQD != null)
            {
                QuoteDetail iQD = lstQD.Where(e => e.ItemID == itemID).FirstOrDefault();
                if (iQD != null)
                {
                    iQD.Quantity = quantity;
                    totalPrice = iQD.Item.Price * quantity;
                    //Updating Penworthy Updated Date
                    if (currentQuote != null)
                    {
                        ItemListViewService lstviewsvc = new ItemListViewService();
                        lstviewsvc.UserVM = UserVM;
                        lstviewsvc.UpdatedDateTime(currentQuoteId);
                    }
                    _Context.QuoteDetail.SaveChanges();
                }
            }
            if (UserVM != null)
            {
                UserVM.SCCount = currentQuote.QuoteDetails.Sum(e => e.Quantity);
                lstPriceQuantity.Add(UserVM.SCCount.ToString());
            }
            lstPriceQuantity.Add(totalPrice.ToString());
            lstPriceQuantity.Add(currentQuote.QuoteDetails.Sum(e => e.Item.Price * e.Quantity).ToString());
            return lstPriceQuantity;
        }

        public int CreateOrder(int QuoteID, string POText, string comments)
        {
            ////quoteModel.CustomerID = UserViewModel.UserID;
            ////Quote quoteEntity = AutoMapper.Mapper.Map<QuoteModel, Quote>(quoteModel);
            //_Context.Quote.Add(quoteEntity);
            //_Context.Quote.SaveChanges();

            Quote quote = new Quote();
            quote.QuoteTitle = "Test Title";
            quote.UserID = UserVM.CRMModelProperties.LoggedINCustomerUserID;
            quote.StatusID = (int)QuoteStatusEnum.Web;
            quote.CreatedDate = DateTime.UtcNow;
            quote.UpdateDate = DateTime.UtcNow;
            quote.QuoteTypeID = (int)QuoteTypeEnum.Web;
            quote.PenworthyUpdatedDate = DateTime.UtcNow;
            quote.IncludeCatalogStatus = false;
            quote.POText = POText;
            quote.Comments = comments;
            _Context.Quote.Add(quote);
            _Context.Quote.SaveChanges();
            QuoteID = _Context.Quote.GetSingle(e => e.UserID == quote.UserID && e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart).QuoteID;
            UpdateQuoteDetails(QuoteID, quote.QuoteID);
            DeleteScItemsFromDws(QuoteID);

            UserVM.SCCount = 0;
            UserVM.SCPrice = 0;
            return quote.QuoteID;
        }

        private void UpdateQuoteDetails(int oldQuoteid, int newQuoteid)
        {
            List<QuoteDetail> quoteDetails = _Context.QuoteDetail.GetAll(e => e.QuoteID == oldQuoteid && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList();
            Quote oldQuote = _Context.Quote.GetSingle(e => e.QuoteID == oldQuoteid);
            Quote newQuote = _Context.Quote.GetSingle(e => e.QuoteID == newQuoteid);
            newQuote.IncludeCatalogStatus = oldQuote.IncludeCatalogStatus;
            newQuote.UserID = oldQuote.UserID;
            foreach (QuoteDetail qd in quoteDetails)
            {
                qd.QuoteID = newQuoteid;
                qd.UpdateDate = DateTime.Now;

                _Context.QuoteDetail.SaveChanges();
            }
            ItemListViewService itemlstsrvc = new ItemListViewService();
            itemlstsrvc.UserVM = UserVM;
            itemlstsrvc.UpdatedDateTime(oldQuote.QuoteID);
            CustomerCatalogBarcodeManipulation(UserVM.CRMModelProperties.CustAutoID, newQuoteid);
        }

        private void DeleteScItemsFromDws(int quoteId)
        {
            if (UserVM.DWDetails != null)
            {
                List<int> lstDwquoteids = UserVM.DWDetails.Select(e => e.Key).ToList();
                List<QuoteDetail> dwquotedetails = _Context.QuoteDetail.GetAll(e => lstDwquoteids.Contains((int)e.QuoteID) && (e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes || e.DWSelectionID == (int)DecisionWhizardStatusEnum.No)).ToList();
                foreach (QuoteDetail qd in dwquotedetails)
                {
                    _Context.QuoteDetail.Delete(qd);
                    _Context.QuoteDetail.SaveChanges();
                }
            }
        }
        public void DWChangesStatusToNew(List<string> lstItemids)
        {
            List<Quote> lstDWQuotes = _Context.Quote.GetAll(e => e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == (int)QuoteStatusEnum.Open).ToList();
            foreach (Quote dwQuote in lstDWQuotes)
            {
                List<QuoteDetail> dwQuoteDetail = _Context.QuoteDetail.GetAll(e => lstItemids.Contains(e.ItemID) && e.QuoteID == dwQuote.QuoteID && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList();
                foreach (QuoteDetail qd in dwQuoteDetail)
                {
                    qd.DWSelectionID = (int)DecisionWhizardStatusEnum.New;
                    _Context.Quote.SaveChanges();
                }
            }
        }
        public List<string> DeleteItem(int quoteid, string item, int QuoteTypeID)
        {
            Quote currentQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            List<QuoteDetail> quoteDetail = new List<QuoteDetail>();
            if (QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard)
            {
                quoteDetail = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid).ToList();
            }
            else
            {
                quoteDetail = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList();
            }
            int itemsCount = 0;
            decimal? itemsprice = 0;
            List<string> lstItemids = quoteDetail.Select(e => e.ItemID).ToList();
            switch (item)
            {
                case "DeleteAll":
                    {
                        foreach (QuoteDetail qd in quoteDetail)
                        {
                            _Context.QuoteDetail.Delete(qd);
                            _Context.Quote.SaveChanges();
                        }
                        DWChangesStatusToNew(lstItemids);
                        itemsCount = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList().Sum(e => e.Quantity);
                        itemsprice = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList().Sum(e => e.Quantity * e.Item.Price);

                        break;
                    }
                case "IncreaseAll":
                    {
                        foreach (QuoteDetail qd in quoteDetail)
                        {
                            qd.Quantity = qd.Quantity + 1;
                            _Context.Quote.SaveChanges();
                        }
                        itemsCount = quoteDetail.Sum(e => e.Quantity);
                        itemsprice = quoteDetail.Sum(e => e.Quantity * e.Item.Price);
                        break;
                    }
                case "DecreaseAll":
                    {
                        foreach (QuoteDetail qd in quoteDetail)
                        {
                            qd.Quantity = qd.Quantity - 1;
                            _Context.Quote.SaveChanges();
                        }
                        itemsCount = quoteDetail.Sum(e => e.Quantity);
                        itemsprice = quoteDetail.Sum(e => e.Quantity * e.Item.Price);
                        break;
                    }
                case "Sumbit":
                    {
                        Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
                        if (quote != null)
                        {
                            if (ValidateQuoteID(quoteid))
                            {
                                itemsCount = SetStateForQuote(quote, (int)QuoteStatusEnum.Transferred);
                            }
                            else
                            {
                                if (quote.StatusID == (int)QuoteStatusEnum.Open)
                                {
                                    itemsCount = SetStateForQuote(quote, (int)QuoteStatusEnum.HoldRepresentative);
                                }
                            }
                        }
                        break;
                    }
                default:
                    {
                        if (!String.IsNullOrEmpty(item))
                        {
                            // int itemid = Convert.ToInt32(item);
                            QuoteDetail qd = _Context.QuoteDetail.GetSingle(e => e.QuoteID == quoteid && e.ItemID == item);

                            _Context.QuoteDetail.Delete(qd);
                            _Context.Quote.SaveChanges();
                            lstItemids.Clear();
                            lstItemids.Add(item);
                            DWChangesStatusToNew(lstItemids);

                        }
                        itemsCount = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList().Sum(e => e.Quantity);
                        itemsprice = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList().Sum(e => e.Quantity * e.Item.Price);


                        break;
                    }

            }
            List<string> lstScountPrice = new List<string>();
            lstScountPrice.Add(itemsCount.ToString());
            lstScountPrice.Add(itemsprice.ToString());
            ItemListViewService lstviewsvc = new ItemListViewService();
            if (currentQuote != null)
            {
                lstviewsvc.UserVM = UserVM;
                lstviewsvc.UpdatedDateTime(quoteid);
            }
            return lstScountPrice;
        }

        public bool ValidateQuoteID(int quoteID)
        {
            return true;
        }

        private int SetStateForQuote(Quote quote, int quoteState)
        {
            quote.StatusID = quoteState;
            _Context.Quote.SaveChanges();

            return quoteState;
        }

        public Dictionary<string, Dictionary<string, string>> UpdateQuantityByQuoteId(int currentQuoteId, int quantity)
        {
            Dictionary<string, Dictionary<string, string>> dictItemIDCountTotalPrice = new Dictionary<string, Dictionary<string, string>>();
            // Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == currentQuoteId && e.StatusID == (int)QuoteStatusEnum.Open);
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == currentQuoteId && (e.StatusID != (int)QuoteStatusEnum.InActive || e.StatusID != (int)QuoteStatusEnum.Invoiced));
            if (quote != null)
            {
                //QuoteDetail quoteDetail = _Context.QuoteDetail.GetAll();
                //List<QuoteDetail> lstQd = _Context.QuoteDetail.GetAll(e => e.QuoteID == quote.QuoteID && e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).ToList();
                List<QuoteDetail> lstQd = _Context.QuoteDetail.GetAll(e => e.QuoteID == quote.QuoteID && e.DWSelectionID != (int)DecisionWhizardStatusEnum.No).ToList();
                lstQd = lstQd.GroupBy(e => e.ItemID).Select(a => a.FirstOrDefault()).ToList();
                foreach (QuoteDetail qd in lstQd)
                {
                    decimal? totalPrice;
                    if (qd != null)
                    {
                        if (quantity == 1)
                        {
                            if (qd.Quantity < 100)
                            {
                                qd.Quantity = qd.Quantity + 1;
                            }
                        }
                        else
                        {
                            if (qd.Quantity > 1)
                            {
                                qd.Quantity = qd.Quantity - 1;
                            }
                        }
                        totalPrice = qd.Item.Price * qd.Quantity;
                        //Updating Penworthy Updated Date


                        _Context.QuoteDetail.SaveChanges();
                        Dictionary<string, string> countTotalPrice = new Dictionary<string, string>();
                        countTotalPrice.Add(qd.Quantity.ToString(), totalPrice.ToString());
                        dictItemIDCountTotalPrice.Add(qd.ItemID.ToString(), countTotalPrice);
                    }
                }
                if (quote != null)
                {
                    ItemListViewService lstviewsvc = new ItemListViewService();
                    lstviewsvc.UserVM = UserVM;
                    lstviewsvc.UpdatedDateTime(currentQuoteId);
                }
                if (UserVM != null)
                {
                    UserVM.SCCount = lstQd.Sum(e => e.Quantity);
                }
            }
            return dictItemIDCountTotalPrice;
        }

        string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
        string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
        public int GetNoofBooksCountbyQuoteID(int quoteID)
        {
            return _Context.Quote.GetSingle(e => e.QuoteID == quoteID).QuoteDetails.Where(e => (e.Item.Status == statusenumB || e.Item.Status == statusenumD) && (e.Item.ProductLine.Trim() != "PRO" && e.Item.ProductLine.Trim() != "PUP")).Sum(x => x.Quantity);
        }



        public List<string> GetlstScDetailsbyQuoteID(int quoteID, string type = "")
        {
            ItemService itemsrvc = new ItemService();
            List<QuoteDetail> lstDWquotedetails = null;
            List<string> quoteDetails = new List<string>();


            itemsrvc.UserVM = UserVM;
            lstDWquotedetails = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteID).ToList();
            Quote scQuote = itemsrvc.GetQuoteByLoggedIn(quoteID, type);
            
            quoteID = scQuote.QuoteID;
            int itemsCount = _Context.Quote.GetSingle(e => e.QuoteID == quoteID).QuoteDetails.Sum(x => x.Quantity);
            quoteDetails.Add(itemsCount.ToString());
            decimal? itemsPrice = _Context.Quote.GetSingle(e => e.QuoteID == quoteID).QuoteDetails.Sum(x => x.Item.Price * x.Quantity);
            quoteDetails.Add(itemsPrice.ToString());

           
            ItemListViewModel itemListVm = new ItemListViewModel();
            itemListVm.noOfYesCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Count();
            itemListVm.noOfNoCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).Count();
            itemListVm.noOfMaybeCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe).Count();
            itemListVm.noOfNewCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New).Count();
            itemListVm.SelectionCount = itemListVm.noOfYesCount + itemListVm.noOfNoCount + itemListVm.noOfMaybeCount + itemListVm.noOfNewCount;
            itemListVm.SCItemsCount = scQuote.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).Sum(e => e.Quantity);
            itemListVm.YesTotalPrice = (decimal)lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Select(e => e.Item).Sum(e => e.Price);

            quoteDetails.Add(itemListVm.noOfYesCount.ToString());
            quoteDetails.Add(itemListVm.noOfNoCount.ToString());
            quoteDetails.Add(itemListVm.noOfMaybeCount.ToString());
            quoteDetails.Add(itemListVm.noOfNewCount.ToString());
            quoteDetails.Add(itemListVm.SelectionCount.ToString());
            quoteDetails.Add(itemListVm.SCItemsCount.ToString());
            quoteDetails.Add(itemListVm.YesTotalPrice.ToString());
            UserVM.SCCount = itemsCount;
            UserVM.SCPrice = itemsPrice;
            return quoteDetails;
        }

        public CartDWPdfModel GetQuotePdfDetails(int quoteid)
        {
            CartDWPdfModel quotePdfVM = new CartDWPdfModel();
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            if (quote != null)
            {
                quotePdfVM.QuoteID = quoteid;
                quotePdfVM.IncludeCatalogStatus = quote.IncludeCatalogStatus != null ? (bool)quote.IncludeCatalogStatus : false;
                List<Item> lstItem = quote.QuoteDetails.Select(e => e.Item).ToList();
                string taxschuduleID = quote.User.Customer != null ? quote.User.Customer.TaxScheduleID : null;
                if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                {
                    quotePdfVM.SalesTax = 0;
                }
                else
                {
                    quotePdfVM.SalesTax = Convert.ToDecimal(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);
                }

                string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
                string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
                quotePdfVM.CartListView = quote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).Select(c => new CartViewModel
                 {
                     Title = c.Item.Title,
                     Author = c.Item.Author == null ? "" : c.Item.Author.AuthorName,
                     ISBN = c.Item.ISBN,
                     AR = c.Item.ARLevel,
                     Lexile = c.Item.Lexile,
                     ItemPrice = (double)c.Item.Price,
                     ItemId = c.Item.ItemID,
                     DwstatusID = c.DWSelectionID.ToString(),
                     Quantity = c.Quantity,
                     Series = c.Item.SeriesAndCharacter1 == null ? "" : c.Item.SeriesAndCharacter1.SCText,
                     QuoteDetailID = c.QuoteDetailID,
                     AcRcLevelText = (c.Item.ARLevel != null && Convert.ToDouble(c.Item.ARLevel) > 0 ? "AR" : "") + (c.Item.ARLevel != null && c.Item.RCLevel != null && Convert.ToDouble(c.Item.ARLevel) > 0 && Convert.ToDouble(c.Item.RCLevel) > 0 ? "," : "") + (c.Item.RCLevel != null && Convert.ToDouble(c.Item.RCLevel) > 0 ? "RC" : ""),
                     Type = string.Empty
                 }).OrderBy(e => e.Title).ToList();

                if ((bool)quotePdfVM.IncludeCatalogStatus == true)
                {
                    List<FlatFileDetailModel> lstCatalogInfo = GetCatalogInfoData(UserVM.CRMModelProperties.CustAutoID, quoteid, "Cart");
                    quotePdfVM.CartListView.AddRange(lstCatalogInfo.Select(c => new CartViewModel
                    {
                        Title = c.ItemNumber,
                        Author = string.Empty,
                        ISBN = string.Empty,
                        AR = null,
                        Lexile = string.Empty,
                        ItemId = c.ItemCode,
                        Quantity = c.Quantity,
                        ItemPrice = c.ItemPrice,
                        Price = c.Price,
                        IncludeCatalog = c.ItemNumber == "Special Bulk Charge" ? false : true,
                        Type = "Catalog",
                    }).ToList());
                }

                quotePdfVM.QuoteTypeID = Convert.ToInt32(quote.QuoteTypeID);
                quotePdfVM.RepoAddress = new CRMModel();
                quotePdfVM.CustomerAddress = new AddressBaseModel();
                quotePdfVM.QuoteID = quoteid;
                RepUser repoAddress = null;
                if (quote.User.Customer.CustomerRep != null)
                {
                    repoAddress = quote.User.Customer.CustomerRep.RepUser != null ? quote.User.Customer.CustomerRep.RepUser : null;
                }
                else
                {
                    repoAddress = _Context.RepUser.GetSingle(e => e.RepID == 302); //Default Rep
                }
                quotePdfVM.RepoAddress.RepName = repoAddress.User != null ? repoAddress.User.FirstName + "  " + repoAddress.User.LastName : string.Empty;
                quotePdfVM.RepoAddress.RepEmail = repoAddress.User != null ? repoAddress.User.Email : string.Empty;
                quotePdfVM.RepoAddress.Persphone = repoAddress != null ? repoAddress.PhoneCustomerService : string.Empty;
                quotePdfVM.PONo = quote.QuoteSubmitSaveInfo != null ? quote.QuoteSubmitSaveInfo.PoNo : quote.POText;
                quotePdfVM.Comments = quote.QuoteSubmitSaveInfo != null ? quote.QuoteSubmitSaveInfo.QuoteSubmitComments : quote.Comments;
                CustomerAddress customeraddress = quote.User.Customer.CustomerAddress != null ? quote.User.Customer.CustomerAddress : null;
                quotePdfVM.CustomerAddress.CustomerName = quote.User != null ? quote.User.Customer != null ? quote.User.Customer.CustomerName : string.Empty : string.Empty; ;
                quotePdfVM.CustomerAddress.CustomerNo = customeraddress.Customer.CustomerNO != null ? customeraddress.Customer.CustomerNO : string.Empty;
                if (customeraddress != null)
                {
                    quotePdfVM.CustomerAddress.AddressLine1 = customeraddress.AddressLine1 != null ? customeraddress.AddressLine1 : string.Empty;
                    quotePdfVM.CustomerAddress.AddressLine2 = customeraddress.AddressLine2 != null ? customeraddress.AddressLine2 : string.Empty;
                    quotePdfVM.CustomerAddress.AddressLine3 = customeraddress.AddressLine3 != null ? customeraddress.AddressLine3 : string.Empty;
                    quotePdfVM.CustomerAddress.City = customeraddress.City != null ? customeraddress.City : string.Empty;
                    quotePdfVM.CustomerAddress.CountryCode = customeraddress.CountryCode != null ? customeraddress.CountryCode : string.Empty;
                    quotePdfVM.CustomerAddress.State = customeraddress.State != null ? customeraddress.State : string.Empty;
                    quotePdfVM.CustomerAddress.TelephoneNo = customeraddress.TelephoneNo != null ? customeraddress.TelephoneNo : string.Empty;
                    quotePdfVM.CustomerAddress.ZipCode = customeraddress.ZipCode != null ? customeraddress.ZipCode : string.Empty;
                    quotePdfVM.CustomerAddress.CustomerName = customeraddress.Customer.CustomerName != null ? customeraddress.Customer.CustomerName : string.Empty;
                    quotePdfVM.CustomerAddress.CustomerNo = customeraddress.Customer.CustomerNO != null ? customeraddress.Customer.CustomerNO : string.Empty;
                }
            }
            quotePdfVM.UserVM = UserVM;
            quotePdfVM.UserVM.CRMModelProperties.LoggedINCustomerUserID = quote.UserID;
            // quotePdfVM.UserVM.SCCount = quote.QuoteDetails.Sum(e => e.Quantity);

            return quotePdfVM;
        }


    }
}
