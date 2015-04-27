using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models
{
    public class KPLItemConatinerViewModel :BaseViewModel
    {
        private List<KPLBasedCommonViewModel> kplbasedcVM;

        public List<KPLBasedCommonViewModel> KPLBasedVM
        {
            get { return kplbasedcVM; }
            set { kplbasedcVM = value; }
        }

        private List<string> columns;

        public List<string> Columns
        {
            get { return columns; }
            set { columns = value; }
        }
        
    }
}
