using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class ShipToAddressModel
    {

        private string shipToCode;

        public string ShipToCode
        {
            get { return shipToCode; }
            set { shipToCode = value; }
        }

        private string shipToName;

        public string ShipToName
        {
            get { return shipToName; }
            set { shipToName = value; }
        }

        private AddressBaseModel shipToAddress;

        public AddressBaseModel ShipToAddress
        {
            get { return shipToAddress; }
            set { shipToAddress = value; }
        }
    }
}
