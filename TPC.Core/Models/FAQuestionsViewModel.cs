using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models
{
    public class FAQuestionsViewModel
    {
        private int faqId;

        public int FAQId
        {
            get { return faqId; }
            set { faqId = value; }
        }

        private string faqQuestion;

        public string FAQQuestion
        {
            get { return faqQuestion; }
            set { faqQuestion = value; }
        }

        private int faqCategoryID;

        public int FAQCategoryID
        {
            get { return faqCategoryID; }
            set { faqCategoryID = value; }
        }
        
    }
}
