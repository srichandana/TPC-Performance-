using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using TPC.Core.Infrastructure;

namespace TPC.Core
{
    public class ItemGroupingService : ServiceBase<IItemGroupingModel>, IItemGroupingService
    {

        public ItemGroupingViewModel GetItemsList(string seletedItemID)
        {
            string strItemID = seletedItemID.ToString();
            List<Item> lstItemList = _Context.Item.GetAll().ToList();
            ItemGroupingViewModel itemGroupingVM = new ItemGroupingViewModel();
            itemGroupingVM.ItemIDs = AutoMapper.Mapper.Map<IList<Item>, IList<ComboBase>>(lstItemList).ToList();
            if (seletedItemID == "0")
            {
                if (itemGroupingVM.ItemIDs.Count != 0)
                {
                    seletedItemID = itemGroupingVM.ItemIDs.FirstOrDefault().ItemID;
                }
            }
                itemGroupingVM.KPLItemListVM = new KPLBasedCommonViewModel();
                Item item = _Context.Item.GetSingle(e => e.ItemID == strItemID);
                itemGroupingVM.KPLItemListVM.ARLevel = item.ARLevel == null ? string.Empty: item.ARLevel;
                itemGroupingVM.KPLItemListVM.ARQuiz = item.ARQuiz;
                itemGroupingVM.KPLItemListVM.Barcode = item.Barcode == null ? string.Empty : item.Barcode;
                itemGroupingVM.KPLItemListVM.Classification = item.Classification == null ? string.Empty : item.Classification;
                itemGroupingVM.KPLItemListVM.CopyRight = item.Copyright == null ? 0 : Convert.ToInt32(item.Copyright);
                itemGroupingVM.KPLItemListVM.Description = item.Description == null ? string.Empty : item.Description;
                itemGroupingVM.KPLItemListVM.Format = item.Format == null ? string.Empty : item.Format;
                itemGroupingVM.KPLItemListVM.Illustrator = item.Illustrator == null ? string.Empty : item.Illustrator;
                itemGroupingVM.KPLItemListVM.InterestGrade = item.InterestGrade == null ? string.Empty : item.InterestGrade;
                itemGroupingVM.KPLItemListVM.ISBN = item.ISBN == null ? string.Empty : item.ISBN;
                itemGroupingVM.KPLItemListVM.ItemID = item.ItemID;
                itemGroupingVM.KPLItemListVM.Lexile = item.Lexile == null ? string.Empty : item.Lexile;
                itemGroupingVM.KPLItemListVM.OnListDate = string.Format("{0:d}", item.OnListDate) ;
                itemGroupingVM.KPLItemListVM.Primarycharacter = item.SeriesAndCharacter == null ? string.Empty : item.SeriesAndCharacter.SCText;
                itemGroupingVM.KPLItemListVM.SecondCharacter = item.SecondaryCharacter == null ? string.Empty : item.SecondaryCharacter;
                itemGroupingVM.KPLItemListVM.Pages = item.Pages;
                itemGroupingVM.KPLItemListVM.Price = item.Price;
                itemGroupingVM.KPLItemListVM.RCLevel = item.RCLevel == null ? string.Empty : item.RCLevel;
                itemGroupingVM.KPLItemListVM.RCQuiz = item.RCQuiz;
                itemGroupingVM.KPLItemListVM.ReviewSource = item.ReviewSource == null ? string.Empty : item.ReviewSource;
                itemGroupingVM.KPLItemListVM.Series = item.SeriesAndCharacter1 == null ? string.Empty : item.SeriesAndCharacter1.SCText;
                itemGroupingVM.KPLItemListVM.Title = item.Title == null ? string.Empty : item.Title;
                itemGroupingVM.KPLItemListVM.Type = item.ProductLine == null ? string.Empty : item.ProductLine.Trim().ToString();
                itemGroupingVM.KPLItemListVM.Publisher = item.PublisherID == null ? string.Empty : _Context.Publisher.GetSingle(e => e.PublisherID == item.PublisherID).PublisherName;
                itemGroupingVM.KPLItemListVM.Author = item.AuthorID == null ? string.Empty : _Context.Author.GetSingle(e => e.AuthorID == item.AuthorID).AuthorName;
               List<Group> lstGroup= _Context.Group.GetAll().ToList();
               List<GroupPackage> lstGroupParentage = _Context.GroupPackage.GetAll().GroupBy(e => e.PackageID).Select(e => e.FirstOrDefault()).ToList();
               List<ItemGroupViewModel> lstGroupNames = lstGroupParentage.Select(e => new ItemGroupViewModel
               {
                    GroupID = e.Group.GroupID,
                    GroupName = e.Group.GroupName
               }).ToList();



               itemGroupingVM.LstItemGVM =lstGroupNames;
               itemGroupingVM.LstGroupTypes = AutoMapper.Mapper.Map<IList<Group>, IList<ComboBase>>(lstGroup).Where(e=>e.ItemValue!=null && e.ItemValue!=string.Empty).GroupBy(e => e.ItemValue).Select(e => e.FirstOrDefault()).ToList();
            
            return itemGroupingVM;
        }

      public void AddNewGroupType(string groupType)
        {
            //while adding new grouptype ...groupname and grouptype both are same
            if (groupType != string.Empty)
            {
                _Context.Group.Add(new Group { GroupName = groupType, GroupType = groupType, CreatedDate = System.DateTime.Now, Updatedate = System.DateTime.Now });
                _Context.Group.SaveChanges();
            }
            
        }

        public ItemGroupingViewModel GetSingle(System.Linq.Expressions.Expression<Func<ItemGroupingViewModel, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public void Add(ItemGroupingViewModel entity)
        {
            // _itemRepository.Add(Mapper.Map<ItemViewModel,Item>(entity));
            throw new NotImplementedException();
        }

        public void Delete(ItemGroupingViewModel entity)
        {
            throw new NotImplementedException();
        }

        public void Update(ItemGroupingViewModel entity)
        {
            throw new NotImplementedException();
        }

        public IList<ItemGroupingViewModel> GetAll(System.Linq.Expressions.Expression<Func<ItemGroupingViewModel, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public IList<ItemGroupingViewModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<ItemGroupingViewModel> Query(System.Linq.Expressions.Expression<Func<ItemGroupingViewModel, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }
        public long Count(System.Linq.Expressions.Expression<Func<ItemGroupingViewModel, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }



        public bool AddGroupParentage(int childGroupID, int parentGroupID)
        {
            _Context.GroupPackage.Add(new GroupPackage {  GroupID = childGroupID, PackageID = parentGroupID, CreatedDate = System.DateTime.Now, Updatedate = System.DateTime.Now });
            _Context.GroupPackage.SaveChanges();
            return true;
        }
    }
}
