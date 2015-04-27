using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;

namespace TPC.Context.Interfaces
{
    public interface IContextBase
    {
        IRepository<Quote> Quote { get; }

        IRepository<Item> Item { get; }

        IRepository<QuoteType> QuoteType { get; }

        IRepository<AppUser> AppUser { get; }

        IRepository<Author> Author { get; }

        IRepository<Customer> Customer { get; }

        IRepository<CustomerRepAssignment> CustomerRepAssignment { get; }

        IRepository<CustomerTag> CustomerTag { get; }

        IRepository<DWSelectionStatu> DWSelectionStatus { get; }

        IRepository<Group> Group { get; }

        IRepository<Package> Package { get; }

        IRepository<GroupPackage> GroupPackage { get; }

        IRepository<ItemHistory> ItemHistory { get; }


        IRepository<ItemTag> ItemTag { get; }

        IRepository<Order> Order { get; }

        IRepository<Publisher> Publisher { get; }

        IRepository<QuoteDetail> QuoteDetail { get; }

        IRepository<RepUser> RepUser { get; }

        IRepository<Status> Status { get; }

        IRepository<User> User { get; }

        IRepository<CustomerAddress> CustomerAddress { get; }

        IRepository<CustomerShipToAddress> CustomerShipToAddress { get; }

        IRepository<SeriesAndCharacter> SeriesAndCharacter { get; }

        IRepository<CustomerSeriesCharacter> CustomerSeriesAndCharacter { get; }

        // IRepository<webpages_UsersInRoles> WebUsersInRoles { get; }

        IRepository<webpages_Roles> WebRoles { get; }

        IRepository<CatalogSubject> CatalogSubject { get; }

        IRepository<CatalogSubjectOption> CatalogSubjectOption { get; }

        IRepository<CatalogSubjectOptionValue> CatalogSubjectOptionValue { get; }

        IRepository<CatalogSubjectOptionProtector> CatalogSubjectOptionProtector { get; }

        IRepository<CatalogSubjectoptionShelfReady> CatalogSubjectOptionShelfReady { get; }

        IRepository<CatalogSubjectOptionProtectorValue> CatalogSubjectOptionProtectorValue { get; }

        IRepository<CatalogSubjectOptionShelfReadyValue> CatalogSubjectOptionShelfReadyValue { get; }

        IRepository<CatalogInformation> CatalogInformation { get; }

        IRepository<CustomerCatalogInformation> CustomerCatalogInformation { get; }

        IRepository<CustomerCatalogProtectorInformation> CustomerCatalogProtectorInformation { get; }

        IRepository<CustomerCatalogShelfReadyInformation> CustomerCatalogShelfReadyInformation { get; }

        IRepository<GroupPackageItem> GroupPackageItem { get; }

        IRepository<UserPreference> UserPreference { get; }

        IRepository<FAQ> FAQ { get; }

        IRepository<FAQCategory> FAQCategory { get; }

        IRepository<FAQDetail> FAQDetail { get; }

        //IRepository<UserProfile> UserProfile { get; }

        IRepository<CustomerTitle> CustomerTitle { get; }
        IRepository<PackageSubPackage> PackageSubPackage { get; }
        IRepository<SourceType> SourceType { get; }
        IRepository<webpages_Membership> webpages_Membership { get; }


        IRepository<CatalogProfileValidation> CatalogProfileValidation { get; }


        IRepository<QuoteSubmitSaveInfo> QuoteSubmitSaveInfo { get; }

        //IRepository<webpages_UsersInRoles> webpages_UsersInRoles { get; }


        IRepository<CalTagInfo> CalTagInfo { get; }

        IRepository<QuoteCallTag> QuoteCallTag { get; }

        IRepository<InvoiceHistory> InvoiceHistory { get; }

        IRepository<MailHistory> EmailHistory { get; }

        IRepository<CatalogSoftwareVersionMapping> CatalogSoftwareVersionMapping { get; }

        IRepository<TaxSchedule> TaxSchedule { get; }

        IRepository<CatalogSubjectItemIDMapping> CatalogSubjectItemIDMapping { get; }

        IRepository<GroupStyle> GroupStyle { get; }

    }
}
