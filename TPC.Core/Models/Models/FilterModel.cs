using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models.Models
{
   public class FilterModel
    {
        private int groupID;

        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
        }

        private string groupName;
       [Required]
        public string GroupName
        {
            get { return groupName; }
            set { groupName = value; }
        }

        private string description;
       [Required]
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }


        private List<FilterModel> childGroups;

        public List<FilterModel> ChildGroups
        {
            get { return childGroups; }
            set { childGroups = value; }
        }

        private int packagePriority;
       [Required]
       [Range(1, Int32.MaxValue)]
        public int PackagePriority
        {
            get { return packagePriority; }
            set { packagePriority = value; }
        }

        //private List<KPLBasedCommonViewModel> kplbasedcVM;

        //public List<KPLBasedCommonViewModel> KPLBasedVM
        //{
        //    get { return kplbasedcVM; }
        //    set { kplbasedcVM = value; }
        //}

        private int selectedgroupID;

        public int SelectedGroupID
        {
            get { return selectedgroupID; }
            set { selectedgroupID = value; }
        }

        private int CurrentPageIndex;

        public int currentPageIndex
        {
            get { return CurrentPageIndex; }
            set { CurrentPageIndex = value; }
        }
        private string GroupType;

        public string Grouptype
        {
            get { return GroupType; }
            set { GroupType = value; }
        }

        private List<CartViewModel> lstKplbasedModel;

        public List<CartViewModel> LstKplbasedModel
        {
            get { return lstKplbasedModel; }
            set { lstKplbasedModel = value; }
        }

        private List<ComboBase> ddlpriority;

        public List<ComboBase> ddlPriority
        {
            get { return ddlpriority; }
            set { ddlpriority = value; }
        }

        private string type;

        public string Type
        {
            get { return type; }
            set { type = value; }
        }
        

        public List<PackageModel> lstpackages { get; set; }
        public int groupPackageItems { get; set; }

        private string style;

        public string Style
        {
            get { return style; }
            set { style = value; }
        }
        
    }
}
