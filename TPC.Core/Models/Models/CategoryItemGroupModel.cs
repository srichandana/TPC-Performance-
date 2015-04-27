using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.Models
{
   public  class CategoryItemGroupModel:BaseViewModel
    {
       public List<FilterModel> lstFilterModel { get; set; }

       public ItemViewModel ItemVm { get; set; }

       public List<PackageModel> lstpackages { get; set; }
    }
}
