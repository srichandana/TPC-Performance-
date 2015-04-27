using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
   public class CreateCatalogInfoModel :BaseViewModel
    {
        private int custUserID;

        public int CustUserID
        {
            get { return custUserID; }
            set { custUserID = value; }
        }

        private int catalogSubjectOptionValueID;

        public int CatalogSubjectOptionValueID
        {
            get { return catalogSubjectOptionValueID; }
            set { catalogSubjectOptionValueID = value; }
        }

        private string comments;

        public string Comments
        {
            get { return comments; }
            set { comments = value; }
        }

        public int CatalogSubjectOptionID { get; set; }

        public int CCProtectorValueID { get; set; }

        public int CCShelfReadyValueID { get; set; }
    }
}
