using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Core.Models.Models
{
    public class CustomerAddressesModel
    {

        private AddressBaseModel parentAddressModel;

        public AddressBaseModel ParentAddressModel
        {
            get { return parentAddressModel; }
            set { parentAddressModel = value; }
        }
        
        private AddressBaseModel childAddressInfo;

        public AddressBaseModel ChildAddressInfo
        {
            get { return childAddressInfo; }
            set { childAddressInfo = value; }
        }

        private ShipToAddressModel shipToAddressInfo;

        public ShipToAddressModel ShipToAddressInfo
        {
            get { return shipToAddressInfo; }
            set { shipToAddressInfo = value; }
        }
        
    }
}
