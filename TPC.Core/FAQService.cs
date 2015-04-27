using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TPC.Context;
using TPC.Context.Interfaces;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Context.EntityModel;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;
using TPC.Common.Enumerations;

namespace TPC.Core
{
    public class FAQService : ServiceBase<ISearchModel>, IFAQService
    {
        public FAQViewModel GetDetails()
        {
            FAQViewModel FAQVM = new FAQViewModel();
            QuoteViewService qvs = new QuoteViewService();
            List<FAQCategory> faqCategory = _Context.FAQCategory.GetAll().ToList();
            FAQVM.FAQCategories = AutoMapper.Mapper.Map<IList<FAQCategory>, IList<FAQCategoriesViewModel>>(faqCategory).ToList();

            List<FAQ> faqQuestion = _Context.FAQ.GetAll().ToList();
            FAQVM.FAQuestions = AutoMapper.Mapper.Map<IList<FAQ>, IList<FAQuestionsViewModel>>(faqQuestion).ToList();

            List<FAQDetail> faqAnswers = _Context.FAQDetail.GetAll().ToList();
            FAQVM.FAQAnswers = AutoMapper.Mapper.Map<IList<FAQDetail>, IList<FAQDetailsViewModel>>(faqAnswers).ToList();
            FAQVM.UserVM = UserVM;
            qvs.UserVM = UserVM;
            if (UserVM != null)
            {
                FAQVM.UserVM.CurrentQuoteID = qvs.getCustomerSCQuoteID();
            }
            return FAQVM;
        }
    }
}
