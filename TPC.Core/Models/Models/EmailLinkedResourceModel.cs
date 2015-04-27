using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class EmailLinkedResourceModel
    {
        private string htmlImagecontent;
        public string HtmlImageContent
        {
            get { return htmlImagecontent; }
            set { htmlImagecontent = value; }
        }

        private List<LinkedResource> lstimageResource;
        public List<LinkedResource> ListImageResource
        {
            get { return lstimageResource; }
            set { lstimageResource = value; }
        }
    }
}
