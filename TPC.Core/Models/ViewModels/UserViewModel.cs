using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class UserViewModel : IUserViewModel
    {
        private int currentQuoteID;

        public int CurrentQuoteID
        {
            get { return currentQuoteID; }
            set { currentQuoteID = value; }
        }

        private Dictionary<string, string> preferences;

        public Dictionary<string, string> Preferences
        {
            get { return preferences; }
            set { preferences = value; }
        }


        public string SearchCategory { get; set; }

        private Dictionary<int, string> dwDetails;

        public Dictionary<int, string> DWDetails
        {
            get { return dwDetails; }
            set { dwDetails = value; }
        }

        public int SCCount { get; set; }
        public decimal? SCPrice { get; set; }
        public CRMModel CRMModelProperties { get; set; }
        public ShipToAddressModel CustomerAddressBaseModel { get; set; }


    }
}
