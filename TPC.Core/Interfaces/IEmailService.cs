using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Interfaces
{
    interface IEmailService
    {
        bool SaveMailHistory(string frmAddress, string toAddress, string ccAddress, string body, int quoteId = 0, string type = null, int personID = 0, string quoteTitle = null);
        
    }
}
