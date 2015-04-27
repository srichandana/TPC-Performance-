using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models.ViewModels
{
    public class ActiveQuoteViewModel :BaseViewModel, IActiveQuoteModel
    {
        public List<CustomerAccountSCDWModel> ListCustomerAccountSCDWQuoteInfo { get; set; }

        public List<QuoteModel> ListActiveQuote { get; set; }

        public List<QuoteModel> ListQuoteHistory { get; set; }

        //private List<ComboBase> lstCustommeremail;

        //public List<ComboBase> LstCustomerEmails
        //{
        //    get { return lstCustommeremail; }
        //    set { lstCustommeremail = value; }
        //}
    }
}
