using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models
{
   public  class QuoteViewModel:BaseViewModel
    {
        /*for data Annotations */
        public CartViewModel cartViewModel { get; set; }

        private int quoteID;

        public int QuoteID
        {
            get { return quoteID; }
            set { quoteID = value; }
        }

        /* For Service*/
        public List<CartViewModel> CartListView { get; set; }

        public List<ComboBase> QuoteTypes { get; set; }

        private string quoteText;

        public string QuoteText
        {
            get { return quoteText; }
            set { quoteText = value; }
        }

        private bool includeCatalogStatus;

        public bool IncludeCatalogStatus
        {
            get { return includeCatalogStatus; }
            set { includeCatalogStatus = value; }
        }

        private int quotestatusid;

        public int QuoteStatusID
        {
            get { return quotestatusid; }
            set { quotestatusid = value; }
        }
        private decimal salesTax;
        public decimal SalesTax
        {
            get { return salesTax; }
            set { salesTax = value; }
        }

        public string CustomerName { get; set; }
        public string PONo { get; set; }

        public string Comments { get; set; }
    }
}
