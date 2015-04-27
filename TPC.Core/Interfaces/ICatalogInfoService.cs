using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using TPC.Context.EntityModel;
using TPC.Core.Infrastructure;

namespace TPC.Core.Interfaces
{
    public interface ICatalogInfoService : IService<CatalogInfoViewModel>
    {
        CatalogInfoViewModel getCatalogInfoValues(int CustomerID);

        string calculatePriceForCatalog(int catalogItemSubjectOptionId, int protectorSubjSubOptionId, int shelfReadySubjSubOptionId, int protectorsCheckedCount);

        CreateCatalogInfoModel CreateCatalogInfo(Dictionary<string, string> dctselectionvalues, int custID);

        List<CatalogSubjectOptionValue> GetVersionListBySoftwareID(int softwareValueID);

        List<ComboBase> GetVersionListStringBySoftwareID(int softwareValueID);

        bool CheckARRCLevelExitsForTitlesByQuoteID(int quoteid);

        void UpdateStatusandInsertforCustomerCatalogInformation(List<CustomerCatalogInformation> lstCustomerCatalogInfo, List<CreateCatalogInfoModel> lstcreatecatInfoModel, int custAutoID);

        int GetSubjectOptionvalueIDbySubjOptionID(int catSubjOptionID);

        CatalogSubjectItemIDMapping GetCatalogSubjectMappingBySelectionofCatalogItem(int catalogSubjectOptID, bool isCatalogLabelChecked, bool isCatalogProChecked, bool isCatalogExtProChecked, bool isCatalogSRChecked);

    }
}
