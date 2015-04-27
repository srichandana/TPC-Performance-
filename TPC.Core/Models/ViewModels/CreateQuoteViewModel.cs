using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;
using System.Web;
using TPC.Core.Infrastructure;
using TPC.Core.Models.Models;

namespace TPC.Core.Models
{
    public class CreateQuoteViewModel :BaseViewModel, ICreateQuoteModel
    {

        public QuoteModel QuoteModel { get; set; }

        public List<ComboBase> QuoteTypes { get; set; }

    }
}
