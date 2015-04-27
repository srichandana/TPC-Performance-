using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TPC.Core.Models.Models
{
    public class PackageModel : BaseViewModel
    {
        private int packageID;

        public int PackageID
        {
            get { return packageID; }
            set { packageID = value; }
        }

        private string packageName;
        [Required]
        public string PackageName
        {
            get { return packageName; }
            set { packageName = value; }
        }

        private int Priority;
         [Required]
        public int priority
        {
            get { return Priority; }
            set { Priority = value; }
        }

        private string isingroup;

        public string IsInGroup
        {
            get { return isingroup; }
            set { isingroup = value; }
        }
        
    }
}
