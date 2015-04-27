using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Context.EntityModel
{
    using System;
    using System.Collections.Generic;

    public partial class KPLItem
    {
        public string ItemID { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string SetName { get; set; }
        public Nullable<int> Copyright { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
        public string Illustrator { get; set; }
        public string Classification { get; set; }
        public string Format { get; set; }
        public string InterestGrade { get; set; }
        public string Subject { get; set; }
        public string ReviewSource { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string ISBN { get; set; }
        public string OnListDate { get; set; }
        public int Pages { get; set; }
        public string ARLevel { get; set; }
        public int ARQuiz { get; set; }
        public string Dewery { get; set; }
        public string RCLevel { get; set; }
        public int RCQuiz { get; set; }
        public string Lexile { get; set; }
        public string Description { get; set; }
        public string Series { get; set; }
        public string PrimaryCharacter { get; set; }
        public string Size { get; set; }
        public Nullable<int> QuoteID { get; set; }
        public Nullable<bool> IsInQuoteType { get; set; }
        public string IsChecked { get; set; }
        public Nullable<bool> IsInCustomerTitles { get; set; }
        public Nullable<bool> SeriesBroughtBefore { get; set; }
        public Nullable<bool> CharecterBroughtBefore { get; set; }
        public Nullable<bool> IsPreviewItem { get; set; }
        public string DWDate { get; set; }
        public int OnhandQty { get; set; }
    }
}
