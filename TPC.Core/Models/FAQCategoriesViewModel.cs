using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models
{
    public class FAQCategoriesViewModel
    {
        private int faqCategoryID;

        public int FAQCategoryID
        {
            get { return faqCategoryID; }
            set { faqCategoryID = value; }
        }

        private string categories;

        public string Categories
        {
            get { return categories; }
            set { categories = value; }
        }

    }
}
