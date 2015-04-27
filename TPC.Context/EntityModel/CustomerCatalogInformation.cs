//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TPC.Context.EntityModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class CustomerCatalogInformation
    {
        public CustomerCatalogInformation()
        {
            this.CustomerCatalogProtectorInformations = new HashSet<CustomerCatalogProtectorInformation>();
            this.CustomerCatalogShelfReadyInformations = new HashSet<CustomerCatalogShelfReadyInformation>();
        }
    
        public int CustomerCatID { get; set; }
        public Nullable<int> CatalogSubjectOptionValueID { get; set; }
        public int CustAutoID { get; set; }
        public string Comments { get; set; }
        public Nullable<bool> Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public Nullable<int> LoggedInUserID { get; set; }
    
        public virtual CatalogSubjectOptionValue CatalogSubjectOptionValue { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual ICollection<CustomerCatalogProtectorInformation> CustomerCatalogProtectorInformations { get; set; }
        public virtual ICollection<CustomerCatalogShelfReadyInformation> CustomerCatalogShelfReadyInformations { get; set; }
        public virtual User User { get; set; }
    }
}