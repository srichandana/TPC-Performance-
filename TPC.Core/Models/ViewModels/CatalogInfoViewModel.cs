using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.Models;
using System.ComponentModel.DataAnnotations;
using TPC.Core.Infrastructure;


namespace TPC.Core.Models.ViewModels
{
    public class CatalogInfoViewModel : BaseViewModel
    {
        [Display(Name = "Cataloging Options", Order = 1)]
        public Dictionary<string,Dictionary<string, Dictionary<CatalogBaseModel, List<CatalogBaseModel>>>> CatalogOptions { get; set; }
        
        private int thisUserID;
        [Display(Name = "Customer ID", Order = 5)]
        public int ThisUserID
        {
            get { return thisUserID; }
            set { thisUserID = value; }
        }
        [Display(Order = -1, Name = "Validation Profile")]
        public Dictionary<string, string> ValidationCatalogBasicProfileModel { get; set; }

        private bool isARRCExits=false;

        public bool ISARRCExits
        {
            get { return isARRCExits; }
            set { isARRCExits = value; }
        }

        private DateTime? createdDate;

        public DateTime? CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        private DateTime? updatedDate;

        public DateTime? UpdatedDate
        {
            get { return updatedDate; }
            set { updatedDate = value; }
        }

        private string updatedUserName;

        public string UpdatedUserName
        {
            get { return updatedUserName; }
            set { updatedUserName = value; }
        }
        
        
    }
}
