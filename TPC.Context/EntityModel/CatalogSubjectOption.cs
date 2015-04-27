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
    
    public partial class CatalogSubjectOption
    {
        public CatalogSubjectOption()
        {
            this.CatalogProfileValidations = new HashSet<CatalogProfileValidation>();
            this.CatalogProfileValidations1 = new HashSet<CatalogProfileValidation>();
            this.CatalogSubjectItemIDMappings = new HashSet<CatalogSubjectItemIDMapping>();
            this.CatalogSubjectOptionProtectors = new HashSet<CatalogSubjectOptionProtector>();
            this.CatalogSubjectOptionValues = new HashSet<CatalogSubjectOptionValue>();
            this.CatalogSubjectoptionShelfReadies = new HashSet<CatalogSubjectoptionShelfReady>();
        }
    
        public int CatalogSubjectOptionID { get; set; }
        public int CatalogSubjectID { get; set; }
        public string CatalogSubjectOption1 { get; set; }
        public string ItemID { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ColumnType { get; set; }
    
        public virtual CatalogColumnType CatalogColumnType { get; set; }
        public virtual ICollection<CatalogProfileValidation> CatalogProfileValidations { get; set; }
        public virtual ICollection<CatalogProfileValidation> CatalogProfileValidations1 { get; set; }
        public virtual CatalogSubject CatalogSubject { get; set; }
        public virtual ICollection<CatalogSubjectItemIDMapping> CatalogSubjectItemIDMappings { get; set; }
        public virtual ICollection<CatalogSubjectOptionProtector> CatalogSubjectOptionProtectors { get; set; }
        public virtual ICollection<CatalogSubjectOptionValue> CatalogSubjectOptionValues { get; set; }
        public virtual Item Item { get; set; }
        public virtual ICollection<CatalogSubjectoptionShelfReady> CatalogSubjectoptionShelfReadies { get; set; }
    }
}
