using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models
{
    public class FAQDetailsViewModel
    {
        private int faqDetailsID;

        public int FAQDetailsID
        {
            get { return faqDetailsID; }
            set { faqDetailsID = value; }
        }

        private int faqId;

        public int FAQId
        {
            get { return faqId; }
            set { faqId = value; }
        }

        private string faqDetailedText;

        public string FAQDetailedText
        {
            get { return faqDetailedText; }
            set { faqDetailedText = value; }
        }
        
    }
}
