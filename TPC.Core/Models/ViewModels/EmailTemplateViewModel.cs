using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class EmailTemplateViewModel
    {
        private List<EmailTemplateModel> emailDWtemplateList;

        public List<EmailTemplateModel> EmailDWTemplateList
        {
            get { return emailDWtemplateList; }
            set { emailDWtemplateList = value; }
        }
        private string fromAddress;

        public string FromAddress
        {
            get { return fromAddress; }
            set { fromAddress = value; }
        }

        private string displayName;
        public string DisplayName
        {
            get { return displayName; }

            set { displayName = value; }
        }

        private string toAddress;

        public string ToAddress
        {
            get { return toAddress; }
            set { toAddress = value; }
        }

        private int personId;
        public int PersonID
        {
            get { return personId; }
            set { personId = value; }
        }

        private string repComments;

        public string RepComments
        {
            get { return repComments; }
            set { repComments = value; }
        }

        private string repPhoneFax = string.Empty;

        public string RepPhoneFax
        {
            get { return repPhoneFax; }
            set { repPhoneFax = value; }
        }

        private string repFisrtName = string.Empty;

        public string RepFirstName
        {
            get { return repFisrtName; }
            set { repFisrtName = value; }
        }


        //private bool isMailSent=false;
        //public bool IsMailSent
        //{
        //    get { return isMailSent; }
        //    set { isMailSent = value; }
        //}

        //private int quoteId;
        //public int QuoteId
        //{
        //    get { return quoteId; }
        //    set { quoteId = value; }
        //}
    }
}
