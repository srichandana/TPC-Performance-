using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.EntityModel;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Models
{
    public class FAQViewModel:BaseViewModel
    {
        public List<FAQCategoriesViewModel> FAQCategories { get; set; }

        public List<FAQuestionsViewModel> FAQuestions { get; set; }

        public List<FAQDetailsViewModel> FAQAnswers { get; set; }

    }
}
