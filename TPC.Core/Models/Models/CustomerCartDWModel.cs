using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
  public  class CustomerCartDWModel:UserBaseModel
    {
        private Dictionary<string,string> dictCartDWItemsCount;

        public Dictionary<string,string> DictCartDWItemsCount
        {
            get { return dictCartDWItemsCount; }
            set { dictCartDWItemsCount = value; }
        }
        
    }
}
