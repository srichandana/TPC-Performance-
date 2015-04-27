using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Infrastructure;
using TPC.Core.Models.Models;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models.ViewModels
{
    public class CreateDecisionWizardViewModel : BaseViewModel
    {
        public QuoteModel QuoteModel { get; set; }

        //[Display(Name="Customer")]
        //public List<ComboBase> lstCustomers { get; set; }

    }
}
