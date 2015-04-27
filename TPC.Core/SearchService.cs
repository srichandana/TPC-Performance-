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
    public class SearchService : ServiceBase<ISearchModel>, ISearchService
    {
        public List<Item> GetDetails()
        {
            string searchText = UserVM != null ? UserVM.SearchCategory : string.Empty;
            CategoriesItemContainerViewModel cicvm = new CategoriesItemContainerViewModel();
            ItemContainerService ics = new ItemContainerService();
            List<string> lstTitlesBroughtBeforeItemIDs = ics.GetTitlesBroughtBeforeItemIDs();
            List<string> lstPreviewitemIDs = ics.GetPreviewableItemIDs();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();
            ItemListViewService ilsv = new ItemListViewService();
            string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
            string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
            cicvm.item = ilsv.GetActiveItemList().Where(e => e.Title.ToLower().Contains(searchText.ToLower())).ToList();
            List<int> lstAuthorIDs = _Context.Author.GetAll().Where(e => e.AuthorName.Contains(searchText)).Select(e => e.AuthorID).ToList();
            if (lstAuthorIDs.Count > 0)
            {
                List<Item> lstAuthorItem = _Context.Item.GetAll().Where(e => e.ISBN != null && e.IsInMas == true && (e.Status == statusenumB || e.Status == statusenumD) && (e.AuthorID != null ? lstAuthorIDs.Contains((int)e.AuthorID) : false)).ToList();
                cicvm.item.RemoveAll(e => lstAuthorItem.Contains(e));
                cicvm.item.AddRange(lstAuthorItem);
            }
            List<int> lstSeriesIDs = _Context.SeriesAndCharacter.GetAll().Where(e => e.SCText.ToLower().Contains(searchText.ToLower())).Select(e => e.SCID).ToList();
            if (lstSeriesIDs.Count > 0)
            {
                List<Item> lstSeriesItem = _Context.Item.GetAll().Where(e => e.ISBN != null && e.IsInMas == true && (e.Status == statusenumB || e.Status == statusenumD) && (e.Series != null ? lstSeriesIDs.Contains((int)e.Series) : false || e.PrimeryCharacter != null ? lstSeriesIDs.Contains((int)e.PrimeryCharacter) : false)).ToList();
                cicvm.item.RemoveAll(e => lstSeriesItem.Contains(e));
                cicvm.item.AddRange(lstSeriesItem);
            }
            if (lstSeriesIDs.Count > 0)
            {
                List<Item> lstCharecterItem = _Context.Item.GetAll().Where(e => e.ISBN != null && e.IsInMas == true && (e.Status == statusenumB || e.Status == statusenumD) && (e.PrimeryCharacter != null ? lstSeriesIDs.Contains((int)e.PrimeryCharacter) : false)).ToList();
                cicvm.item.RemoveAll(e => lstCharecterItem.Contains(e));
                cicvm.item.AddRange(lstCharecterItem);
            }
            List<Item> lstItemByids = ilsv.GetActiveItemList().Where(e => e.ItemID.Contains(searchText)).ToList();
            if (lstItemByids.Count > 0)
            {
                cicvm.item.RemoveAll(e => lstItemByids.Contains(e));
                cicvm.item.AddRange(lstItemByids);
            }
            List<string> lstSetids = _Context.Item.GetAll(e => e.SetID == null && e.SetProfile == "Y" && e.IsInMas == true).Where(e => e.Title.ToLower().Contains(searchText.ToLower())).Select(e => e.ItemID).ToList();
            if (lstSetids.Count > 0)
            {
                List<Item> lstsetItems = null;
                foreach (string setid in lstSetids)
                {
                    lstsetItems = ilsv.GetActiveItemList().Where(e => e.SetID == Convert.ToInt32(setid)).ToList();
                    cicvm.item.RemoveAll(e => lstsetItems.Contains(e));
                    cicvm.item.AddRange(lstsetItems);
                }
            }
            cicvm.CategoriesPVM = new CategoriesPartialViewModel();
            cicvm.CategoriesPVM.SelectedTitleText = "Search Results for \"" + searchText + "\"";
            ItemContainerService itemContainerService = new ItemContainerService();
            cicvm.CategoriesPVM.pageDenomination = itemContainerService.Pagenation("60");
            cicvm.CategoriesPVM.ItemGroupVM = new ItemGroupViewModel();
            cicvm.CategoriesPVM.ItemGroupVM.ItemPVM = new ItemParentViewModel();
            cicvm.CategoriesPVM.ItemGroupVM.ItemPVM.ListItemVM = new List<ItemViewModel>();
            Quote SCQuote = _Context.Quote.GetSingle(e => e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID && e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart);
            cicvm.item.Distinct().ToList();
            cicvm.UserVM = UserVM;
            return cicvm.item;
        }
    }
}
