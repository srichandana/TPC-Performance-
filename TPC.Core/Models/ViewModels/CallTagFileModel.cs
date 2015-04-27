using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.ViewModels
{
    public class CallTagFileViewModel
    {
        private int quoteId;
        [Display(Name = "Quote Number")]
        public int QuoteID
        {
            get { return quoteId; }
            set { quoteId = value; }
        }

        private string divCust;
        [Display(Name = "Div-Cust #")]
        public string DivCust
        {
            get { return divCust; }
            set { divCust = value; }
        }

        private string custName;
        [Display(Name = "Customer Name")]
        public string CustName
        {
            get { return custName; }
            set { custName = value; }
        }

        private DateTime futureDate;
        [Display(Name = "Date")]
        public DateTime FutureDate
        {
            get { return futureDate; }
            set { futureDate = value; }
        }
    }
}
