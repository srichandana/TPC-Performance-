using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
   public class UserPreferenceModel
    {
        private Array userPreferredList;

        public Array UserPreferredList
        {
            get { return userPreferredList; }
            set { userPreferredList = value; }
        }
        
    }
}
