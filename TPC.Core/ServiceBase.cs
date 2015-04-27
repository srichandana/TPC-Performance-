using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TPC.Common;
using TPC.Common.Enumerations;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
namespace TPC.Core
{
    public abstract class ServiceBase<T> : IService<T>
    {
        private IContextBase _context;

        public ServiceBase()
            : this(new ContextBase())
        {

        }

        public ServiceBase(IContextBase contextBase)
        {
            _context = contextBase ?? new ContextBase();

        }

        public IContextBase _Context
        {
            get { return _context; }
        }

        public UserViewModel UserVM { get; set; }

        public void CustomerCatalogBarcodeManipulation(int custAutoID, int quoteid)
        {
            List<CreateCatalogInfoModel> lstccInfoModel = new List<CreateCatalogInfoModel>();

            ICatalogInfoService _iCatInfoSrv = new CatalogInfoService();
            long lastblockUsed = 0;
            long nextblockavailble = 0;
            long startingBarcodeNumber = 0;
            long blockStart = 0;
            long blockEnd = 0;
            int noofBooks = 0;

            List<CustomerCatalogInformation> lstBarcodeCustCatInfo = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoID && e.Status == true
                                                                                                    && e.CatalogSubjectOptionValue.CatalogSubjectOption.CatalogSubjectID == 5
                                                                                                    && e.CatalogSubjectOptionValue.CatalogSubjectOption.ColumnType == 1).ToList();
          

            if (lstBarcodeCustCatInfo != null && lstBarcodeCustCatInfo.Count() > 0)
            {
                
                noofBooks = _context.QuoteDetail.GetAll(e => e.QuoteID == quoteid).Sum(e => e.Quantity);
                blockStart = checkandRetunValue(36, lstBarcodeCustCatInfo);
                startingBarcodeNumber = checkandRetunValue(35, lstBarcodeCustCatInfo);
                blockEnd = checkandRetunValue(74, lstBarcodeCustCatInfo);
                lastblockUsed = checkandRetunValue(101, lstBarcodeCustCatInfo);

                foreach (CustomerCatalogInformation barcodeCatInfo in lstBarcodeCustCatInfo.Where(e=>e.CatalogSubjectOptionValue.CatalogSubjectOptionID!=101 && e.CatalogSubjectOptionValue.CatalogSubjectOptionID!=75))
                {
                    lstccInfoModel.Add(new CreateCatalogInfoModel
                    {
                        CatalogSubjectOptionValueID = (int)barcodeCatInfo.CatalogSubjectOptionValueID,
                        Comments = barcodeCatInfo.Comments,
                        CustUserID = UserVM.CRMModelProperties.LoggedINCustomerUserID
                    });
                }
                if (blockStart > 0 && blockEnd > 0)
                {
                    //Last Block #Used Value Updation
                    //== StartingBarcodeNumber + (noofItemsin QUote) + 2

                    lastblockUsed = lastblockUsed!=0 ? lastblockUsed +( noofBooks + 2) : blockStart + 2 + noofBooks;
                    lstccInfoModel.Add(new CreateCatalogInfoModel
                    {
                        CatalogSubjectOptionValueID = _iCatInfoSrv.GetSubjectOptionvalueIDbySubjOptionID(101),
                        Comments = lastblockUsed.ToString(),
                        CustUserID = UserVM.CRMModelProperties.LoggedINCustomerUserID
                    });

                    //Next Avialble Block == Last Block Used + 1
                    nextblockavailble = lastblockUsed + 1;
                                      
                    lstccInfoModel.Add(new CreateCatalogInfoModel
                    {
                        CatalogSubjectOptionValueID = _iCatInfoSrv.GetSubjectOptionvalueIDbySubjOptionID(75),
                        Comments = lastblockUsed > blockEnd ? "ERROR, EXCEEDS BLOCK" : nextblockavailble.ToString(),
                        CustUserID = UserVM.CRMModelProperties.LoggedINCustomerUserID
                    });
                }
                
                _iCatInfoSrv.UserVM = UserVM;
                _iCatInfoSrv.UpdateStatusandInsertforCustomerCatalogInformation(lstBarcodeCustCatInfo, lstccInfoModel, custAutoID);
            }
        }

        private long checkandRetunValue(int catsubjOptvalueID, List<CustomerCatalogInformation> lstBarcodeCustCatInfo)
        {
            return lstBarcodeCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == catsubjOptvalueID).FirstOrDefault() != null ?
                                              string.IsNullOrEmpty(lstBarcodeCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == catsubjOptvalueID).FirstOrDefault().Comments) ?
                                              0 :Convert.ToInt64(lstBarcodeCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == catsubjOptvalueID).FirstOrDefault().Comments) : 0;
        }


    }
}
