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
    
    public partial class CalTagInfo
    {
        public CalTagInfo()
        {
            this.QuoteCallTags = new HashSet<QuoteCallTag>();
        }
    
        public int CalTagInfoID { get; set; }
        public string CalTagInfoText { get; set; }
        public string CalTagInfoType { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
    
        public virtual ICollection<QuoteCallTag> QuoteCallTags { get; set; }
    }
}
