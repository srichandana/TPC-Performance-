using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models
{
   public  class BaseViewModel
    {
        [Display(Name = "UserVM", Order = -1)]
       public UserViewModel UserVM { get; set; }


        private bool isListView = false;
       [Display(Name = "IsListView", Order = -1)]
        public bool IsListView
        {
            get { return isListView; }
            set { isListView = value; }
        }
    }
}
