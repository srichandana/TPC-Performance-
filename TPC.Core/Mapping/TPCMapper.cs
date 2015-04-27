using AutoMapper;
using System.Linq;
using TPC.Common.Constants;
using TPC.Common.Enumerations;
using TPC.Context.EntityModel;
using TPC.Core.Infrastructure;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Mapping
{
    public class TPCMapper
    {
        public static void RegisterMappers()
        {

            #region Mapping from ViewModels To Entity
            Mapper.CreateMap<ItemViewModel, Item>();
            Mapper.CreateMap<QuoteModel, Quote>()
                  .ForMember(a => a.QuoteTitle, b => b.MapFrom(e => e.QuoteText))
                  ;
            Mapper.CreateMap<CartViewModel, QuoteDetail>()
                .ForMember(a => a.QuoteDetailID, b => b.MapFrom(e => e.QuoteDetailID))
                ;
            #endregion


            #region Mapping from Entity to ViewModels
            Mapper.CreateMap<Item, ItemViewModel>();
            Mapper.CreateMap<Quote, CartWhizardInfoModel>()
                .ForMember(a => a.NumberOfBooks, b => b.MapFrom(e => e.QuoteDetails.ToList().Count))
                .ForMember(a => a.TotalPrice, b => b.MapFrom(e => e.QuoteDetails.Sum(z => z.Item.Price * z.Quantity)));


            //Mapper.CreateMap<Quote, QuoteModel>();

            Mapper.CreateMap<Quote, QuoteModel>()
                .ForMember(a => a.QuoteTypeText, b => b.MapFrom(e => e.QuoteType.QuoteTypeText))
                .ForMember(a => a.TotalItems, b => b.MapFrom(e => e.QuoteDetails.Where(z => z.Item.Status == "B" || z.Item.Status == "D").Sum(x => x.Quantity)))
                .ForMember(a => a.TotalPrice, b => b.MapFrom(e => e.QuoteDetails.Where(z => z.Item.Status == "B" || z.Item.Status == "D").Sum(z => z.Item.Price * z.Quantity)))
                .ForMember(a => a.StatusText, b => b.MapFrom(e => e.Status.StatusDescription))
                .ForMember(a=>a.QuoteName,b=>b.MapFrom(e=>e.QuoteTitle));


            Mapper.CreateMap<QuoteType, ComboBase>()
              .ForMember(a => a.ItemID, b => b.MapFrom(e => e.QuoteTypeID))
              .ForMember(a => a.ItemValue, b => b.MapFrom(e => e.QuoteTypeText));
            Mapper.CreateMap<KPLItem, KPLBasedCommonViewModel>();

            Mapper.CreateMap<Item, KPLBasedCommonViewModel>()
                .ForMember(a => a.Series, b => b.MapFrom(e => e.SeriesAndCharacter1.SCText))
                 .ForMember(a => a.Primarycharacter, b => b.MapFrom(e => e.SeriesAndCharacter.SCText))
                 .ForMember(a => a.IPrice, b => b.MapFrom(e => e.Price == null ? 0 : (double)e.Price))
                 .ForMember(a => a.Author, b => b.MapFrom(e => e.Author.AuthorName))
                  .ForMember(a => a.Publisher, b => b.MapFrom(e => e.Publisher.PublisherName))
                  .ForMember(a => a.Type, b => b.MapFrom(e => e.ProductLine.Trim()))
                 .ForMember(a => a.InterestGrade, b => b.MapFrom(e => e.InterestGrade == null ? string.Empty : e.InterestGrade == ((int)InterestGradeEnums.Grade2to3).ToString() ? QuoteValidationConstants.Grade2to3 : e.InterestGrade == ((int)InterestGradeEnums.Grade4Above).ToString() ? QuoteValidationConstants.Grade4Above : e.InterestGrade == QuoteValidationConstants.Preschooltograde1Text ? QuoteValidationConstants.Preschooltograde1 : string.Empty))
                 .ForMember(a => a.ReviewSource, b => b.MapFrom(e => (e.SLJ == "Y" ? QuoteValidationConstants.SLJ + "," : string.Empty) + (e.PW == "Y" ? QuoteValidationConstants.PW + "," : string.Empty) + (e.Horn == "Y" ? QuoteValidationConstants.HORN + "," : string.Empty) + (e.Kirkus == "Y" ? QuoteValidationConstants.KIRKUS + "," : string.Empty) + (e.LJ == "Y" ? QuoteValidationConstants.LJ : string.Empty))
                  );

            Mapper.CreateMap<Item, ItemViewModel>()
                .ForMember(a => a.IPrice, b => b.MapFrom(e => e.Price == null ? 0 : (double)e.Price));

            Mapper.CreateMap<Item, ItemDetailedViewModel>();


            Mapper.CreateMap<Item, CartViewModel>()
             .ForMember(a => a.ItemPrice, b => b.MapFrom(e => e.Price))
             .ForMember(a => a.Author, b => b.MapFrom(e => e.Author.AuthorName))
             .ForMember(a => a.Series, b => b.MapFrom(e => e.SeriesAndCharacter1.SCText))
             .ForMember(a => a.Quantity, b => b.MapFrom(e => e.QuoteDetails.FirstOrDefault().Quantity))
             .ForMember(a => a.QuoteDetailID, b => b.MapFrom(e => e.QuoteDetails.FirstOrDefault().QuoteDetailID))
             ;

            //Mapper.CreateMap<Item, KPLBasedCommonViewModel>()
            //      .ForMember(a => a.Author, b => b.MapFrom(e => e.Author.AuthorName))
            //      .ForMember(a => a.Publisher, b => b.MapFrom(e => e.Publisher.PublisherName))
            //      .ForMember(a => a.Type, b => b.MapFrom(e => e.ProductLine.Trim()))
            //    ;

            //Mapper.CreateMap<Author, AuthorViewModel>()
            //    .ForMember(a=>a.NoofBooks, b=>b.MapFrom(e=>e.Items.Select(c=>c.AuthorID).Count()));

            Mapper.CreateMap<FAQCategory, FAQCategoriesViewModel>();

            Mapper.CreateMap<FAQ, FAQuestionsViewModel>();

            Mapper.CreateMap<FAQDetail, FAQDetailsViewModel>();


            Mapper.CreateMap<Item, ComboBase>()
           .ForMember(a => a.ItemID, b => b.MapFrom(e => e.ItemID))
           .ForMember(a => a.ItemValue, b => b.MapFrom(e => e.ItemID));

            Mapper.CreateMap<Group, ItemGroupViewModel>()
          .ForMember(a => a.GroupName, b => b.MapFrom(e => e.GroupName));
            Mapper.CreateMap<Group, ComboBase>()
         .ForMember(a => a.ItemID, b => b.MapFrom(e => e.GroupID))
            .ForMember(a => a.ItemValue, b => b.MapFrom(e => e.GroupType));

            Mapper.CreateMap<User, ComboBase>()
                .ForMember(a => a.ItemID, b => b.MapFrom(e => e.UserId))
                .ForMember(a => a.ItemValue, b => b.MapFrom(e => e.UserName));

            Mapper.CreateMap<CustomerAddress, AddressBaseModel>()
                .ForMember(a => a.AddressID, b => b.MapFrom(e => e.CustAutoID))
                .ForMember(a => a.CustomerName, b => b.MapFrom(e => e.Customer.CustomerName))
                .ForMember(a => a.DivNo, b => b.MapFrom(e => e.Customer.DivisionNo))
                .ForMember(a => a.CustomerNo, b => b.MapFrom(e => e.Customer.CustomerNO)
                );



            Mapper.CreateMap<AddressBaseModel, ComboBase>()
               .ForMember(a => a.ItemID, b => b.MapFrom(e => e.AddressID))
             .ForMember(a => a.ItemValue, b => b.MapFrom(e => e.CustomerName + ',' + e.AddressLine1 + ',' + e.AddressLine2 + ',' + e.AddressLine3 + ',' + e.City + ',' + e.State + ',' + e.ZipCode));

            Mapper.CreateMap<ShipToAddressModel, ComboBase>()
             .ForMember(a => a.ItemID, b => b.MapFrom(e => e.ShipToAddress.AddressID))
          // .ForMember(a => a.ItemValue, b => b.MapFrom(e => '(' + e.ShipToAddress.CustomerNo + ") " + e.ShipToName + ',' + e.ShipToAddress.AddressLine1 + ',' + e.ShipToAddress.AddressLine2 + ',' + e.ShipToAddress.AddressLine3 + ',' + e.ShipToAddress.City + ',' + e.ShipToAddress.State + ',' + e.ShipToAddress.ZipCode));
          .ForMember(a => a.ItemValue, b => b.MapFrom(e =>  e.ShipToCode + ": " + e.ShipToName + ',' + e.ShipToAddress.AddressLine1 + ',' + e.ShipToAddress.AddressLine2 + ',' + e.ShipToAddress.AddressLine3 + ',' + e.ShipToAddress.City + ',' + e.ShipToAddress.State + ',' + e.ShipToAddress.ZipCode));


            Mapper.CreateMap<SourceType, ComboBase>()
                .ForMember(a => a.ItemID, b => b.MapFrom(e => e.SourceID))
                .ForMember(a => a.ItemValue, b => b.MapFrom(e => e.SourceName));
            #endregion
        }
    }
}
