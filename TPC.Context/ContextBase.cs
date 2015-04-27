using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;

namespace TPC.Context
{
    public class ContextBase : IContextBase
    {
        public IRepository<Quote> Quote
        {
            get
            {
                return new RepositoryBase<Quote>();
            }
        }

        public IRepository<Item> Item
        {
            get
            {
                return new RepositoryBase<Item>();
            }
        }

        public IRepository<QuoteType> QuoteType
        {
            get
            {
                return new RepositoryBase<QuoteType>();
            }
        }

        public IRepository<QuoteDetail> QuoteDetail
        {
            get
            {
                return new RepositoryBase<QuoteDetail>();
            }
        }



        public IRepository<AppUser> AppUser
        {
            get
            {
                return new RepositoryBase<AppUser>();
            }
        }

        public IRepository<Author> Author
        {
            get
            {
                return new RepositoryBase<Author>();
            }
        }

        public IRepository<Customer> Customer
        {
            get
            {
                return new RepositoryBase<Customer>();
            }
        }

        public IRepository<CustomerRepAssignment> CustomerRepAssignment
        {
            get
            {
                return new RepositoryBase<CustomerRepAssignment>();
            }
        }

        public IRepository<CustomerTag> CustomerTag
        {
            get
            {
                return new RepositoryBase<CustomerTag>();
            }
        }

        public IRepository<DWSelectionStatu> DWSelectionStatus
        {
            get
            {
                return new RepositoryBase<DWSelectionStatu>();
            }
        }

        public IRepository<Group> Group
        {
            get
            {
                return new RepositoryBase<Group>();
            }
        }

        public IRepository<Package> Package
        {
            get
            {
                return new RepositoryBase<Package>();
            }
        }

        public IRepository<GroupPackage> GroupPackage
        {
            get
            {
                return new RepositoryBase<GroupPackage>();
            }
        }



        public IRepository<ItemHistory> ItemHistory
        {
            get
            {
                return new RepositoryBase<ItemHistory>();
            }
        }


        public IRepository<ItemTag> ItemTag
        {
            get
            {
                return new RepositoryBase<ItemTag>();
            }
        }

        public IRepository<Order> Order
        {
            get
            {
                return new RepositoryBase<Order>();
            }
        }

        public IRepository<Publisher> Publisher
        {
            get
            {
                return new RepositoryBase<Publisher>();
            }
        }



        public IRepository<RepUser> RepUser
        {
            get
            {
                return new RepositoryBase<RepUser>();
            }
        }

        public IRepository<Status> Status
        {
            get
            {
                return new RepositoryBase<Status>();
            }
        }

        public IRepository<User> User
        {
            get
            {
                return new RepositoryBase<User>();
            }
        }

        public IRepository<CustomerAddress> CustomerAddress
        {
            get
            {
                return new RepositoryBase<CustomerAddress>();
            }
        }

        public IRepository<CustomerShipToAddress> CustomerShipToAddress
        {
            get
            {
                return new RepositoryBase<CustomerShipToAddress>();
            }
        }


        public IRepository<SeriesAndCharacter> SeriesAndCharacter
        {
            get
            {
                return new RepositoryBase<SeriesAndCharacter>();
            }
        }

        public IRepository<CustomerSeriesCharacter> CustomerSeriesAndCharacter
        {
            get
            {
                return new RepositoryBase<CustomerSeriesCharacter>();
            }
        }

        //public IRepository<webpages_UsersInRoles> WebUsersInRoles
        //{
        //    get
        //    {
        //        return new RepositoryBase<webpages_UsersInRoles>();
        //    }
        //}
        public IRepository<webpages_Roles> WebRoles
        {
            get
            {
                return new RepositoryBase<webpages_Roles>();
            }
        }

        public IRepository<CatalogSubject> CatalogSubject
        {
            get { return new RepositoryBase<CatalogSubject>(); }
        }

        public IRepository<CatalogSubjectOption> CatalogSubjectOption
        {
            get { return new RepositoryBase<CatalogSubjectOption>(); }
        }

        public IRepository<CatalogSubjectOptionValue> CatalogSubjectOptionValue
        {
            get { return new RepositoryBase<CatalogSubjectOptionValue>(); }
        }


        public IRepository<CatalogInformation> CatalogInformation
        {
            get
            {
                return new RepositoryBase<CatalogInformation>();
            }
        }

        public IRepository<CustomerCatalogInformation> CustomerCatalogInformation
        {
            get
            {
                return new RepositoryBase<CustomerCatalogInformation>();

            }
        }
        public IRepository<CatalogSubjectOptionProtector> CatalogSubjectOptionProtector
        {
            get
            {
                return new RepositoryBase<CatalogSubjectOptionProtector>();

            }
        }
        public IRepository<CatalogSubjectOptionProtectorValue> CatalogSubjectOptionProtectorValue
        {
            get
            {
                return new RepositoryBase<CatalogSubjectOptionProtectorValue>();

            }
        }

        public IRepository<CatalogSubjectOptionShelfReadyValue> CatalogSubjectOptionShelfReadyValue
        {
            get
            {
                return new RepositoryBase<CatalogSubjectOptionShelfReadyValue>();

            }
        }

        public IRepository<CatalogSubjectoptionShelfReady> CatalogSubjectOptionShelfReady
        {
            get { return new RepositoryBase<CatalogSubjectoptionShelfReady>(); }
        }


        public IRepository<GroupPackageItem> GroupPackageItem
        {
            get
            {
                return new RepositoryBase<GroupPackageItem>();

            }
        }
        public IRepository<UserPreference> UserPreference
        {
            get
            {
                return new RepositoryBase<UserPreference>();

            }
        }


        public IRepository<FAQ> FAQ
        {
            get
            {
                return new RepositoryBase<FAQ>();

            }
        }

        public IRepository<FAQCategory> FAQCategory
        {
            get
            {
                return new RepositoryBase<FAQCategory>();

            }
        }

        public IRepository<FAQDetail> FAQDetail
        {
            get
            {
                return new RepositoryBase<FAQDetail>();
            }
        }

        //public IRepository<UserProfile> UserProfile
        //{
        //    get
        //    {
        //        return new RepositoryBase<UserProfile>();

        //    }
        //}

        public IRepository<CustomerTitle> CustomerTitle
        {
            get
            {
                return new RepositoryBase<CustomerTitle>();

            }
        }

        public IRepository<PackageSubPackage> PackageSubPackage
        {
            get
            {
                return new RepositoryBase<PackageSubPackage>();
            }
        }

        public IRepository<SourceType> SourceType
        {
            get
            {
                return new RepositoryBase<SourceType>();
            }
        }



        public IRepository<webpages_Membership> webpages_Membership
        {
            get
            {
                return new RepositoryBase<webpages_Membership>();
            }
        }
        public IRepository<CatalogProfileValidation> CatalogProfileValidation
        {
            get
            {
                return new RepositoryBase<CatalogProfileValidation>();
            }
        }


        public IRepository<QuoteSubmitSaveInfo> QuoteSubmitSaveInfo
        {
            get
            {
                return new RepositoryBase<QuoteSubmitSaveInfo>();

            }
        }
        //public IRepository<webpages_UsersInRoles> webpages_UsersInRoles
        //{
        //    get
        //    {
        //        return new RepositoryBase<webpages_UsersInRoles>();

        //    }
        //}

        public IRepository<CalTagInfo> CalTagInfo
        {
            get
            {
                return new RepositoryBase<CalTagInfo>();

            }
        }

        public IRepository<QuoteCallTag> QuoteCallTag
        {
            get
            {
                return new RepositoryBase<QuoteCallTag>();

            }
        }

        public IRepository<InvoiceHistory> InvoiceHistory
        {
            get
            {
                return new RepositoryBase<InvoiceHistory>();

            }
        }

        public IRepository<CustomerCatalogProtectorInformation> CustomerCatalogProtectorInformation
        {
            get { return new RepositoryBase<CustomerCatalogProtectorInformation>(); }
        }

        public IRepository<CustomerCatalogShelfReadyInformation> CustomerCatalogShelfReadyInformation
        {
            get { return new RepositoryBase<CustomerCatalogShelfReadyInformation>(); }
        }

        public IRepository<MailHistory> EmailHistory
        {
            get { return new RepositoryBase<MailHistory>(); }
        }


        public IRepository<CatalogSoftwareVersionMapping> CatalogSoftwareVersionMapping
        {
            get { return new RepositoryBase<CatalogSoftwareVersionMapping>(); }

        }
        public IRepository<TaxSchedule> TaxSchedule
        {
            get { return new RepositoryBase<TaxSchedule>(); }

        }

        public IRepository<CatalogSubjectItemIDMapping> CatalogSubjectItemIDMapping
        {
            get { return new RepositoryBase<CatalogSubjectItemIDMapping>(); }
        }

        public IRepository<GroupStyle> GroupStyle
        {
            get { return new RepositoryBase<GroupStyle>(); }
        }
    }
}
