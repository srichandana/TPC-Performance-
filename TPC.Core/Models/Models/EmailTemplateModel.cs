using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models.Models
{
    public class EmailTemplateModel : CartWhizardInfoModel
    {
        private int noOfDays;

        public int NoOfDays
        {
            get { return noOfDays; }
            set { noOfDays = value; }
        }

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        private List<string> lstISBN;

        public List<string> LstISBN
        {
            get { return lstISBN; }
            set { lstISBN = value; }
        }
        
    }
    
}
