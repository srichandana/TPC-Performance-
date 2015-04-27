using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
    public interface IActiveQuoteService : IService<ActiveQuoteViewModel>
    {
        ActiveQuoteViewModel GetActiveQuoteVM();

        bool CreateQuote(QuoteModel quoteModel);

        CreateQuoteViewModel GetQuoteTypes();

        bool DeleteQuote(QuoteModel quoteModel);

        bool CheckCataalogInfo(int CustomerID);

        string CheckCatalogInfoforCustomer(int CustomerID);

        CreateDecisionWizardViewModel GetDWInfo();

        QuoteModel InsertMergeQuoteDetails(List<int?> lstQuoteIds, int customerID,int currentMergeQuoteID);

        string UpdateRecentContactedByQuoteIDs(string quoteIDs, string selectedTemplate, string repComment);

        string GetQuoteTypeTextByQuoteTypeID(int quoteTypeID);

        string GetQuoteStatusbyStatusID(int statusID);

        SubmitQuoteViewModel GetUserAddressInfo(int quoteID);

        ActiveQuoteViewModel SubmitQuote(SubmitQuoteViewModel sqvm, string saveOrSubmit);
       // void SubmitQuote(SubmitQuoteViewModel sqvm, bool isSave=false);

        //bool UpdateRepoHoldStaus(int quoteID, bool quoteStatus);
        //bool ExecuteValidations(int quoteID);





        Dictionary<string, bool> ValidateQuote(int quoteID);

        Dictionary<string, bool> UpdateRepoHoldStatus(int quoteID, bool isRepoHold);


        OrderViewModel GeneratePDF(int quoteID);

        CalTagViewModel GetCalTagInfo(int quoteId);

        void InsertCalTagInfo(CalTagViewModel calTagVM);

        RapidEntryModel getRapidEntry(int quoteid);
    }
}
