using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPC.Common.Enumerations;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;
using TPC.Core.Infrastructure;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;

namespace TPC.Core
{
    public class QuoteViewService : ServiceBase<IQuoteViewModel>, IQuoteViewService
    {
        public QuoteViewModel GetQuoteView(int quoteid)
        {
            if (quoteid == 0)
            {
                quoteid = UserVM.CurrentQuoteID;
            }
            QuoteViewModel qvm = new QuoteViewModel();
            Quote quoteViewDetails = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            if (quoteViewDetails != null)
            {
                string taxschuduleID = quoteViewDetails.User.Customer != null ? quoteViewDetails.User.Customer.TaxScheduleID : null;
                if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                    qvm.SalesTax = 0;
                else

                    qvm.SalesTax = Convert.ToDecimal(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);
            }
            qvm.QuoteStatusID = (int)quoteViewDetails.StatusID;
            qvm.IncludeCatalogStatus = quoteViewDetails.IncludeCatalogStatus == null ? false : (bool)quoteViewDetails.IncludeCatalogStatus;
            qvm.PONo = quoteViewDetails.QuoteSubmitSaveInfo != null ? quoteViewDetails.QuoteSubmitSaveInfo.PoNo : quoteViewDetails.POText;
            qvm.Comments = quoteViewDetails.QuoteSubmitSaveInfo != null ? quoteViewDetails.QuoteSubmitSaveInfo.QuoteSubmitComments : quoteViewDetails.Comments;
            qvm.CustomerName = quoteViewDetails.User != null ? quoteViewDetails.User.Customer != null ? quoteViewDetails.User.Customer.CustomerName : string.Empty : string.Empty;
            qvm.CartListView = quoteViewDetails.QuoteDetails.Select(c => new CartViewModel
            {
                Title = c.Item.Title,
                Author = c.Item.Author == null ? "" : c.Item.Author.AuthorName,
                ISBN = c.Item.ISBN,
                AR = c.Item.ARLevel,
                Lexile = c.Item.Lexile,
                ItemPrice = (double)c.Item.Price,
                ItemId = c.ItemID,
                Quantity = c.Quantity,
                Series = c.Item.SeriesAndCharacter1 == null ? "" : c.Item.SeriesAndCharacter1.SCText,
                QuoteDetailID = c.QuoteDetailID,
                ProductLine = c.Item.ProductLine,
                RC = c.Item.RCLevel,
                ItemStatus = c.Item.Status,
                Type = string.Empty,
                LevelType = string.Empty,
            }).ToList();
            IShoppingCartService _shoppingCartSrv = new ShoppingCartService();
            _shoppingCartSrv.UserVM = UserVM;
            List<FlatFileDetailModel> lstCatalogInfo = _shoppingCartSrv.GetCatalogInfoData(UserVM.CRMModelProperties.CustAutoID, quoteid, "Quote");
            qvm.CartListView.AddRange(lstCatalogInfo.Select(c => new CartViewModel
            {
                Title = c.ItemNumber,
                Author = string.Empty,
                ISBN = string.Empty,
                ItemId = c.ItemCode,
                Quantity = c.Quantity,
                ItemPrice = c.ItemPrice,
                ProductLine = c.ProductLine,
                Price = c.Price,
                IncludeCatalog = c.ItemNumber == "Special Bulk Charge" ? false : true,
                Type = "Catalog",
                LevelType = c.LevelType
            }).ToList());

            //  qvm.CartListView = AutoMapper.Mapper.Map<IList<Item>, IList<CartViewModel>>(lstItem).ToList();
            qvm.QuoteID = quoteid;

            List<QuoteType> lstQuote = new List<QuoteType>();
            if (UserVM.CRMModelProperties.DivNO == 11)
            {
                lstQuote = _Context.QuoteType.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.Direct || e.QuoteTypeID == (int)QuoteTypeEnum.Web).OrderBy(e => e.QuoteTypeText).ToList();
            }
            else
            {
                lstQuote = _Context.QuoteType.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.Direct || e.QuoteTypeID == (int)QuoteTypeEnum.Preview || e.QuoteTypeID == (int)QuoteTypeEnum.Web).OrderBy(e => e.QuoteTypeText).ToList();
            }
            qvm.QuoteTypes = AutoMapper.Mapper.Map<IList<QuoteType>, IList<ComboBase>>(lstQuote).ToList();
            qvm.QuoteText = quoteViewDetails.QuoteTitle;
            UserVM.CurrentQuoteID = quoteid;
            qvm.UserVM = UserVM;
            return qvm;
        }

        public string getQuoteTypeText(int quotid)
        {
            if (quotid != 0)
            {
                string quotetyeText = _Context.Quote.GetSingle(e => e.QuoteID == quotid).QuoteType.QuoteTypeText;
                return quotetyeText;
            }
            else
            {
                string quotetyeText = _Context.Quote.GetSingle(e => e.QuoteID == UserVM.CurrentQuoteID).QuoteType.QuoteTypeText;
                return quotetyeText;
            }

        }

        public int getQuoteTypeId(int quotid)
        {
            if (quotid != 0)
            {
                int quotetyeid = _Context.Quote.GetSingle(e => e.QuoteID == quotid).QuoteType.QuoteTypeID;
                return quotetyeid;
            }
            else
            {
                int quotetyeid = _Context.Quote.GetSingle(e => e.QuoteID == UserVM.CurrentQuoteID).QuoteType.QuoteTypeID;
                return quotetyeid;
            }

        }

        public string getQuoteTitleText(int quotid)
        {
            string quoteTitleText = string.Empty;
            if (quotid != 0)
            {
                quoteTitleText = _Context.Quote.GetSingle(e => e.QuoteID == quotid).QuoteTitle;
            }
            else
            {
                if (UserVM != null)
                {
                    quoteTitleText = _Context.Quote.GetSingle(e => e.QuoteID == UserVM.CurrentQuoteID).QuoteTitle;
                }
            }
            return quoteTitleText;
        }

        public bool UpdateQuantity(CartViewModel cartViewModel)
        {
            QuoteDetail qd = _Context.QuoteDetail.GetSingle(e => e.QuoteDetailID == cartViewModel.QuoteDetailID && e.ItemID == cartViewModel.ItemId);
            qd.Quantity = cartViewModel.Quantity;
            int QuoteID = (int)qd.QuoteID;
            _Context.QuoteDetail.SaveChanges();

            //Updating Penworthy Updated Date
            Quote quote = _Context.Quote.GetSingle(a => a.QuoteID == QuoteID);
            quote.PenworthyUpdatedDate = System.DateTime.UtcNow;
            _Context.Quote.SaveChanges();
            return true;
        }

        public int getCustomerSCQuoteID()
        {
            //string custID = UserVM.CRMModelProperties.CustID;
            int quoteid = _Context.Quote.GetSingle(e => e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID && e.StatusID == (int)QuoteStatusEnum.Open && e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart).QuoteID;
            return quoteid;
        }


        public bool UpdateIncludeCatalogStatus(int quoteID, bool IncludeCatalogStatus)
        {
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            quote.IncludeCatalogStatus = IncludeCatalogStatus;
            _Context.Quote.SaveChanges();
            return true;
        }
        public int getGroupId(string groupName)
        {
            int groupId = 1;
            if (!string.IsNullOrEmpty(groupName))
            {
                Group grp = _Context.Group.GetSingle(e => e.GroupName == groupName);
                if (grp != null)
                {
                    groupId = grp.GroupID;
                }
            }
            return groupId;
        }
    }
}
