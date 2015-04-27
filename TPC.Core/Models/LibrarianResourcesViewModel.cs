using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class LibrarianResourcesViewModel:BaseViewModel
    {
        private Dictionary<string, Dictionary<string, string>> dictlibraryResource;

        public Dictionary<string, Dictionary<string, string>> DictlibraryResource
        {
            get { return dictlibraryResource; }
            set { dictlibraryResource = value; }
        }
        
    }
}
