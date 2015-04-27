using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using TPC.Context;
using TPC.Context.EntityModel;

namespace TPC.Core.Interfaces
{
    public interface IFileCreationService
    {
        bool FileCreation(SubmitQuoteViewModel sqvm);
        FlatFileDetailModel FillCatalogProtectorValues(int noofBooksCount, int customerID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0);
        List<FlatFileDetailModel> FillCatalogSpecialChargeValues(int noofBooksCount, int customerID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0);
        FlatFileDetailModel FillCatalogShelfReadyValues(int noofBooksCount, int customerID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0);
        List<FlatFileDetailModel> FillCatalogInfovalues(int noofBooksCount, int custAutoID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0);
        List<CustomerCatalogInformation> GetCatalogProtector_ShelfReadyInfoValues(int customerID);
        UserViewModel UserVM { get; set; }
    }
}
