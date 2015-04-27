using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Models.Models;

namespace TPC.Core.Models.ViewModels
{
    public class CategoriesItemContainerViewModel:BaseViewModel
    {
        private CategoriesPartialViewModel categoriesPVM;

        public CategoriesPartialViewModel CategoriesPVM
        {
            get { return categoriesPVM; }
            set { categoriesPVM = value; }
        }

        public List<Item> item { get; set; }
        
    }
}