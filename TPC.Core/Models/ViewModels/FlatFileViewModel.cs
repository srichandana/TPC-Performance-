using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class FlatFileViewModel : BaseViewModel
    {
        private FlatFileHeaderModel flatHeaderModel;

        public FlatFileHeaderModel FlatHeaderModel
        {
            get { return flatHeaderModel; }
            set { flatHeaderModel = value; }
        }

        private List<FlatFileDetailModel> lstFlatDetailModel;

        public List<FlatFileDetailModel> LstFlatDetailModel
        {
            get { return lstFlatDetailModel; }
            set { lstFlatDetailModel = value; }
        }

    }
}
