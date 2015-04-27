using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Infrastructure
{
    public class ComboBase
    {
        #region PrivateProperties
       
        private string itemID;
        private string itemValue;
        private bool selected=false;

        #endregion

        #region PublicProperties

        public string ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }
        
        public string ItemValue
        {
            get { return itemValue; }
            set { itemValue = value; }
        }

        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }
        #endregion


    }
}
