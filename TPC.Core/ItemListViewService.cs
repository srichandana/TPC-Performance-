using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Core.Interfaces;
using TPC.Context.Interfaces;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Common.Enumerations;
using TPC.Core.Models.ViewModels;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using TPC.Core.Models.Models;
using TPC.Core.Infrastructure;

namespace TPC.Core
{
    public class ItemListViewService : ServiceBase<IItemListViewModel>, IItemListViewService
    {
        //private IContextBase _Context;

        //public ItemListViewService()
        //    : this(new ContextBase())
        //{

        //}

        public List<Item> GetItemsByGroupCritreria(int groupID)
        {
            string displayAttribute = string.Empty;

            switch (groupID)
            {
                case 1:
                    {
                        //Entire Penworthy Collection
                        return GetItemListByGroupID(groupID);
                    }
                case 2:
                    {
                        //New THis MOnth
                        return GetItemListByGroupID(groupID);
                    }
                case 3:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 4:
                    {
                        //Readers Leveled
                        return GetItemListByGroupID(groupID);
                    }
                case 5:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 6:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 7:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 8:
                    {
                        List<Item> allArlevelItms = GetItemListByGroupID(groupID);
                        return allArlevelItms.Where(e => e.ARLevel != null && Convert.ToDouble(e.ARLevel) >= 0 && Convert.ToDouble(e.ARLevel) <= 1.5).ToList();
                    }
                case 9:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 10:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 11:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 12:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 13:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 14:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 15:
                    {
                        // Dewey Number
                        return GetItemListByGroupID(groupID).Where(e => e.Dewery != null).ToList();
                    }
                case 16:
                    {
                        // AR Level
                        return GetItemListByGroupID(groupID).Where(e => e.ARLevel != null).ToList();
                    }
                case 17:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 18:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 19:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 20:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 21:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 22:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 23:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 24:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 25:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 26:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 27:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 28:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 29:
                    {
                        return GetItemListByGroupID(groupID);
                    }
                case 30:
                    {
                        return GetItemListByGroupID(groupID);
                    }

                default:
                    {
                        return GetItemListByGroupID(groupID);
                    }
            }
        }

        public List<Item> GetItemsByPackageCriteria(int packageID)
        {
            string displayAttribute = string.Empty;
            switch (packageID)
            {
                case 8:
                    {
                        //Fiction
                        string classificationenumValue = ((char)ClassificationEnums.Fiction).ToString();
                        return GetItemsByClassification(classificationenumValue);
                    }
                case 9:
                    {
                        //Non Fiction
                        string classificationenumValue = ((char)ClassificationEnums.Nonfiction).ToString();
                        return GetItemsByClassification(classificationenumValue);
                    }
                case 10:
                    {
                        //Activity/Graphic
                        List<string> lstFormats = new List<string>();
                        lstFormats.Add(((char)BookFormatEnums.Activity).ToString());
                        lstFormats.Add(((char)BookFormatEnums.Graphic).ToString());
                        return GetItemsByMultipleFormats(lstFormats);
                    }
                case 11:
                    {
                        //Chapter && Readers Not Leveled
                        List<string> lstFormats = new List<string>();
                        lstFormats.Add(((char)BookFormatEnums.Chapter).ToString());
                        lstFormats.Add(((char)BookFormatEnums.LeveledReader).ToString());
                        return GetItemsByMultipleFormats(lstFormats);
                    }
                case 12:
                    {
                        //Picture Books
                        return GetItemsByFormet(((char)BookFormatEnums.Picture).ToString());
                    }
                case 52:
                    {
                        //Readers Leveled
                        return GetItemsByFormet(((char)BookFormatEnums.Reader).ToString());
                    }
                case 13:
                    {
                        //Preschool-Grade1
                        return GetItemsByInterestGrade(((char)InterestGradeEnums.Preschooltograde1).ToString());
                    }
                case 14:
                    {
                        //Grades 2-3
                        return GetItemsByInterestGrade(((int)InterestGradeEnums.Grade2to3).ToString());
                    }
                case 15:
                    {
                        //Grades 4+
                        return GetItemsByInterestGrade(((int)InterestGradeEnums.Grade4Above).ToString());
                    }
                case 16:
                    {
                        //Arts/Recreation/Concepts
                        List<string> lstEnums = new List<string>();
                        displayAttribute = ((DisplayAttribute)SubjectEnums.ArtRecreation.GetType().GetField(SubjectEnums.ArtRecreation.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Description;
                        string enumValue = SubjectEnums.Concept.ToString();
                        lstEnums.Add(displayAttribute);
                        lstEnums.Add(enumValue);
                        return GetItemsByMultipleSubject(lstEnums);
                    }
                case 17:
                    {
                        //History
                        string enumValue = SubjectEnums.History.ToString();
                        return GetItemsBySubject(enumValue);
                    }
                case 18:
                    {
                        //Language/Literature
                        displayAttribute = ((DisplayAttribute)SubjectEnums.LanguageLiterature.GetType().GetField(SubjectEnums.LanguageLiterature.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Description;
                        return GetItemsBySubject(displayAttribute);
                    }
                case 19:
                    {
                        //Science
                        string enumValue = SubjectEnums.Science.ToString();
                        return GetItemsBySubject(enumValue);
                    }
                case 20:
                    {
                        //Social Studies
                        displayAttribute = ((DisplayAttribute)SubjectEnums.SocialStudies.GetType().GetField(SubjectEnums.SocialStudies.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Description;
                        return GetItemsBySubject(displayAttribute);
                    }
                case 21:
                    {
                        //Character Purchased Before
                        return GetCustomerSeriesCharacterByCustomerId(SeriesCharacterEnum.PC.ToString());
                    }
                case 22:
                    {
                        //Series Purchased Before
                        return GetCustomerSeriesCharacterByCustomerId(SeriesCharacterEnum.SE.ToString());
                    }
                case 26:
                    {
                        //Hand Puppets
                        string classificationenumValue = ((char)ClassificationEnums.HandPuppet).ToString();
                        return GetItemsByClassification(classificationenumValue);
                    }
                case 27:
                    {
                        //Character Puppets
                        string classificationenumValue = ((char)ClassificationEnums.CharacterPuppet).ToString();
                        return GetItemsByClassification(classificationenumValue);
                    }
                case 28:
                    {
                        //	Finger Puppets
                        string classificationenumValue = ((char)ClassificationEnums.FingerPuppet).ToString();
                        return GetItemsByClassification(classificationenumValue);
                    }
                case 29:
                    {
                        //	Stage Puppets
                        string classificationenumValue = ((char)ClassificationEnums.StagePuppet).ToString();
                        return GetItemsByClassification(classificationenumValue);
                    }
                case 31:
                    {
                        //	0.1 to 2.0
                        return GetItemsbyARLevelRange((double)0.1, (double)2.0);
                    }
                case 32:
                    {
                        //	2.1 to 3.0
                        return GetItemsbyARLevelRange((double)2.1, (double)3.0);
                    }
                case 33:
                    {
                        //	3.1 to 4.0
                        return GetItemsbyARLevelRange((double)3.1, (double)4.0);
                    }
                case 34:
                    {
                        //	4.1 to 5.0
                        return GetItemsbyARLevelRange((double)4.1, (double)5.0);
                    }
                case 35:
                    {
                        //	5.1 and Up
                        return GetItemsbyARLevelRange((double)5.1, (double)100.0);
                    }
                case 53:
                    {
                        //	0.1 to 2.0
                        return GetItemsbyRCLevelRange((double)0.1, (double)2.0);
                    }
                case 54:
                    {
                        //	2.1 to 3.0
                        return GetItemsbyRCLevelRange((double)2.1, (double)3.0);
                    }
                case 55:
                    {
                        //	3.1 to 4.0
                        return GetItemsbyRCLevelRange((double)3.1, (double)4.0);
                    }
                case 56:
                    {
                        //	4.1 to 5.0
                        return GetItemsbyRCLevelRange((double)4.1, (double)5.0);
                    }
                case 57:
                    {
                        //	5.1 and Up
                        return GetItemsbyRCLevelRange((double)5.1, (double)100.0);
                    }
                case 58:
                    {
                        //8x8 Picture Books
                        return GetItemsByFormet(((char)BookFormatEnums.PictureBooks8).ToString());
                    }
                case 36:
                    {
                        //	000 to 399
                        return GetItemsbyLexileLevel("0", "3");
                    }
                case 37:
                    {
                        //	400 to 599
                        return GetItemsbyLexileLevel("4", "5");
                    }
                case 38:
                    {
                        //	600 to 799
                        return GetItemsbyLexileLevel("6", "7");
                    }
                case 40:
                    {
                        //	800 and up
                        return GetItemsbyLexileLevel("8", "");
                    }
                case 41:
                    {
                        //	000s
                        return GetItemsbyDeweyNumber("0");
                    }
                case 42:
                    {
                        //	100s
                        return GetItemsbyDeweyNumber("1");
                    }
                case 43:
                    {
                        //	200s
                        return GetItemsbyDeweyNumber("2");
                    }
                case 44:
                    {
                        //	300s
                        return GetItemsbyDeweyNumber("3");
                    }
                case 45:
                    {
                        //400s
                        return GetItemsbyDeweyNumber("4");
                    }
                case 46:
                    {
                        //	500s
                        return GetItemsbyDeweyNumber("5");
                    }
                case 47:
                    {
                        //	600s
                        return GetItemsbyDeweyNumber("6");
                    }
                case 48:
                    {
                        //	700s
                        return GetItemsbyDeweyNumber("7");
                    }
                case 49:
                    {
                        //	800s
                        return GetItemsbyDeweyNumber("8");
                    }
                case 50:
                    {
                        //	900s
                        return GetItemsbyDeweyNumber9andOthers("9");
                    }
                default:
                    {
                        return new List<Item>();
                    }
            }
        }

        /// <summary>
        /// This Method retuns the current Active Items from Item Table and Filter the Items whose SetID is NULL and ISBN is NULL
        /// </summary>
        /// <returns></returns>
        public List<Item> GetActiveItemList()
        {
            string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
            string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
            return _Context.Item.GetAll(e => e.ISBN != null && e.SetProfile != "Y" && e.IsInMas == true && (e.Status == statusenumB || e.Status == statusenumD)).ToList();
        }

        public List<Item> GetPreviewableItemList()
        {
            string statusenum = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
            return _Context.Item.GetAll(e => e.ISBN != null && e.IsInMas == true && e.Status != statusenum).ToList();
        }

        public bool CheckItemInMas(string itemID)
        {
            return GetActiveItemList().Where(e => e.ItemID == itemID).FirstOrDefault() != null ? true : false;
        }

        private List<Item> GetItemsByType(string productLineEnumVal)
        {
            return GetActiveItemList().Where(e => e.ProductLine == productLineEnumVal).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsByFormet(string formetVal)
        {
            return GetActiveItemList().Where(e => e.Format == formetVal).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsByMultipleFormats(List<string> lstFormatVal)
        {
            return GetActiveItemList().Where(e => lstFormatVal.Contains(e.Format)).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsByInterestGrade(string gradeVal)
        {
            return GetActiveItemList().Where(e => e.InterestGrade == gradeVal).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsByClassification(string classificationVal)
        {
            return GetActiveItemList().Where(e => e.Classification == classificationVal && e.Classification != null).OrderBy(e => e.Title).ToList();
        }
        private List<Item> GetItemsBySubject(string displayAttribute)
        {
            return GetActiveItemList().Where(e => e.Subject == displayAttribute).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsByMultipleSubject(List<string> lstdisplayAttribute)
        {
            return GetActiveItemList().Where(e => lstdisplayAttribute.Contains(e.Subject)).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemListByGroupID(int groupID)
        {
            string statusenum = Convert.ToString((char)ItemStatusEnum.NotOnListDoNotDisplay);
            List<Item> lstRawItem = GetActiveItemList();
            return _Context.GroupPackageItem.GetAll(e => e.GroupID == groupID).Where(e => lstRawItem.Contains(e.Item)).Select(e => e.Item).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsbyDeweyNumber(string deweyNo)
        {
            return GetActiveItemList().Where(e => e.Dewery != null && e.Dewery.StartsWith(deweyNo)).OrderBy(e => e.Title).ToList();
        }
        private List<Item> GetItemsbyDeweyNumber9andOthers(string deweyNo)
        {
            return GetActiveItemList().Where(e => e.Dewery != null && (e.Dewery.StartsWith(deweyNo) || e.Dewery == "E" || e.Dewery == "Fic" || e.Dewery == "B")).OrderBy(e => e.Title).ToList();
        }

        private List<Item> GetItemsbyARLevelRange(double lowRange, double highRange)
        {
            return GetActiveItemList().Where(e => e.ARLevel != null && (Convert.ToDouble(e.ARLevel) >= Convert.ToDouble(lowRange) && Convert.ToDouble(e.ARLevel) <= Convert.ToDouble(highRange))).OrderBy(e => e.Title).ToList();
        }
        private List<Item> GetItemsbyRCLevelRange(double lowRange, double highRange)
        {
            return GetActiveItemList().Where(e => e.RCLevel != null && (Convert.ToDouble(e.RCLevel) >= lowRange && Convert.ToDouble(e.RCLevel) <= highRange)).OrderBy(e => e.Title).ToList();
        }
        private List<Item> GetItemsbyLexileLevel(string lowRange, string highRange)
        {
            if (highRange != string.Empty)
            {
                return GetActiveItemList().Where(e => e.Lexile != null && (e.Lexile.StartsWith(lowRange) || e.Lexile.StartsWith(highRange))).OrderBy(e => e.Title).ToList();
            }
            else
            {
                return GetActiveItemList().Where(e => e.Lexile != null && (e.Lexile.StartsWith(lowRange))).OrderBy(e => e.Title).ToList();
            }
        }

        private List<Item> GetCustomerSeriesCharacterByCustomerId(string scType)
        {
            List<Item> lstItem = new List<Item>();
            if (UserVM != null)
            {
                //string customerBilTOShipTO = UserVM.CRMModelProperties.CustID;

                if (scType == SeriesCharacterEnum.SE.ToString())
                {
                    _Context.CustomerSeriesAndCharacter.GetAll(e =>
                        e.CusAutoID == UserVM.CRMModelProperties.CustAutoID && e.SCType == scType).OrderByDescending(e => e.SeriesAndCharacter.Items1.Count())
                        .ToList()
                        .ForEach(e => lstItem.AddRange(e.SeriesAndCharacter.Items1));
                }
                else if (scType == SeriesCharacterEnum.PC.ToString())
                {
                    _Context.CustomerSeriesAndCharacter.GetAll(e =>
                       e.CusAutoID == UserVM.CRMModelProperties.CustAutoID && e.SCType == scType).OrderByDescending(e => e.SeriesAndCharacter.Items.Count())
                       .ToList()
                       .ForEach(e => lstItem.AddRange(e.SeriesAndCharacter.Items));
                }
            }
            return lstItem;
        }

        private List<Item> GetItemsByGroupzero(int groupID)
        {
            List<Item> lstAllPackageItems = new List<Item>();
            lstAllPackageItems = _Context.GroupPackageItem.GetAll().Select(e => e.Item).OrderBy(e => e.Title).ToList();
            return lstAllPackageItems.Distinct().ToList();
        }

        public List<KPLBasedCommonViewModel> GetSingleListOfItems(int quoteDWID, string noOfItems, int pageno, string selectionStatus = "")
        {
            ItemListViewModel _itemList = new ItemListViewModel();
            _itemList = GetDWItemsList(quoteDWID, "Decision Wizard", noOfItems, pageno, selectionStatus, true); // GetListOfItems(quoteDWID, "Quote", noOfItems, pageno, selectionStatus);
            //return new List<KPLBasedCommonViewModel>();
            return _itemList.KPLItemListVM;
        }
        public Quote GetQuoteByLoggedIn(int quoteId, string type = "")
        {
            User scQuoteUser = _Context.Quote.GetSingle(e => e.QuoteID == quoteId).User;
            Quote quote = null;
            quote = scQuoteUser.Quotes.Where(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart).FirstOrDefault();
            return quote;
        }
        private List<ComboBase> Pagenation(string noofitems)
        {
            List<ComboBase> pagenamtion = new List<ComboBase>();
            if (UserVM != null && !UserVM.CRMModelProperties.IsRepLoggedIN)
            {
                pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "1", ItemValue = "View 1 title", Selected = noofitems == "0" || noofitems == "1" ? true : false });
            }
            else if (UserVM == null)
            {
                pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "1", ItemValue = "View 1 title", Selected = noofitems == "0" || noofitems == "1" ? true : false });
            }

            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "10", ItemValue = "View 10 titles", Selected = noofitems == "0" || noofitems == "10" ? true : false });
            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "20", ItemValue = "View 20 titles", Selected = noofitems == "20" ? true : false });
            pagenamtion.Add(new Infrastructure.ComboBase { ItemID = "30", ItemValue = "View 30 titles", Selected = noofitems == "30" ? true : false });
            return pagenamtion;
        }

        public string GetListItemIDsbyGroupID(int groupID, int quoteId, string selectedFilters)
        {
            List<Item> lstGroupItem = GetItemsByGroupCritreria(groupID);
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteId);
            List<Item> lstFilterItems = GetSelectedFiltersItemList(selectedFilters);
            if (lstFilterItems.Count() > 0)
            {
                lstGroupItem.RemoveAll(e => !lstFilterItems.Contains(e));
            }
            if (quote != null && quote.QuoteTypeID == (int)QuoteTypeEnum.Preview)
            {
                ItemContainerService itmContainer = new ItemContainerService();
                List<string> lstPreviewitemIDs = itmContainer.GetPreviewableItemIDs();
                lstGroupItem.RemoveAll(e => !lstPreviewitemIDs.Contains(e.ItemID));
            }
            string ItemIDS = string.Join(",", lstGroupItem.Select(e => Convert.ToString(e.ItemID)).ToList());
            return ItemIDS;
        }

        private List<Item> GetSelectedFiltersItemList(string selectedFilters)
        {
            List<Item> lstFilterItems = new List<Item>();
            string[] selectedpackges = selectedFilters.Split(',');
            foreach (string filterID in selectedpackges)
            {
                if (filterID != string.Empty)
                {
                    lstFilterItems.AddRange(GetItemsByPackageCriteria(Convert.ToInt32(filterID)));
                }
            }
            return lstFilterItems;
        }

        //old method
        public ItemListViewModel GetListOfItems(int quoteDwGroupid, string type, string noOfItems, int pageno, string selectionStatus = "")
        {
            int upperLimit = pageno == 0 ? 0 : Convert.ToInt32(noOfItems) * pageno;
            int lowerLimit = pageno == 1 ? 0 : Convert.ToInt32(noOfItems) * (pageno - 1);
            List<QuoteDetail> quoteDetailList = new List<QuoteDetail>();
            ItemListViewModel _itemList = new ItemListViewModel();
            List<Item> lstItem = null;
            int quoteID = 0;
            int selectedCount = 0;
            _itemList.KPLItemListVM = new List<KPLBasedCommonViewModel>();
            if (type == "Group")
            {

                lstItem = GetItemsByGroupCritreria(quoteDwGroupid);
                //  lstItem = SortingAlgorithim(lstItem);
                _itemList.SelectionCount = lstItem.Count;
                _itemList.GroupID = quoteDwGroupid;
                _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();
                if (_itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0)
                    _itemList.KPLItemListVM.FirstOrDefault().QuoteFlag = type;
                quoteID = UserVM.CurrentQuoteID;
            }
            else
            {
                if (quoteDwGroupid == 0 && (type == "LiveCustDW" || type == "DefaultDW"))
                {
                    quoteDwGroupid = GetDefaultDWByUserID();
                }
                quoteDetailList = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteDwGroupid).ToList();
                lstItem = quoteDetailList.Select(e => e.Item).ToList();
                //   lstItem = SortingAlgorithim(lstItem);
                _itemList.noOfYesCount =
                   quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes);
                _itemList.noOfNoCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No);
                _itemList.noOfMaybeCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe);
                _itemList.noOfNewCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New);
                _itemList.SelectionCount = _itemList.noOfYesCount + _itemList.noOfNoCount + _itemList.noOfMaybeCount + _itemList.noOfNewCount;

                _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();
                if (_itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0)
                    _itemList.KPLItemListVM.FirstOrDefault().QuoteFlag = type;
                foreach (QuoteDetail qd in quoteDetailList)
                {
                    if (qd.Item != null)
                    {

                        _itemList.KPLItemListVM.Where(e => e.ItemID == qd.ItemID).FirstOrDefault().DWSelectionStatus = qd.DWSelectionID.ToString();
                    }
                }
                _itemList.YesTotalPrice = (decimal)_itemList.KPLItemListVM.Where(e => e.DWSelectionStatus == ((int)DecisionWhizardStatusEnum.Yes).ToString()).Sum(e => e.Price);
                quoteID = quoteDwGroupid;
            }
            UserVM.CurrentQuoteID = quoteID;
            QuoteViewService quoteviewService = new QuoteViewService();
            _itemList.QuoteTitle = quoteviewService.getQuoteTitleText(quoteID);
            _itemList.QuoteType = quoteviewService.getQuoteTypeText(quoteID);
            _itemList.QuoteID = quoteID;
            Quote scQuote = GetQuoteByLoggedIn(quoteID, type);
            int quoteTypeID = quoteviewService.getQuoteTypeId(scQuote.QuoteID);
            List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();
            foreach (KPLBasedCommonViewModel Item in _itemList.KPLItemListVM)
            {
                string SetID = lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID == null ? "0" : lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID.ToString();
                if (SetID != "0")
                {
                    string statusenum = Convert.ToString((char)ItemStatusEnum.NotOnListDoNotDisplay);
                    Item setItem = _Context.Item.GetSingle(e => e.ItemID == SetID);
                    string setName = string.Empty;
                    string setDescription = string.Empty;
                    if (setItem != null)
                    {
                        setName = setItem.Title;
                        setDescription = setItem.Description;
                    }

                    List<Item> itemListAll = GetActiveItemList().Where(e => e.SetID == Convert.ToInt32(SetID)).ToList();
                    Item.ItemListGVM = new ItemGroupViewModel
                    {
                        GroupName = setName,
                        SetDescription = setDescription,
                        GroupItemCount = itemListAll.Count(),
                        ItemPVM = new ItemParentViewModel
                        {
                            ListItemVM = itemListAll.Select(c => new ItemViewModel
                            {
                                ItemID = c.ItemID,
                                ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                                Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                                Description = (c.Description == null ? string.Empty : c.Description),
                                IsSelected = (c.ItemID == Item.ItemID ? true : false),
                                Title = (c.Title == null ? string.Empty : c.Title),
                                IsInSCDWQuote = lstSCQuoteDetails.Any(x => x.ItemID == c.ItemID &&
                                        x.DWSelectionID == (quoteTypeID == (int)QuoteTypeEnum.ShoppingCart ? (int)DecisionWhizardStatusEnum.Yes :
                                        quoteTypeID == (int)QuoteTypeEnum.DecisionWhizard ? (int)lstSCQuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID :
                                        (int)DecisionWhizardStatusEnum.New)) == false ? false : true,
                                QuoteTypeText = scQuote.QuoteType.QuoteTypeText,
                                QuoteFlag = type,
                                IPrice = Convert.ToDouble((decimal)c.Price)
                            }).OrderByDescending(c => c.IsSelected).ToList()
                        }
                    };
                }
            }

            //Selection count is used to display items according to 2 or more checked checkboxes(yes,no,maybe,all)
            if (!string.IsNullOrEmpty(selectionStatus))
            {
                string[] selectionIDs = selectionStatus.Split(',');
                foreach (string id in selectionIDs)
                {
                    if (id == "1")
                    {
                        selectedCount = selectedCount + _itemList.noOfYesCount;
                    }
                    if (id == "2")
                    {
                        selectedCount = selectedCount + _itemList.noOfNoCount;
                    }
                    if (id == "3")
                    {
                        selectedCount = selectedCount + _itemList.noOfMaybeCount;
                    }
                    if (id == "5")
                    {
                        selectedCount = selectedCount + _itemList.noOfNewCount;
                    }

                }

                _itemList.SelectionCount = selectedCount;
            }



            //this call is regarding user selection for yes/NO /May be 
            if (!string.IsNullOrEmpty(selectionStatus))
            {
                PharseYesMayBeNo(_itemList, selectionStatus);

            }

            //   _itemList.NoOfListItems = lstItem.Count;
            if (_itemList.KPLItemListVM.Count > lowerLimit)
            {
                _itemList.KPLItemListVM.RemoveRange(0, lowerLimit);
            }
            if (_itemList.KPLItemListVM.Count > upperLimit)
            {
                _itemList.KPLItemListVM.RemoveRange(upperLimit - lowerLimit, _itemList.KPLItemListVM.Count - (upperLimit - lowerLimit));
            }
            if (_itemList.KPLItemListVM.Count > (upperLimit - lowerLimit) && _itemList.KPLItemListVM.Count != (upperLimit - lowerLimit))
            {
                _itemList.KPLItemListVM = _itemList.KPLItemListVM.GetRange(0, Convert.ToInt32(noOfItems)).ToList();
            }

            _itemList.pageDenomination = new List<Infrastructure.ComboBase>();
            _itemList.pageDenomination = Pagenation(noOfItems);


            _itemList.currentPageIndex = pageno;
            _itemList.UserVM = UserVM;
            return _itemList;
        }

        public ItemListViewModel GetDWItemsList(int quoteDwGroupid, string type, string noOfItems, int pageno, string selectionStatus = "", bool isSingle = false)
        {
            ItemContainerService _itemConatinerSrv = new ItemContainerService();
            ItemListViewModel _itemList = new ItemListViewModel();
            _itemList.ItemGroupViewM = new ItemGroupViewModel();
            _itemList.KPLItemListVM = new List<KPLBasedCommonViewModel>();
            _itemConatinerSrv.UserVM = UserVM;
            List<QuoteDetail> quoteDetailList = new List<QuoteDetail>();
            List<Item> lstItem = null;
            int quoteID = 0;
            int selectedCount = 0;
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();

            List<string> lstTitlesBroughtBeforeItemIDs = _itemConatinerSrv.GetTitlesBroughtBeforeItemIDs();
            if (UserVM != null && UserVM.CRMModelProperties != null)
            {

                lstSeriesBroughtBeforeItemIDs = _itemConatinerSrv.GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
                lstCharacterBroughtBeforeItemIDs = _itemConatinerSrv.GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            }
            if (type == "Group")
            {
                lstItem = GetItemsByGroupCritreria(quoteDwGroupid);
                if (lstItem != null && lstItem.Count() > 0)
                {
                    lstItem = lstItem.OrderBy(e => e.Title).OrderBy(e => e.SetID).ToList();
                }
                _itemList.SelectionCount = lstItem.Count;
                _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();
                if (_itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0)
                    _itemList.KPLItemListVM.FirstOrDefault().QuoteFlag = type;
                _itemList.GroupID = quoteDwGroupid;
                quoteID = UserVM.CurrentQuoteID;
            }
            else
            {
                if (quoteDwGroupid == 0 && (type == "LiveCustDW" || type == "DefaultDW"))
                {
                    quoteDwGroupid = GetDefaultDWByUserID();
                }
                quoteDetailList = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteDwGroupid).ToList();
                lstItem = quoteDetailList.Select(e => e.Item).ToList();

                List<Item> lstinOrderBy = new List<Item>();

                // m = groupVM.ItemPVM.ListItemVM;
                lstinOrderBy.AddRange(lstItem.Where(e => e.ProductLine.Trim() == "PPB").OrderBy(e => e.Series == null).ThenBy(c => c.SeriesAndCharacter1 != null ? c.SeriesAndCharacter1.SCText : string.Empty).ThenBy(e => e.Title).ToList());
                lstinOrderBy.AddRange(lstItem.Where(e => e.ProductLine.Trim() == "LBY").OrderBy(e => e.Series == null).ThenBy(c => c.SeriesAndCharacter1 != null ? c.SeriesAndCharacter1.SCText : string.Empty).ThenBy(e => e.Title).ToList());
                lstinOrderBy.AddRange(lstItem.Where(e => e.ProductLine.Trim() == "BB").OrderBy(e => e.Series == null).ThenBy(c => c.SeriesAndCharacter1 != null ? c.SeriesAndCharacter1.SCText : string.Empty).ThenBy(e => e.Title).ToList());
                lstinOrderBy.AddRange(lstItem.Where(e => e.ProductLine.Trim() == "PUP").OrderBy(e => e.Series == null).ThenBy(c => c.SeriesAndCharacter1 != null ? c.SeriesAndCharacter1.SCText : string.Empty).ThenBy(e => e.Title).ToList());
                List<Item> lstexceptbounds = lstItem.Where(e => !lstinOrderBy.Contains(e)).ToList();//; e.ProductLine.Trim() != "PPB" && e.ProductLine.Trim() != "LBY" && e.ProductLine.Trim() != "PUP").ToList();
                lstinOrderBy.AddRange(lstexceptbounds.OrderBy(e => e.Series == null).ThenBy(c => c.SeriesAndCharacter1 != null ? c.SeriesAndCharacter1.SCText : string.Empty).ThenBy(e => e.Title));
                lstItem = lstinOrderBy;
                //lstItem = quoteDetailList.Select(e => e.Item).OrderBy(e => e.SetID).ToList();
                _itemList.noOfYesCount =
                      quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes);
                _itemList.noOfNoCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No);
                _itemList.noOfMaybeCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe);
                _itemList.noOfNewCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New);
                _itemList.SelectionCount = _itemList.noOfYesCount + _itemList.noOfNoCount + _itemList.noOfMaybeCount + _itemList.noOfNewCount;
                _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();

                _itemList.KPLItemListVM.Where(e => lstTitlesBroughtBeforeItemIDs.Contains(e.ItemID)).ToList().ForEach(e => e.IsInCustomerTitles = true);
                _itemList.KPLItemListVM.Where(e => lstCharacterBroughtBeforeItemIDs.Contains(e.ItemID)).ToList().ForEach(e => e.CharecterBroughtBefore = true);
                _itemList.KPLItemListVM.Where(e => lstSeriesBroughtBeforeItemIDs.Contains(e.ItemID)).ToList().ForEach(e => e.SeriesBroughtBefore = true);

                if (_itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0)
                    _itemList.KPLItemListVM.FirstOrDefault().QuoteFlag = type;
                foreach (QuoteDetail qd in quoteDetailList)
                {
                    if (qd.Item != null)
                    {
                        _itemList.KPLItemListVM.Where(e => e.ItemID == qd.ItemID).FirstOrDefault().DWSelectionStatus = qd.DWSelectionID.ToString();
                    }
                }
                _itemList.YesTotalPrice = _itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0 ? (decimal)_itemList.KPLItemListVM.Where(e => e.DWSelectionStatus == ((int)DecisionWhizardStatusEnum.Yes).ToString()).Sum(e => e.Price) : (decimal)0;
                quoteID = quoteDwGroupid;
            }
            UserVM.CurrentQuoteID = quoteID;
            QuoteViewService quoteviewService = new QuoteViewService();
            quoteviewService.UserVM = UserVM;
            _itemList.QuoteTitle = quoteviewService.getQuoteTitleText(quoteID);
            _itemList.QuoteType = quoteviewService.getQuoteTypeText(quoteID);
            _itemList.QuoteID = quoteID;

            _itemList.ItemGroupViewM = FillItembyQuoteID(quoteID, lstItem, selectionStatus);
            string[] selectionIDs = selectionStatus.Split(',');
            if (selectionStatus != string.Empty && selectionIDs.Length == 1 && _itemList.ItemGroupViewM.ItemPVM != null && _itemList.ItemGroupViewM.ItemPVM.ListItemVM.Count() > 0)
            {
                _itemList.ItemGroupViewM.ItemPVM.ListItemVM.RemoveAll(e => e.DWSelectionStatus != selectionStatus);
            }

            if (isSingle)
            {
                if (_itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0)
                {
                    if (selectionStatus != string.Empty && selectionIDs.Length == 1)
                    {
                        _itemList.KPLItemListVM.RemoveAll(e => e.DWSelectionStatus != selectionStatus);
                    }
                }
                int upperLimit = pageno == 0 ? 0 : Convert.ToInt32(noOfItems) * pageno;
                int lowerLimit = pageno == 1 ? 0 : Convert.ToInt32(noOfItems) * (pageno - 1);

                if (_itemList.KPLItemListVM.Count > lowerLimit)
                {
                    _itemList.KPLItemListVM.RemoveRange(0, lowerLimit);
                }
                if (_itemList.KPLItemListVM.Count > upperLimit)
                {
                    _itemList.KPLItemListVM.RemoveRange(upperLimit - lowerLimit, _itemList.KPLItemListVM.Count - (upperLimit - lowerLimit));
                }
                if (_itemList.KPLItemListVM.Count > (upperLimit - lowerLimit) && _itemList.KPLItemListVM.Count != (upperLimit - lowerLimit))
                {
                    _itemList.KPLItemListVM = _itemList.KPLItemListVM.GetRange(0, Convert.ToInt32(noOfItems)).ToList();
                }

                Quote scQuote = GetQuoteByLoggedIn(quoteID, type);
                Quote currentQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
                int quoteTypeID = quoteviewService.getQuoteTypeId(scQuote.QuoteID);
                List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();
                foreach (KPLBasedCommonViewModel Item in _itemList.KPLItemListVM)
                {
                    string SetID = lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID == null ? "0" : lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID.ToString();
                    if (SetID != "0")
                    {
                        string statusenum = Convert.ToString((char)ItemStatusEnum.NotOnListDoNotDisplay);
                        Item setItem = _Context.Item.GetSingle(e => e.ItemID == SetID);
                        string setName = string.Empty;
                        string setDescription = string.Empty;
                        if (setItem != null)
                        {
                            setName = setItem.Title;
                            setDescription = setItem.Description;
                        }

                        List<Item> itemListAll = GetActiveItemList().Where(e => e.SetID == Convert.ToInt32(SetID)).ToList();
                        Item.ItemListGVM = new ItemGroupViewModel
                        {
                            GroupName = setName,
                            SetDescription = setDescription,
                            GroupItemCount = itemListAll.Count(),
                            ItemPVM = new ItemParentViewModel
                            {
                                ListItemVM = itemListAll.Select(c => new ItemViewModel
                                {
                                    ItemID = c.ItemID,
                                    ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                                    Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                                    Description = (c.Description == null ? string.Empty : c.Description),
                                    IsSelected = (c.ItemID == Item.ItemID ? true : false),
                                    Title = (c.Title == null ? string.Empty : c.Title),
                                    IsInSCDWQuote = lstSCQuoteDetails.Any(x => x.ItemID == c.ItemID &&
                                            x.DWSelectionID == (quoteTypeID == (int)QuoteTypeEnum.ShoppingCart ? (int)DecisionWhizardStatusEnum.Yes :
                                            quoteTypeID == (int)QuoteTypeEnum.DecisionWhizard ? (int)lstSCQuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID :
                                            (int)DecisionWhizardStatusEnum.New)) == false ? false : true,
                                    QuoteTypeText = scQuote.QuoteType.QuoteTypeText,
                                    QuoteFlag = type,
                                    IPrice = Convert.ToDouble((decimal)c.Price),
                                    IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                                    SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                                    CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                                    DWSelectionStatus = currentQuote.QuoteDetails.Where(e => e.ItemID == c.ItemID).FirstOrDefault() != null ? currentQuote.QuoteDetails.Where(e => e.ItemID == c.ItemID).FirstOrDefault().DWSelectionID.ToString() : ((int)DecisionWhizardStatusEnum.New).ToString(),
                                }).OrderByDescending(c => c.IsSelected).ToList()
                            }
                        };
                    }
                }
            }
            //Selection count is used to display items according to 2 or more checked checkboxes(yes,no,maybe,all)

            if (!string.IsNullOrEmpty(selectionStatus))
            {
                foreach (string id in selectionIDs)
                {
                    if (id == "1")
                    {
                        selectedCount = selectedCount + _itemList.noOfYesCount;
                    }
                    if (id == "2")
                    {
                        selectedCount = selectedCount + _itemList.noOfNoCount;
                    }
                    if (id == "3")
                    {
                        selectedCount = selectedCount + _itemList.noOfMaybeCount;
                    }
                    if (id == "5")
                    {
                        selectedCount = selectedCount + _itemList.noOfNewCount;
                    }
                }
                _itemList.SelectionCount = selectedCount;
            }

            //this call is regarding user selection for yes/NO /May be 
            if (!string.IsNullOrEmpty(selectionStatus))
            {
                PharseYesMayBeNo(_itemList, selectionStatus);
            }
            _itemList.UserVM = UserVM;
            return _itemList;
        }

        public ItemGroupViewModel FillItembyQuoteID(int quoteID, List<Item> lstiVM, string selectionStatus)
        {
            ItemContainerService containersrvc = new ItemContainerService();
            containersrvc.UserVM = UserVM;
          
            ItemListViewService itemListviewsrv = new ItemListViewService();
            ItemGroupViewModel groupVM = new ItemGroupViewModel();
            List<string> lstSeriesBroughtBeforeItemIDs = new List<string>();
            List<string> lstCharacterBroughtBeforeItemIDs = new List<string>();
            Quote scQuote = null;
            Quote currentQuote = null;
            List<QuoteDetail> lstSCQuoteDetails = null;
            int quoteTypeID = 0;
            if (quoteID != 0)
            {
                scQuote = GetQuoteByLoggedIn(quoteID);// _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
                currentQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
                lstSCQuoteDetails = scQuote.QuoteDetails.ToList();
                quoteTypeID = (int)scQuote.QuoteTypeID;
            }

            itemListviewsrv.UserVM = UserVM;
            List<string> lstTitlesBroughtBeforeItemIDs = containersrvc.GetTitlesBroughtBeforeItemIDs();
            if (UserVM != null && UserVM.CRMModelProperties != null)
            {
                lstSeriesBroughtBeforeItemIDs = containersrvc.GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "SE");
                lstCharacterBroughtBeforeItemIDs = containersrvc.GetListSeriesCharacterbyCustomerID(UserVM.CRMModelProperties.CustAutoID, "PC");
            }

            if (lstiVM.Count() != 0)
            {
                groupVM = new ItemGroupViewModel
                {
                    GroupItemCount = lstiVM.Count(),
                    ItemPVM = new ItemParentViewModel
                    {
                        ListItemVM = lstiVM.Select(c => new ItemViewModel
                        {
                            ItemID = c.ItemID,
                            ProductLine = c.ProductLine,
                            ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
                            Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
                            Title = (c.Title == null ? string.Empty : c.Title),
                            IsSelected = false,
                            IsInSCDWQuote = scQuote != null ? (scQuote.QuoteDetails.Any(x =>
                                x.ItemID == c.ItemID && x.DWSelectionID == (scQuote.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ?
                                (int)DecisionWhizardStatusEnum.Yes : (scQuote.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard || scQuote.QuoteTypeID == (int)QuoteTypeEnum.Web) ?
                                (int)scQuote.QuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID : (int)DecisionWhizardStatusEnum.New)) == false ? false : true) : false,
                            QuoteTypeText = scQuote != null ? scQuote.QuoteType.QuoteTypeText : string.Empty,
                            IPrice = (double)c.Price,
                            ARLevel = string.IsNullOrEmpty(c.ARLevel) ? "" : c.ARLevel != null ? "AR:" + c.ARLevel : string.Empty,
                            RCLevel = string.IsNullOrEmpty(c.RCLevel) ? "" : c.RCLevel != null ? c.ARLevel != null ? ",RC:" + c.RCLevel : "RC:" + c.RCLevel : string.Empty,
                            ClassificationType = c.Classification == "F" ? "Fic" : c.Classification == "N" ? "NF" : string.Empty,
                            strCopyRight = c.Copyright.ToString(),
                            IsInCustomerTitles = lstTitlesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            SeriesBroughtBefore = lstSeriesBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            CharecterBroughtBefore = lstCharacterBroughtBeforeItemIDs.Contains(c.ItemID) ? true : false,
                            DWSelectionStatus = currentQuote.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart ? null : currentQuote.QuoteDetails.Where(e => e.ItemID == c.ItemID).FirstOrDefault() != null ? currentQuote.QuoteDetails.Where(e => e.ItemID == c.ItemID).FirstOrDefault().DWSelectionID.ToString() : ((int)DecisionWhizardStatusEnum.New).ToString(),
                        }).ToList()
                    },

                };
            }

            return groupVM;
        }

        //new method
        //public ItemListViewModel GetListOfItems(int quoteDwGroupid, string type, string noOfItems, int pageno, string selectionStatus = "")
        //{
        //    int upperLimit = pageno == 0 ? 0 : Convert.ToInt32(noOfItems) * pageno;
        //    int lowerLimit = pageno == 1 ? 0 : Convert.ToInt32(noOfItems) * (pageno - 1);
        //    List<QuoteDetail> quoteDetailList = new List<QuoteDetail>();
        //    ItemListViewModel _itemList = new ItemListViewModel();
        //    List<Item> lstItem = null;
        //    int quoteID = 0;
        //    int selectedCount = 0;
        //    _itemList.KPLItemListVM = new List<KPLBasedCommonViewModel>();
        //    if (type == "Group")
        //    {
        //        lstItem = GetItemsByGroupCritreria(quoteDwGroupid);
        //        _itemList.SelectionCount = lstItem.Count;
        //        _itemList.GroupID = quoteDwGroupid;
        //        quoteID = UserVM.CurrentQuoteID;
        //    }
        //    else
        //    {
        //        if (quoteDwGroupid == 0 && (type == "LiveCustDW" || type == "DefaultDW"))
        //        {
        //            quoteDwGroupid = GetDefaultDWByUserID();
        //        }
        //        quoteDetailList = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteDwGroupid).ToList();
        //        lstItem = quoteDetailList.Select(e => e.Item).ToList();
        //        _itemList.noOfYesCount =
        //           quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes);
        //        _itemList.noOfNoCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No);
        //        _itemList.noOfMaybeCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe);
        //        _itemList.noOfNewCount = quoteDetailList.Count(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New);
        //        _itemList.SelectionCount = _itemList.noOfYesCount + _itemList.noOfNoCount + _itemList.noOfMaybeCount + _itemList.noOfNewCount;
        //        _itemList.YesTotalPrice = (decimal)quoteDetailList.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(e => e.Item.Price);
        //        quoteID = quoteDwGroupid;
        //    }
        //    //this call is regarding user selection for yes/NO /May be 
        //    if (!string.IsNullOrEmpty(selectionStatus))
        //    {
        //        List<string> afterPharseKPL = new List<string>();
        //        int[] selectionIDs = selectionStatus.Split(',').Select(e => Convert.ToInt32(e)).ToArray();
        //        foreach (int id in selectionIDs)
        //        {
        //            afterPharseKPL.AddRange(quoteDetailList.Where(e => e.DWSelectionID == id).Select(e => e.ItemID).ToList());
        //        }
        //        lstItem = lstItem.Where(e => afterPharseKPL.Contains(e.ItemID)).ToList();

        //    }
        //    //getting current list
        //    if (lstItem.Count > Convert.ToInt32(noOfItems))
        //    {
        //        if (!string.IsNullOrEmpty(selectionStatus) && selectionStatus.Contains(','))
        //        {
        //            lstItem = lstItem.Skip(lowerLimit).Take(Convert.ToInt32(noOfItems)).ToList();
        //        }
        //        else
        //        {
        //            lstItem = lstItem.Take(Convert.ToInt32(noOfItems)).ToList();
        //        }
        //    }
        //    _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();
        //    if (_itemList.KPLItemListVM != null && _itemList.KPLItemListVM.Count() > 0)
        //        _itemList.KPLItemListVM.FirstOrDefault().QuoteFlag = type;
        //    if (type != "Group")
        //    {
        //        quoteDetailList.RemoveAll(e => !lstItem.Select(i => i.ItemID).Contains(e.ItemID));
        //        foreach (QuoteDetail qd in quoteDetailList)
        //        {
        //            if (qd.Item != null)
        //            {
        //                _itemList.KPLItemListVM.Where(e => e.ItemID == qd.ItemID).FirstOrDefault().DWSelectionStatus = qd.DWSelectionID.ToString();
        //            }
        //        }
        //    }
        //    if (UserVM != null)
        //        UserVM.CurrentQuoteID = quoteID;
        //    QuoteViewService quoteviewService = new QuoteViewService();
        //    _itemList.QuoteTitle = quoteviewService.getQuoteTitleText(quoteID);
        //    _itemList.QuoteType = quoteviewService.getQuoteTypeText(quoteID);
        //    _itemList.QuoteID = quoteID;
        //    Quote scQuote = GetQuoteByLoggedIn(quoteID, type);
        //    int quoteTypeID = quoteviewService.getQuoteTypeId(scQuote.QuoteID);
        //    List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();

        //    foreach (KPLBasedCommonViewModel Item in _itemList.KPLItemListVM)
        //    {
        //        string SetID = lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID == null ? "0" : lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID.ToString();
        //        if (SetID != "0")
        //        {
        //            Item setItem = _Context.Item.GetSingle(e => e.ItemID == SetID);
        //            string setName = string.Empty;
        //            string setDescription = string.Empty;
        //            if (setItem != null)
        //            {
        //                setName = setItem.Title;
        //                setDescription = setItem.Description;
        //            }

        //            List<Item> itemListAll = GetActiveItemList().Where(e => e.SetID == Convert.ToInt32(SetID)).ToList();
        //            Item.ItemListGVM = new ItemGroupViewModel
        //            {
        //                GroupName = setName,
        //                GroupItemCount = itemListAll.Count(),
        //                SetDescription = setDescription,
        //                ItemPVM = new ItemParentViewModel
        //                {
        //                    ListItemVM = itemListAll.Select(c => new ItemViewModel
        //                    {
        //                        ItemID = c.ItemID,
        //                        ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
        //                        Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
        //                        Description = (c.Description == null ? string.Empty : c.Description),
        //                        IsSelected = (c.ItemID == Item.ItemID ? true : false),
        //                        Title = (c.Title == null ? string.Empty : c.Title),
        //                        IsInSCDWQuote = lstSCQuoteDetails.Any(x => x.ItemID == c.ItemID &&
        //                                x.DWSelectionID == (quoteTypeID == (int)QuoteTypeEnum.ShoppingCart ? (int)DecisionWhizardStatusEnum.Yes :
        //                                quoteTypeID == (int)QuoteTypeEnum.DecisionWhizard ? (int)lstSCQuoteDetails.Where(g => g.ItemID == x.ItemID).FirstOrDefault().DWSelectionID :
        //                                (int)DecisionWhizardStatusEnum.New)) == false ? false : true,
        //                        QuoteTypeText = scQuote.QuoteType.QuoteTypeText,
        //                        QuoteFlag = type,
        //                        IPrice = Convert.ToDouble((decimal)c.Price)
        //                    }).OrderByDescending(c => c.IsSelected).ToList()
        //                }
        //            };
        //        }
        //    }

        //    //Selection count is used to display items according to 2 or more checked checkboxes(yes,no,maybe,all)
        //    if (!string.IsNullOrEmpty(selectionStatus))
        //    {
        //        string[] selectionIDs = selectionStatus.Split(',');
        //        foreach (string id in selectionIDs)
        //        {
        //            if (id == "1")
        //            {
        //                selectedCount = selectedCount + _itemList.noOfYesCount;
        //            }
        //            if (id == "2")
        //            {
        //                selectedCount = selectedCount + _itemList.noOfNoCount;
        //            }
        //            if (id == "3")
        //            {
        //                selectedCount = selectedCount + _itemList.noOfMaybeCount;
        //            }
        //            if (id == "5")
        //            {
        //                selectedCount = selectedCount + _itemList.noOfNewCount;
        //            }

        //        }

        //        _itemList.SelectionCount = selectedCount;
        //    }

        //    _itemList.pageDenomination = new List<Infrastructure.ComboBase>();
        //    _itemList.pageDenomination = Pagenation(noOfItems);
        //    _itemList.currentPageIndex = pageno;
        //    _itemList.UserVM = UserVM;
        //    return _itemList;
        //}
        public ItemListViewModel GetItemListDetailByClientID(int custUserID)
        {
            Quote quote = _Context.Quote.GetSingle(e => e.UserID == custUserID && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == (int)QuoteStatusEnum.Open);

            return GetListOfItems(quote.QuoteID, "Quote", "30", 1, "");
            //new ItemListViewModel(); //GetListOfItems(quote.QuoteID,"Quote", 10, 1);
        }


        private ItemListViewModel PharseYesMayBeNo(ItemListViewModel _itemList, string selectionStatus)
        {
            List<KPLBasedCommonViewModel> afterPharseKPL = new List<KPLBasedCommonViewModel>();
            string[] selectionIDs = selectionStatus.Split(',');

            foreach (string id in selectionIDs)
            {

                afterPharseKPL.AddRange(_itemList.KPLItemListVM.Where(e => e.DWSelectionStatus == id.ToString()).ToList());

            }
            _itemList.KPLItemListVM = afterPharseKPL;
            return _itemList;

        }

        public ItemListViewModel GetListOfItemsBySelection(int custUserID, string selectionStatus, int ddlSelectedValue, int pgno)
        {

            Quote quote = _Context.Quote.GetSingle(e => e.UserID == custUserID && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard);

            return new ItemListViewModel(); //GetListOfItems(quote.QuoteID,"Quote", ddlSelectedValue, pgno, selectionStatus);

        }

        //public ItemListViewModel GetListOfAllItems(int quoteDWID, string noOfItems, int pageno, string selectionStatus = "")
        //{
        //    int upperLimit = pageno == 0 ? 0 : noOfItems * pageno;

        //    int lowerLimit = pageno == 1 ? 0 : noOfItems * (pageno - 1);
        //    List<QuoteDetail> quoteDetailList = new List<QuoteDetail>();

        //    ItemListViewModel _itemList = new ItemListViewModel();

        //    try
        //    {
        //        _itemList.KPLItemListVM = new List<KPLBasedCommonViewModel>();
        //        quoteDetailList = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteDWID).ToList();
        //        List<Item> lstItem = quoteDetailList.Select(e => e.Item).ToList();
        //        lstItem = SortingAlgorithim(lstItem);
        //        _itemList.KPLItemListVM = AutoMapper.Mapper.Map<IList<Item>, IList<KPLBasedCommonViewModel>>(lstItem).ToList();
        //        _itemList.noOfYesCount = quoteDetailList.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Count();
        //        _itemList.noOfNoCount = quoteDetailList.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).Count();
        //        _itemList.noOfMaybeCount = quoteDetailList.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe).Count();
        //        _itemList.noOfNewCount = quoteDetailList.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New).Count();
        //        _itemList.SelectionCount = _itemList.noOfYesCount + _itemList.noOfNoCount + _itemList.noOfMaybeCount + _itemList.noOfNewCount;
        //        int selectedCount = 0;
        //        Quote scQuote = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
        //        List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();

        //        foreach (KPLBasedCommonViewModel Item in _itemList.KPLItemListVM)
        //        {
        //            int SetID = (int)lstItem.Where(e => e.ItemID == Item.ItemID).FirstOrDefault().SetID;
        //            List<Item> itemListAll = GetActiveItemList().Where(e => e.SetID == SetID).OrderBy(e => e.Title).ToList();

        //            Item.ItemListGVM = new ItemGroupViewModel
        //            {
        //                GroupName = itemListAll.FirstOrDefault().SetName,
        //                GroupItemCount = itemListAll.Count(),
        //                ItemPVM = new ItemParentViewModel
        //                {
        //                    ListItemVM = itemListAll.Select(c => new ItemViewModel
        //                    {
        //                        ItemID = c.ItemID,
        //                        ISBN = (c.ISBN == null ? string.Empty : c.ISBN),
        //                        Barcode = (c.Barcode == null ? string.Empty : c.Barcode),
        //                        Description = (c.Description == null ? string.Empty : c.Description),
        //                        IsSelected = (c.ItemID == Item.ItemID ? true : false),
        //                        Title = (c.Title == null ? string.Empty : c.Title),
        //                        IsInSCDWQuote = lstSCQuoteDetails.Any(x => x.ItemID == c.ItemID) == false ? false : true,
        //                    }).OrderByDescending(c => c.IsSelected).ToList()
        //                }
        //            };
        //        }


        //        //Selection count is used to display items according to 2 or more checked checkboxes(yes,no,maybe,all)
        //        if (!string.IsNullOrEmpty(selectionStatus))
        //        {
        //            string[] selectionIDs = selectionStatus.Split(',');
        //            foreach (string id in selectionIDs)
        //            {
        //                if (id == "1")
        //                {
        //                    selectedCount = selectedCount + _itemList.noOfYesCount;
        //                }
        //                if (id == "2")
        //                {
        //                    selectedCount = selectedCount + _itemList.noOfNoCount;
        //                }
        //                if (id == "3")
        //                {
        //                    selectedCount = selectedCount + _itemList.noOfMaybeCount;
        //                }
        //                if (id == "5")
        //                {
        //                    selectedCount = selectedCount + _itemList.noOfNewCount;
        //                }

        //            }

        //            _itemList.SelectionCount = selectedCount;
        //        }

        //        foreach (QuoteDetail qd in quoteDetailList)
        //        {
        //            if (qd.Item != null)
        //            {

        //                _itemList.KPLItemListVM.Where(e => e.ItemID == qd.ItemID).FirstOrDefault().DWSelectionStatus = qd.DWSelectionID.ToString();
        //            }
        //        }

        //        //this call is regardinguser selection for yes/NO /May be 
        //        if (!string.IsNullOrEmpty(selectionStatus))
        //        {
        //            PharseYesMayBeNo(_itemList, selectionStatus);

        //        }

        //        _itemList.pageDenomination = new List<Infrastructure.ComboBase>();
        //        _itemList.pageDenomination = Pagenation(noOfItems);

        //        _itemList.QuoteID = quoteDWID;
        //        _itemList.currentPageIndex = pageno;

        //        QuoteViewService quoteviewService = new QuoteViewService();
        //        _itemList.QuoteTitle = quoteviewService.getQuoteTitleText(quoteDWID);
        //        _itemList.QuoteType = quoteviewService.getQuoteTypeText(quoteDWID);
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return _itemList;
        //}

        //public ItemListViewModel GetAllItemListDetailByClientID(int custUserId)
        //{
        //    Quote quote = _Context.Quote.GetSingle(e => e.UserID == custUserId && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard);
        //    return GetListOfAllItems(quote.QuoteID, 30, 1);
        //}

        public ItemListViewModel GetCustomerListOfItems(int quoteDWID)
        {
            return new ItemListViewModel();
        }

        public ItemListViewService()
        {


        }

        public void UpdatedDateTime(int quoteID)
         {
            Quote currentQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            if (currentQuote != null)
            {
                if (UserVM != null && UserVM.CRMModelProperties.IsRepLoggedIN) { currentQuote.PenworthyUpdatedDate = System.DateTime.UtcNow; }
                else
                {
                    currentQuote.CustomerUpdatedDate = System.DateTime.UtcNow;
                }
                currentQuote.UpdateDate = System.DateTime.UtcNow;
            }

            _Context.Quote.SaveChanges();
        }

        public ItemListViewModel UpdateDW(KPLBasedCommonViewModel itemViewModel, string selectionStatus, string ddlSelectedValue, int pgno)
        {
            #region Private Variables
            int currentDWStatus = Convert.ToInt32(itemViewModel.DWSelectionStatus);
            int DWYes = (int)DecisionWhizardStatusEnum.Yes;
            int DWNo = (int)DecisionWhizardStatusEnum.No;
            int DWMayBe = (int)DecisionWhizardStatusEnum.MayBe;
            int DWYesToAll = (int)DecisionWhizardStatusEnum.YesToAll;

            #endregion
            QuoteDetail dwquoteDetail = null;
            QuoteDetail scQuoteDetail = null;
            List<QuoteDetail> lstDWquotedetails = null;
            ItemService itemService = new ItemService();
            //User quoteUser = _Context.Quote.GetSingle(e => e.QuoteID == itemViewModel.QuoteID).User;
            int quoteID = itemViewModel.QuoteID;
            Quote scQuote = GetQuoteByLoggedIn(itemViewModel.QuoteID);
            // quoteUser.Quotes.Where(e => e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart).FirstOrDefault();
            List<QuoteDetail> lstSCQuoteDetails = scQuote.QuoteDetails.ToList();
            if (lstSCQuoteDetails != null)//item is in shoppingcart
            {
                scQuoteDetail = lstSCQuoteDetails.Where(e => e.ItemID == itemViewModel.ItemID).FirstOrDefault();
                dwquoteDetail = _Context.QuoteDetail.GetSingle(e => e.QuoteID == itemViewModel.QuoteID && e.ItemID == itemViewModel.ItemID);
                lstDWquotedetails = _Context.QuoteDetail.GetAll(e => e.QuoteID == itemViewModel.QuoteID).ToList();
                if (scQuoteDetail != null)
                {
                    if (currentDWStatus == DWNo || currentDWStatus == DWMayBe)
                    {
                        //delete from sc
                        UpdatedDateTime((int)scQuoteDetail.QuoteID);
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
                            UpdateDate = DateTime.Now,

                        });
                        UpdatedDateTime(scQuote.QuoteID);
                    }
                }
                if (currentDWStatus == DWYesToAll)
                {
                    Item item = _Context.Item.GetSingle(e => e.ItemID == itemViewModel.ItemID);
                    List<Item> setItems = GetActiveItemList().Where(e => e.SetID == item.SetID).ToList();
                    string lstiIDs = string.Join(",", setItems.Select(e => Convert.ToString(e.ItemID)).ToList());
                    itemService.UserVM = UserVM;
                    itemService.selectedOptions(lstiIDs, quoteID.ToString(), scQuote.UserID, "");
                    itemViewModel.DWSelectionStatus = "1";
                }
                if (dwquoteDetail != null)
                {
                    dwquoteDetail.DWSelectionID = Convert.ToInt32(itemViewModel.DWSelectionStatus);
                    dwquoteDetail.UpdateDate = System.DateTime.Now;
                    UpdatedDateTime((int)dwquoteDetail.QuoteID);
                }


            }

            _Context.QuoteDetail.SaveChanges();
            //Commented the below for the Issue - #60-6,7,61
            //  ItemListViewModel itemListVM = GetListOfItemsBySelection(UserViewModel.CustomerAccountInfoObj.UserID, selectionStatus, ddlSelectedValue, pgno);
            ItemListViewModel itemListVm = new ItemListViewModel();
            itemListVm.noOfYesCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Count();
            itemListVm.noOfNoCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).Count();
            itemListVm.noOfMaybeCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe).Count();
            itemListVm.noOfNewCount = lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.New).Count();
            itemListVm.SelectionCount = itemListVm.noOfYesCount + itemListVm.noOfNoCount + itemListVm.noOfMaybeCount + itemListVm.noOfNewCount;
            itemListVm.SCItemsCount =
                scQuote.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes)
                    .Sum(e => e.Quantity);
            itemListVm.YesTotalPrice = (decimal)lstDWquotedetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Select(e => e.Item).Sum(e => e.Price);

            string taxschuduleID = scQuote.User.Customer != null ? scQuote.User.Customer.TaxScheduleID : null;
            decimal SalesTax = 0;
            if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
            {
                SalesTax = 0;
            }
            else
            {
                SalesTax = Convert.ToDecimal(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);
            }
            decimal? price = scQuote.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(e => e.Quantity * (e.Item != null ? e.Item.Price : GetItemPriceByItemID(itemViewModel.ItemID)));
            itemListVm.SCItemsPrice = price + (price * SalesTax);
            UserVM.SCPrice = itemListVm.SCItemsPrice;
            //itemListVm.UserVM = UserVM;
            UserVM.SCCount = itemListVm.SCItemsCount;
            return itemListVm;

        }


        private decimal? GetItemPriceByItemID(string itemID)
        {
            Item item = _Context.Item.GetSingle(e => e.ItemID == itemID);
            return item != null ? item.Price : 0;
        }

        public string getGroupName(int groupID)
        {
            string groupName = string.Empty;
            if (groupID != 0)
            {
                groupName = _Context.Group.GetSingle(e => e.GroupID == groupID).GroupName;

            }
            else
            {
                groupName = "All";
            }
            return groupName;
        }
        public List<Item> SortingAlgorithim(List<Item> lstItem)
        {
            lstItem = lstItem.OrderByDescending(e => e.ItemID).ToList();
            return lstItem;
        }

        public int GetDefaultDWByUserID()
        {
            Quote dW = null;
            if (UserVM.CRMModelProperties != null)
            {
                dW = _Context.Quote.GetSingle(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID);
            }
            if (dW != null)
            {
                return dW.QuoteID;
            }
            return 0;
        }


        public List<string> DeleteNoItemsByQuoteID(int quoteID)
        {
            List<QuoteDetail> quoteDetail = new List<QuoteDetail>();
            quoteDetail = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteID && e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).ToList();
            List<string> noItemIds = quoteDetail.Select(e => e.ItemID).ToList();
            foreach (QuoteDetail qd in quoteDetail)
            {
                _Context.QuoteDetail.Delete(qd);
                _Context.Quote.SaveChanges();
            }
            return noItemIds;
        }

    }
}
