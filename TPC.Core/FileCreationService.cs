using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;
using System.Configuration;
using System.Web;
using System.IO;
using TPC.Core.Models.Models;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;
using TPC.Common.Enumerations;

namespace TPC.Core
{
    public class FileCreationService : ServiceBase<IItemModel>, IFileCreationService
    {

        public bool FileCreation(SubmitQuoteViewModel sqvm)
        {
            FlatFileViewModel flatFileVM = new FlatFileViewModel();
            flatFileVM.UserVM = UserVM;
            flatFileVM.FlatHeaderModel = new FlatFileHeaderModel();
            flatFileVM.LstFlatDetailModel = new List<FlatFileDetailModel>();
            bool isAddInvRequired = (!string.IsNullOrEmpty(sqvm.AddInvRecipient) && sqvm.AddInvRecipient != "0") ? true : false;
            double dbValue = sqvm.DBValue;
            string orderSource = GetSourceNameBySourceType(sqvm.SourceType);
            int custAutoID = UserVM.CRMModelProperties.CustAutoID;
            flatFileVM.FlatHeaderModel = FillHeaderValues(sqvm, isAddInvRequired);
            flatFileVM.LstFlatDetailModel = FillDetailsValues(sqvm.QuoteID, custAutoID, dbValue, isAddInvRequired, orderSource);

            GenerateSubmittedQuoteCsvFiles(flatFileVM);
            return true;
        }

        public string FilePath
        {
            get { return ConfigurationManager.AppSettings["SubmittedQuotePath"]; }
        }

        private FlatFileHeaderModel FillHeaderValues(SubmitQuoteViewModel sqvm, bool isAddInvRequired)
        {
            FlatFileHeaderModel flatFHM = new FlatFileHeaderModel();
            flatFHM.catalogFileModel = new CatalogFlatFileModel();
            flatFHM.OrderDate = System.DateTime.Today.ToString("yyyyMMdd");
            flatFHM.CustomerPO = Convert.ToString(sqvm.PONo);
            flatFHM.OrderSubtype = sqvm.Type;
            flatFHM.FutureBillingdate = sqvm.FutureBillingDate == null ? string.Empty : sqvm.FutureBillingDate.Value.ToString("yyyyMMdd");
            flatFHM.CRMCommunicationID = "0";
            User userInfo = GetUserInfoUserID(Convert.ToInt32(UserVM.CRMModelProperties.LoggedINCustomerUserID));
            flatFHM.CustomerNumber = userInfo != null ? userInfo.Customer.CustomerNO : "0";
            flatFHM.InvoiceRecipient = userInfo != null ? userInfo.FirstName + " " + userInfo.LastName : "";
            flatFHM.ConfirmTo = userInfo != null ? userInfo.FirstName + " " + userInfo.LastName : "";
            string strComments = sqvm.Comments.Contains("\n") ? sqvm.Comments.Replace("\n", " ") : sqvm.Comments;
            strComments = strComments.Contains("\r") ? strComments.Replace("\r", " ") : sqvm.Comments;
            flatFHM.Comment = strComments.Contains(',') ? strComments.Replace(',', ' ') : strComments;
            flatFHM.DivisionNo = sqvm.Division;
            //Sales PersonID
            int userID = Convert.ToInt32(UserVM.CRMModelProperties.LoggedINUserID);
            int? repID = UserVM.CRMModelProperties.RepID;
            flatFHM.SalesPersonID = Convert.ToString(repID);
            flatFHM.QuoteNumber = Convert.ToString(sqvm.QuoteID);
            //flatFHM.DBValue = string.Format("{0:C}", sqvm.DBValue).Replace("$", "");
            if (isAddInvRequired)
            {
                if (!string.IsNullOrEmpty(sqvm.AddInvRecipient))
                {
                    Boolean shitpFlag = sqvm.AddInvRecipient.EndsWith("-S") ? true : false;
                    int custAutoShipID = shitpFlag == true ? Convert.ToInt32(sqvm.AddInvRecipient.Split('-')[0]) : Convert.ToInt32(sqvm.AddInvRecipient);
                    Customer InvcustInfo = GetCustomerInfoByCustomerID(custAutoShipID);

                    flatFHM.InvoiceRecipientName = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToName : InvcustInfo.CustomerName;
                    flatFHM.InvoiceRecipientAddress1 = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToAddress1 == null ? string.Empty : InvcustInfo.CustomerShipToAddress.ShipToAddress1 : InvcustInfo.CustomerAddress.AddressLine1 == null ? string.Empty : InvcustInfo.CustomerAddress.AddressLine1;
                    flatFHM.InvoiceRecipientAddress2 = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToAddress2 == null ? string.Empty : InvcustInfo.CustomerShipToAddress.ShipToAddress2 : InvcustInfo.CustomerAddress.AddressLine2 == null ? string.Empty : InvcustInfo.CustomerAddress.AddressLine2;
                    flatFHM.InvoiceRecipientAddress3 = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToAddress3 == null ? string.Empty : InvcustInfo.CustomerShipToAddress.ShipToAddress3 : InvcustInfo.CustomerAddress.AddressLine3 == null ? string.Empty : InvcustInfo.CustomerAddress.AddressLine3;
                    flatFHM.InvoiceRecipientCity = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToCity == null ? string.Empty : InvcustInfo.CustomerShipToAddress.ShipToCity : InvcustInfo.CustomerAddress.City == null ? string.Empty : InvcustInfo.CustomerAddress.City;
                    flatFHM.InvoiceRecipientState = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToState == null ? string.Empty : InvcustInfo.CustomerShipToAddress.ShipToState : InvcustInfo.CustomerAddress.State == null ? string.Empty : InvcustInfo.CustomerAddress.State;
                    flatFHM.InvoiceRecipientZip = shitpFlag == true ? InvcustInfo.CustomerShipToAddress.ShipToZipCode == null ? string.Empty : InvcustInfo.CustomerShipToAddress.ShipToZipCode : InvcustInfo.CustomerAddress.ZipCode == null ? string.Empty : InvcustInfo.CustomerAddress.ZipCode;
                }
            }

            if (sqvm.Payer != null && sqvm.Payer != 0)
            {
                Customer payerCustInfo = GetCustomerInfoByCustomerID(sqvm.Payer);
                flatFHM.BillToName = payerCustInfo.CustomerName;
                flatFHM.BillToAddress1 = payerCustInfo.CustomerAddress.AddressLine1 == null ? string.Empty : payerCustInfo.CustomerAddress.AddressLine1;
                flatFHM.BillToAddress2 = payerCustInfo.CustomerAddress.AddressLine2 == null ? string.Empty : payerCustInfo.CustomerAddress.AddressLine2;
                flatFHM.BillToAddress3 = payerCustInfo.CustomerAddress.AddressLine3 == null ? string.Empty : payerCustInfo.CustomerAddress.AddressLine3;
                flatFHM.BillToCity = payerCustInfo.CustomerAddress.City == null ? string.Empty : payerCustInfo.CustomerAddress.City;
                flatFHM.BillToState = payerCustInfo.CustomerAddress.State == null ? string.Empty : payerCustInfo.CustomerAddress.State;
                flatFHM.BillToZip = payerCustInfo.CustomerAddress.ZipCode == null ? string.Empty : payerCustInfo.CustomerAddress.ZipCode;
            }
            string strbillingcomments = sqvm.BillingReference.Contains("\n") ? sqvm.BillingReference.Replace("\n", "") : sqvm.BillingReference;
            strbillingcomments = strbillingcomments.Contains("\r") ? strbillingcomments.Replace("\r", "") : sqvm.BillingReference;

            flatFHM.BillingReference = strbillingcomments;

            if (!string.IsNullOrEmpty(sqvm.ShipItemsTo))
            {
                Boolean shitpFlag = sqvm.ShipItemsTo.EndsWith("-S") ? true : false;
                int custAutoShipID = shitpFlag == true ? Convert.ToInt32(sqvm.ShipItemsTo.Split('-')[0]) : Convert.ToInt32(sqvm.ShipItemsTo);
                Customer shiptoCustInfo = GetCustomerInfoByCustomerID(custAutoShipID);
                flatFHM.ShipToName = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToName : shiptoCustInfo.CustomerName;
                flatFHM.ShipToAddress1 = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToAddress1 == null ? string.Empty : shiptoCustInfo.CustomerShipToAddress.ShipToAddress1 : shiptoCustInfo.CustomerAddress.AddressLine1 == null ? string.Empty : shiptoCustInfo.CustomerAddress.AddressLine1;
                flatFHM.ShipToAddress2 = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToAddress2 == null ? string.Empty : shiptoCustInfo.CustomerShipToAddress.ShipToAddress2 : shiptoCustInfo.CustomerAddress.AddressLine2 == null ? string.Empty : shiptoCustInfo.CustomerAddress.AddressLine2;
                flatFHM.ShipToAddress3 = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToAddress3 == null ? string.Empty : shiptoCustInfo.CustomerShipToAddress.ShipToAddress3 : shiptoCustInfo.CustomerAddress.AddressLine3 == null ? string.Empty : shiptoCustInfo.CustomerAddress.AddressLine3;
                flatFHM.ShipToCity = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToCity == null ? string.Empty : shiptoCustInfo.CustomerShipToAddress.ShipToCity : shiptoCustInfo.CustomerAddress.City == null ? string.Empty : shiptoCustInfo.CustomerAddress.City;
                flatFHM.ShipToState = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToState == null ? string.Empty : shiptoCustInfo.CustomerShipToAddress.ShipToState : shiptoCustInfo.CustomerAddress.State == null ? string.Empty : shiptoCustInfo.CustomerAddress.State;
                flatFHM.ShipToZip = shitpFlag == true ? shiptoCustInfo.CustomerShipToAddress.ShipToZipCode == null ? string.Empty : shiptoCustInfo.CustomerShipToAddress.ShipToZipCode : shiptoCustInfo.CustomerAddress.ZipCode == null ? string.Empty : shiptoCustInfo.CustomerAddress.ZipCode;
            }

            return flatFHM;
        }

        private string GetSourceNameBySourceType(int sourceId)
        {
            if (sourceId != 0)
            {
                return _Context.SourceType.GetSingle(e => e.SourceID == sourceId).SourceName;
            }
            return string.Empty;
        }

        private List<FlatFileDetailModel> FillDetailsValues(int quoteID, int custAutoID, double dbValue, bool isAddInvRequired = false, string orderSource = "")
        {
            List<FlatFileDetailModel> lstDetailValues = new List<FlatFileDetailModel>();
            List<QuoteDetail> lstQuoteDetail = GetQuoteDetails(quoteID);
            bool includeCatalog = CatalogStatus(quoteID);
            lstDetailValues = lstQuoteDetail.Select(e => new FlatFileDetailModel
            {
                ItemNumber = e.ItemID,
                ItemPrice = Convert.ToDouble(e.Item.Price),
                Price = Convert.ToDouble(e.Item.Price * e.Quantity),
                Quantity = e.Quantity,
                AddInvRecptFlag = isAddInvRequired ? "Y" : "N",
                OrderSource = orderSource,
                Cataloging = includeCatalog ? "Y" : "N"
            }).ToList();
            int noofBooksCount = lstQuoteDetail.Where(e => (e.Item.ProductLine.Trim() != "PRO" && e.Item.ProductLine.Trim() != "PUP")).Sum(x => x.Quantity);

            if (noofBooksCount > 0)
            {
                if (includeCatalog)
                {
                    List<FlatFileDetailModel> lstCatalogInfovalues = FillCatalogInfovalues(noofBooksCount, custAutoID, "File", isAddInvRequired, orderSource, quoteID);
                    //FlatFileDetailModel flatFileCatalogProtectorInfo = FillCatalogProtectorValues(noofBooksCount, custAutoID, "File", isAddInvRequired, orderSource, quoteID);
                    //FlatFileDetailModel flatfileCatalogShelfReadyInfo = FillCatalogShelfReadyValues(noofBooksCount, custAutoID, "File", isAddInvRequired, orderSource, quoteID);
                    List<FlatFileDetailModel> lstCatalogSpecialInfovalues = FillCatalogSpecialChargeValues(noofBooksCount, custAutoID, "File", isAddInvRequired, orderSource, quoteID);
                    if (lstCatalogInfovalues != null && lstCatalogInfovalues.Count() > 0) lstDetailValues.AddRange(lstCatalogInfovalues);
                    //if (flatFileCatalogProtectorInfo != null && flatFileCatalogProtectorInfo.Quantity != 0) lstDetailValues.Add(flatFileCatalogProtectorInfo);
                    //if (flatfileCatalogShelfReadyInfo != null) lstDetailValues.Add(flatfileCatalogShelfReadyInfo);
                    if (lstCatalogSpecialInfovalues != null && lstCatalogSpecialInfovalues.Count() > 0) lstDetailValues.AddRange(lstCatalogSpecialInfovalues);
                }
                FlatFileDetailModel ffDBValueInfoValues = FillDBValueInfo(dbValue, quoteID, noofBooksCount, isAddInvRequired, orderSource);
                if (ffDBValueInfoValues != null) lstDetailValues.Add(ffDBValueInfoValues);
            }

            return lstDetailValues;
        }

        private string GenerateText(Object obj)
        {
            string strColumnNames = string.Empty;
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                if ((DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() != null && ((DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name != string.Empty)
                {
                    string annonationName = ((DisplayAttribute)property.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name;
                    if (annonationName != "Catalog")
                        strColumnNames += (annonationName + '|');
                }
            }
            strColumnNames = strColumnNames.Remove(strColumnNames.Length - 1, 1);
            return strColumnNames;
        }

        private void GenerateSubmittedQuoteCsvFiles(FlatFileViewModel flatFileVM)
        {
            string filePathWithName = FilePath + GetFilename(flatFileVM.FlatHeaderModel.DivisionNo, flatFileVM.FlatHeaderModel.CustomerNumber, flatFileVM.FlatHeaderModel.QuoteNumber);
            using (StreamWriter writer =
     new StreamWriter(filePathWithName, false))
            {
                if (flatFileVM.LstFlatDetailModel.FirstOrDefault().Cataloging == "Y")
                {
                    writer.WriteLine(GenerateText(new FlatFileHeaderModel()) + '|' + GenerateText(new FlatFileDetailModel()) + '|' + GenerateText(new CatalogFlatFileModel()));
                    foreach (FlatFileDetailModel flatfileDM in flatFileVM.LstFlatDetailModel)
                    {
                        writer.WriteLine(PrepareFileValuesforcsv(flatFileVM.FlatHeaderModel) + '|' + PrepareFileValuesforcsv(flatfileDM) + '|' + PrepareFileValuesforCatalogcsv(flatFileVM.FlatHeaderModel.catalogFileModel, flatFileVM.UserVM.CRMModelProperties.CustAutoID));
                    }
                }
                else
                {
                    writer.WriteLine(GenerateText(new FlatFileHeaderModel()) + '|' + GenerateText(new FlatFileDetailModel()));
                    foreach (FlatFileDetailModel flatfileDM in flatFileVM.LstFlatDetailModel)
                    {
                        writer.WriteLine(PrepareFileValuesforcsv(flatFileVM.FlatHeaderModel) + '|' + PrepareFileValuesforcsv(flatfileDM));
                    }
                }

            }

        }


        private string PrepareFileValuesforCatalogcsv(CatalogFlatFileModel obj, int custAutoId)
        {
            List<CustomerCatalogInformation> lstCustomerCatalogInformation = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoId && e.CatalogSubjectOptionValue.CatalogSubjectOption.CatalogSubjectID != 16 && e.Status == true).OrderBy(e => e.CatalogSubjectOptionValue.Priority).ToList();
            StringBuilder sb = new StringBuilder();
            string strobjValues = string.Empty;
            string strSubjectValue = string.Empty;
            //
            foreach (PropertyInfo propinfo in obj.GetType().GetProperties())
            {
                if ((DisplayAttribute)propinfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() != null && ((DisplayAttribute)propinfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name != string.Empty)
                {
                    int CatalogSubjectOptionid = Convert.ToInt32(((DisplayAttribute)propinfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName);
                    CustomerCatalogInformation custCatInfo = lstCustomerCatalogInformation.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == CatalogSubjectOptionid && e.CatalogSubjectOptionValueID == e.CatalogSubjectOptionValue.CatalogSubjectOptionValueID).FirstOrDefault();
                    if (CatalogSubjectOptionid == 9)
                    { 
                        strSubjectValue = lstCustomerCatalogInformation != null && lstCustomerCatalogInformation.Count() > 1 ? 
                            string.Join(";", lstCustomerCatalogInformation.Where(e => 
                                e.CatalogSubjectOptionValue.CatalogSubjectOptionID == CatalogSubjectOptionid).Select(e => 
                                    e.CatalogSubjectOptionValue.CatalogSubjectOptionValue1)) : 
                                    custCatInfo != null ? custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOptionValue1 
                                    : string.Empty; 
                    }

                    strobjValues = lstCustomerCatalogInformation != null && lstCustomerCatalogInformation.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == CatalogSubjectOptionid).Count() > 0 ?
                     custCatInfo != null ? custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ColumnType == (int)CatalogColumnTypeEnum.TextBox
                          || custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ColumnType == (int)CatalogColumnTypeEnum.TextArea ?
                      RemovingSpecialCharacter(custCatInfo.Comments) : custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOptionValueID == 175 ? RemovingSpecialCharacter(custCatInfo.Comments)
                      : CatalogSubjectOptionid==9 ? lstCustomerCatalogInformation.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == 9).Count() > 1 ?
                      strSubjectValue : custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOptionValue1 : custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOptionValue1
                      : string.Empty : string.Empty;
                    sb.Append(strobjValues).Append("|");
                }
            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1);
        }

        public string RemovingSpecialCharacter(string strobjValues)
        {
            string strRemovingSpCh = string.Empty;
            if (!string.IsNullOrEmpty(strobjValues))
            {
                strRemovingSpCh = strobjValues.Contains("\n") ? strobjValues.Replace("\n", " ").TrimEnd() : strobjValues;
                strRemovingSpCh = strRemovingSpCh.Contains("\r") ? strRemovingSpCh.Replace("\r", " ").TrimEnd() : strRemovingSpCh;
                strRemovingSpCh = strRemovingSpCh.Contains(",") ? strRemovingSpCh.Replace(",", ";") : strRemovingSpCh;
            }
            return strRemovingSpCh;
           
        }


        private string GetFilename(string divID, string custID, string quoteNo)
        {
            return divID.ToString() + "_" + custID + "_" + quoteNo + ".csv";
        }

        private string PrepareFileValuesforcsv(Object obj)
        {
            StringBuilder sb = new StringBuilder();
            string strobjValues = string.Empty;
            //
            foreach (PropertyInfo propinfo in obj.GetType().GetProperties())
            {
                if ((DisplayAttribute)propinfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault() != null && ((DisplayAttribute)propinfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name != string.Empty)
                {
                    if (((DisplayAttribute)propinfo.GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name != "Catalog")
                    {
                        strobjValues = obj.GetType().GetProperty(propinfo.Name).GetValue(obj, null) == null ?
                            string.Empty : (obj.GetType().GetProperty(propinfo.Name).GetValue(obj, null).ToString() == "0") ?
                            string.Empty : obj.GetType().GetProperty(propinfo.Name).GetValue(obj, null).ToString().Contains(",") ?
                            obj.GetType().GetProperty(propinfo.Name).GetValue(obj, null).ToString().Replace(',', ' ') : obj.GetType().GetProperty(propinfo.Name).GetValue(obj, null).ToString();

                        sb.Append(strobjValues).Append("|");
                    }
                }
            }
            return sb.ToString().Substring(0, sb.ToString().Length - 1);
        }

        private Customer GetCustomerInfobyUserID(int userID)
        {
            int cutAutoID = (int)_Context.User.GetSingle(e => e.UserId == userID).CustAutoID;
            return GetCustomerInfoByCustomerID(cutAutoID);
        }

        private User GetUserInfoUserID(int userID)
        {
            return _Context.User.GetSingle(e => e.UserId == userID);
        }

        private Customer GetCustomerInfoByCustomerID(int custAutoID)
        {
            return _Context.Customer.GetSingle(e => e.CustAutoID == custAutoID);
        }

        private string GetQuoteTypeTextbyQuoteType(int quoteTypeID)
        {
            return _Context.QuoteType.GetSingle(e => e.QuoteTypeID == quoteTypeID).QuoteTypeText;
        }



        #region Catalog


        public FlatFileDetailModel FillDBValueInfo(double DBValue, int quoteID, int noofBooks, bool isAddInvRequired = false, string orderSource = "")
        {
            FlatFileDetailModel ffDBValueInfo = new FlatFileDetailModel();
            ffDBValueInfo.ItemNumber = "/DB";
            ffDBValueInfo.Quantity = noofBooks;
            ffDBValueInfo.ItemPrice = 0;
            ffDBValueInfo.Price = DBValue;
            ffDBValueInfo.AddInvRecptFlag = isAddInvRequired ? "Y" : "N";
            ffDBValueInfo.OrderSource = orderSource;
            ffDBValueInfo.Cataloging = CatalogStatus(quoteID) ? "Y" : "N";
            return ffDBValueInfo;
        }

        public bool CatalogStatus(int quoteID)
        {
            if (quoteID != 0)
            {
                Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
                return quote == null ? false : (bool)quote.IncludeCatalogStatus;
            }
            return false;
        }
        string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
        string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
        public List<QuoteDetail> GetQuoteDetails(int quoteID)
        {
            return _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteID && (e.Item.Status == statusenumB || e.Item.Status == statusenumD)).ToList();
        }


        public List<FlatFileDetailModel> FillCatalogInfovalues(int noofBooksCount, int custAutoID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0)
        {
            List<FlatFileDetailModel> lstCatalogFaltInfoM = new List<FlatFileDetailModel>();
            ICatalogInfoService catalogInfoSrv = new CatalogInfoService();
            List<QuoteDetail> lstquoteDetails = GetQuoteDetails(quoteID);
            int nooftitles = quoteID != 0 ? lstquoteDetails.Where(e => (e.Item.ProductLine.Trim() != "PRO" && e.Item.ProductLine.Trim() != "PUP")).Count() : 0;
            List<CustomerCatalogInformation> lstCustomerCatalogInfo = GetCatalogProtector_ShelfReadyInfoValues(custAutoID);
            if (lstCustomerCatalogInfo.Count > 0)
                lstCustomerCatalogInfo = lstCustomerCatalogInfo.GroupBy(e => e.CatalogSubjectOptionValueID).Select(e => e.FirstOrDefault()).ToList();

            if (lstCustomerCatalogInfo != null)
            {
                foreach (CustomerCatalogInformation custCatalogInfo in lstCustomerCatalogInfo)
                {
                    if (custCatalogInfo != null)
                    {
                        int customerCatID = custCatalogInfo.CustomerCatID;
                        int catSubjOptID = (int)custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOptionID;

                        CatalogSubjectItemIDMapping catSubjItemMapping = GetCatalogSubjectMappingByCatalogInfo(catSubjOptID, custCatalogInfo);
                        if (catSubjItemMapping != null)
                        {
                            FlatFileDetailModel ffDMCatInfo = new FlatFileDetailModel();
                            ffDMCatInfo.ItemNumber = strInsertionType == "File" ? catSubjItemMapping.Item.ItemID : catSubjItemMapping.Item.Title;
                            ffDMCatInfo.ProductLine = catSubjItemMapping.Item.ProductLine;
                            ffDMCatInfo.ItemCode = catSubjItemMapping.ItemID;
                            ffDMCatInfo.LevelType =
                             custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3150" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3154" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3151" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3250" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3251" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3155" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3156" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3254" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3255" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3256" ? LevelTypeEnums.AR.ToString() :
                             custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3160" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3164" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3161" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3260" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3261" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3165" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3168" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3264" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3265" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3268" ? LevelTypeEnums.RC.ToString() :
                             custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3166" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3167" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3266" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3267" ?
                              LevelTypeEnums.Lexile.ToString() : string.Empty;

                            ffDMCatInfo.ItemPrice = Convert.ToDouble(catSubjItemMapping.Item.Price);
                            ffDMCatInfo.Quantity = custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.Item.ProductLine == "CATB" ? 
                                                   custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3150" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3154" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3151" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3250" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3251" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3155" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3156" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3254" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3255" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3256" ?
                                                   GetTitlesCountByLevelType(LevelTypeEnums.AR.ToString(), quoteID) :
                                                   custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3160" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3164" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3161" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3260" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3261" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3165" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3168" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3264" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3265" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3268" ?
                                                   GetTitlesCountByLevelType(LevelTypeEnums.RC.ToString(), quoteID) :
                                                   custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3166" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3167" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3266" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3267" ?
                                                   GetTitlesCountByLevelType(LevelTypeEnums.Lexile.ToString(), quoteID) :
                                                   noofBooksCount : nooftitles;
                            ffDMCatInfo.Price = custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.Item.ProductLine == "CATB" ?
                                                     custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3150" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3154" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3151" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3250" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3251" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3155" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3156" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3254" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3255" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3256" ?
                                                    ffDMCatInfo.ItemPrice * GetTitlesCountByLevelType(LevelTypeEnums.AR.ToString(), quoteID) :
                                                   custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3160" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3164" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3161" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3260" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3261" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3165" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3168" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3264" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3265" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3268" ?
                                                    ffDMCatInfo.ItemPrice * GetTitlesCountByLevelType(LevelTypeEnums.RC.ToString(), quoteID) :
                                                    custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3166" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3167" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3266" || custCatalogInfo.CatalogSubjectOptionValue.CatalogSubjectOption.ItemID == "3267" ?
                                                    ffDMCatInfo.ItemPrice * GetTitlesCountByLevelType(LevelTypeEnums.Lexile.ToString(), quoteID) :
                                                    ffDMCatInfo.ItemPrice * noofBooksCount :
                                                    ffDMCatInfo.ItemPrice * nooftitles;
                            ffDMCatInfo.AddInvRecptFlag = isAddInvRequired ? "Y" : "N";
                            ffDMCatInfo.OrderSource = orderSource;
                            ffDMCatInfo.Cataloging = CatalogStatus(quoteID) ? "Y" : "N";
                            lstCatalogFaltInfoM.Add(ffDMCatInfo);
                        }
                    }
                }
            }
            lstCatalogFaltInfoM.RemoveAll(e => e.Quantity == 0);
            return lstCatalogFaltInfoM;
        }

        /// <summary>
        /// Getting the count of Titles which having Ar Level and RC Level
        /// </summary>
        /// <param name="levelType"></param>
        /// //AR , RC
        /// <returns></returns>
        private int GetTitlesCountByLevelType(string levelType, int quoteID)
        {
            int titlesCount = 0;
            if (quoteID != 0)
            {
                Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
                if (quote != null)
                {
                    if (levelType == LevelTypeEnums.AR.ToString())
                    {
                        titlesCount = quote.QuoteDetails.Where(e => e.Item.ARLevel != null && (e.Item.Status == statusenumB || e.Item.Status == statusenumD) && (e.Item.ProductLine.Trim() != "PRO" && e.Item.ProductLine.Trim() != "PUP")).Count();
                    }
                    else if (levelType == LevelTypeEnums.RC.ToString())
                    {
                        titlesCount = quote.QuoteDetails.Where(e => e.Item.RCLevel != null && (e.Item.Status == statusenumB || e.Item.Status == statusenumD) && (e.Item.ProductLine.Trim() != "PRO" && e.Item.ProductLine.Trim() != "PUP")).Count();
                    }
                    else if (levelType == LevelTypeEnums.Lexile.ToString())
                    {
                        titlesCount = quote.QuoteDetails.Where(e => e.Item.Lexile != null && (e.Item.Status == statusenumB || e.Item.Status == statusenumD) && (e.Item.ProductLine.Trim() != "PRO" && e.Item.ProductLine.Trim() != "PUP")).Count();
                    }

                }

            }
            return titlesCount;
        }

        private CatalogSubjectItemIDMapping GetCatalogSubjectMappingByCatalogInfo(int catalogSubjectOptID, CustomerCatalogInformation customerCatalogInfo)
        {
            ICatalogInfoService catalogInfoSrv = new CatalogInfoService();
            CustomerCatalogProtectorInformation custcatProInfo = customerCatalogInfo.CustomerCatalogProtectorInformations.Where(x => x.CCatID == customerCatalogInfo.CustomerCatID && x.Status == true).FirstOrDefault();
            bool isExtraProtectorChecked = customerCatalogInfo.CustomerCatalogProtectorInformations.Where(x => x.CCatID == customerCatalogInfo.CustomerCatID && x.Status == true).Count() > 1 ? true : false;
            CustomerCatalogShelfReadyInformation custcatSRInfo = customerCatalogInfo.CustomerCatalogShelfReadyInformations.Where(x => x.CCatID == customerCatalogInfo.CustomerCatID && x.Status == true).FirstOrDefault();
            return catalogInfoSrv.GetCatalogSubjectMappingBySelectionofCatalogItem(catalogSubjectOptID, true, custcatProInfo != null ? true : false, isExtraProtectorChecked, custcatSRInfo != null ? true : false);
        }

        public FlatFileDetailModel FillCatalogProtectorValues(int noofBooksCount, int custAutoID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0)
        {
            FlatFileDetailModel flatfileProtectorCatalogM = null;

            List<CustomerCatalogInformation> lstCustomerCatalogInfo = GetCatalogProtector_ShelfReadyInfoValues(custAutoID);
            List<CustomerCatalogProtectorInformation> lstCCProtectorInfo = lstCustomerCatalogInfo.Select(e => e.CustomerCatalogProtectorInformations.Where(s => s.Status == true).FirstOrDefault()).ToList();
            lstCCProtectorInfo.RemoveAll(e => e == null);

            if (lstCustomerCatalogInfo != null && lstCustomerCatalogInfo.Count() > 0)
            {
                flatfileProtectorCatalogM = new FlatFileDetailModel();
                int extraProtectorValue = 0;
                if (lstCustomerCatalogInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors).Count() > 0)
                {
                    if (!string.IsNullOrEmpty(lstCustomerCatalogInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors).FirstOrDefault().Comments.Trim()))
                    {
                        extraProtectorValue = Convert.ToInt32(lstCustomerCatalogInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors).FirstOrDefault().Comments);
                    }
                }

                int checkedCountforProtector = lstCCProtectorInfo.Count();
                double dblUnitPrice = 0;

                //For "Flat File" --Qunatity is Calculated As -- ( total number of books) X (number of boxes checked) + (the quantity in the additional Protectors box)
                //For Shopping Cart or For Quote View --- Quantity - total number of books
                int quantity = (noofBooksCount * checkedCountforProtector) + extraProtectorValue;

                double dblProtectorPrice = lstCCProtectorInfo.Select(e => Convert.ToDouble(e.CatalogSubjectOptionProtectorValue.CSOProtectorValue)).FirstOrDefault();
                dblUnitPrice = dblProtectorPrice;
                if (strInsertionType != "File")
                {
                    //Unit Price is calculated as -- (Sum of Protectors Checked * 0.12) + Sum of Prices of Cataloging Items Checked (like Electronic Record or Barcode Items ect..)
                    double sumofProtectorschecked = (checkedCountforProtector * dblProtectorPrice);
                    double sumofCatalogingItemsChecked = lstCustomerCatalogInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOption.CatalogSubjectID == 16 && e.CatalogSubjectOptionValue.CatalogSubjectOptionValue1 != string.Empty).Sum(s => Convert.ToDouble(s.CatalogSubjectOptionValue.CatalogSubjectOptionValue1));
                    dblUnitPrice = sumofProtectorschecked + sumofCatalogingItemsChecked;

                    quantity = noofBooksCount;
                }
                if (quantity > 0)
                {
                    flatfileProtectorCatalogM.ItemNumber = GetItemNumberByCatalogSubjectOptionID((int)CatalogSubjectOptionEnums.Protector, strInsertionType);
                    flatfileProtectorCatalogM.ItemPrice = dblUnitPrice;
                    flatfileProtectorCatalogM.Quantity = quantity;
                    flatfileProtectorCatalogM.Price = flatfileProtectorCatalogM.ItemPrice * flatfileProtectorCatalogM.Quantity;
                    flatfileProtectorCatalogM.AddInvRecptFlag = isAddInvRequired ? "Y" : "N";
                    flatfileProtectorCatalogM.OrderSource = orderSource;
                    flatfileProtectorCatalogM.Cataloging = CatalogStatus(quoteID) ? "Y" : "N";
                }
            }
            return flatfileProtectorCatalogM;

        }

        public FlatFileDetailModel FillCatalogShelfReadyValues(int noofBooksCount, int custAutoID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0)
        {
            FlatFileDetailModel flatfileShelfReadyCatalogM = null;

            List<CustomerCatalogInformation> lstCustomerCatalogInfo = GetCatalogProtector_ShelfReadyInfoValues(custAutoID);
            List<CustomerCatalogShelfReadyInformation> lstCCShelfReadyInfo = lstCustomerCatalogInfo.Select(e => e.CustomerCatalogShelfReadyInformations.Where(s => s.Status == true).FirstOrDefault()).ToList();
            lstCCShelfReadyInfo.RemoveAll(e => e == null);

            if (lstCCShelfReadyInfo != null && lstCCShelfReadyInfo.Count() > 0)
            {
                flatfileShelfReadyCatalogM = new FlatFileDetailModel();
                double dblUnitPrice = 0;
                int shelfReadyCheckedCount = lstCCShelfReadyInfo.Sum(e => e.CatalogSubjectOptionShelfReadyValue.CatalogSubjectoptionShelfReady.ShelfReadyCount);

                int extraShlefReadyCount = 0;

                foreach (CustomerCatalogInformation custCatInfo in lstCustomerCatalogInfo)
                {
                    if (custCatInfo != null)
                    {
                        CatalogSubjectItemIDMapping catSubjItemMapping = GetCatalogSubjectMappingByCatalogInfo((int)custCatInfo.CatalogSubjectOptionValue.CatalogSubjectOptionID, custCatInfo);
                        if (catSubjItemMapping != null)
                        {
                            if ((bool)catSubjItemMapping.IsCLabelChecked && ((bool)catSubjItemMapping.ISCSubjProtectorChecked || (bool)catSubjItemMapping.IsCExtraProtectorChecked) && (bool)catSubjItemMapping.IsCSubjSRChecked)
                            {
                                extraShlefReadyCount += 1;
                            }
                        }
                    }
                }

                shelfReadyCheckedCount = shelfReadyCheckedCount + extraShlefReadyCount;
                //For "Flat File" --Qunatity is Calculated As -- ( total number of books) X ((number of boxes checked remembering to add two for 3100, 3144, 3154, 3164 if checked)
                //For Shopping Cart or For Quote View --- Quantity - total number of books
                int quantity = (noofBooksCount * shelfReadyCheckedCount);



                double dblShelfReadyPrice = lstCCShelfReadyInfo.Select(e => Convert.ToDouble(e.CatalogSubjectOptionShelfReadyValue.CSOShelfReadyValue)).FirstOrDefault();
                dblUnitPrice = dblShelfReadyPrice;
                if (strInsertionType != "File")
                {
                    //Unit Price is calculated as -- sum of shelf ready units in column 4   remembering to add two for 3100, 3144, 3154, 3164 if checked)
                    double sumofShelfReadyItemsChecked = shelfReadyCheckedCount * dblShelfReadyPrice;
                    dblUnitPrice = sumofShelfReadyItemsChecked;

                    quantity = noofBooksCount;
                }

                flatfileShelfReadyCatalogM.ItemNumber = GetItemNumberByCatalogSubjectOptionID((int)CatalogSubjectOptionEnums.ShelfReadyUnit, strInsertionType);
                flatfileShelfReadyCatalogM.ItemPrice = dblUnitPrice;
                flatfileShelfReadyCatalogM.Quantity = quantity;
                flatfileShelfReadyCatalogM.Price = flatfileShelfReadyCatalogM.ItemPrice * flatfileShelfReadyCatalogM.Quantity;
                flatfileShelfReadyCatalogM.AddInvRecptFlag = isAddInvRequired ? "Y" : "N";
                flatfileShelfReadyCatalogM.OrderSource = orderSource;
                flatfileShelfReadyCatalogM.Cataloging = CatalogStatus(quoteID) ? "Y" : "N";
            }
            return flatfileShelfReadyCatalogM;

        }

        public List<FlatFileDetailModel> FillCatalogSpecialChargeValues(int noofBooksCount, int custAutoID, string strInsertionType, bool isAddInvRequired = false, string orderSource = "", int quoteID = 0)
        {
            List<FlatFileDetailModel> lstCatalogFaltInfoM = new List<FlatFileDetailModel>();
            List<CustomerCatalogInformation> lstCustomerCatalogInfo = GetCatalogProtector_ShelfReadyInfoValues(custAutoID);

            if (lstCustomerCatalogInfo != null)
            {
                //if (strInsertionType == "File")
                //{
                //    lstCustomerCatalogInfo = lstCustomerCatalogInfo.Where(e => (e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialBulkCharge
                //                                                            || e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialPerBookCharge)
                //                                                            && e.Comments != null && e.Comments != string.Empty).ToList();
                //}
                //else
                //{
                lstCustomerCatalogInfo = lstCustomerCatalogInfo.Where(e => (e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialBulkCharge ||
                                                                            e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialPerBookCharge) ||
                                                                            e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors
                                                                                                   && e.Comments != null && e.Comments != string.Empty).ToList();
                //}
                double unitPriceofProtectors = GetUnitPriceforProtectors();
                if (lstCustomerCatalogInfo != null && lstCustomerCatalogInfo.Count() > 0)
                {
                    lstCatalogFaltInfoM = lstCustomerCatalogInfo.Select(e => new FlatFileDetailModel
                    {
                        ItemNumber = GetItemNumberByCatalogSubjectOptionID(Convert.ToInt32(e.CatalogSubjectOptionValue.CatalogSubjectOptionID), strInsertionType),
                        ItemPrice = e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialBulkCharge ?
                                                string.IsNullOrEmpty(e.Comments) ? (double)0 : Convert.ToDouble(e.Comments) :
                                                 e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors ? unitPriceofProtectors :
                                                string.IsNullOrEmpty(e.Comments) ? (double)0 : Convert.ToDouble(e.Comments),
                        Quantity = e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialPerBookCharge ? noofBooksCount
                                                                : e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors ? string.IsNullOrEmpty(e.Comments) ? 1 : Convert.ToInt32(e.Comments)
                                                                : e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialBulkCharge ? 1 : noofBooksCount,
                        Price = strInsertionType == "File" ? e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.SpecialBulkCharge ?
                                                                string.IsNullOrEmpty(e.Comments) ? (double)0 : Convert.ToDouble(e.Comments)
                                                                : e.CatalogSubjectOptionValue.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.ExtraProtectors ? Convert.ToDouble(e.Comments) * unitPriceofProtectors
                                                                : Convert.ToDouble(e.Comments) * noofBooksCount
                                                                : string.IsNullOrEmpty(e.Comments) ? (double)0 : Convert.ToDouble(e.Comments) * unitPriceofProtectors,
                        AddInvRecptFlag = isAddInvRequired ? "Y" : "N",
                        OrderSource = orderSource,
                        Cataloging = CatalogStatus(quoteID) ? "Y" : "N"
                    }).ToList();
                }
            }
            return lstCatalogFaltInfoM;
        }


        private double GetUnitPriceforProtectors()
        {
            List<CatalogSubjectOptionValue> lstCatalogOptionValues = _Context.CatalogSubjectOptionValue.GetAll().ToList();
            return Convert.ToDouble(lstCatalogOptionValues.Where(e => e.CatalogSubjectOption.CatalogSubjectOptionID == (int)CatalogSubjectOptionEnums.Protector).FirstOrDefault().CatalogSubjectOptionValue1);
        }


        public List<CustomerCatalogInformation> GetCatalogProtector_ShelfReadyInfoValues(int custAutoID)
        {
            return _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoID && e.CatalogSubjectOptionValue.CatalogSubjectOption.CatalogSubject.CatalogInfoID == 6 && e.Status == true).ToList();
        }

        public string GetItemNumberByCatalogSubjectOptionID(int catalogSubjectOptionID, string strInsertionType)
        {
            string strItemNumber = "0";
            switch (catalogSubjectOptionID)
            {
                case 44:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.ElectronicRecord.GetType().GetField(CatalogSubjectOptionEnums.ElectronicRecord.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 48:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.BarcodeLabel.GetType().GetField(CatalogSubjectOptionEnums.BarcodeLabel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 49:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.secondBarcodeLabel.GetType().GetField(CatalogSubjectOptionEnums.secondBarcodeLabel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;

                    }
                case 50:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.ShelfListCard.GetType().GetField(CatalogSubjectOptionEnums.ShelfListCard.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 51:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.SpineLabel.GetType().GetField(CatalogSubjectOptionEnums.SpineLabel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 52:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.CatalogCardKit.GetType().GetField(CatalogSubjectOptionEnums.CatalogCardKit.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 53:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.PeelStickPocket.GetType().GetField(CatalogSubjectOptionEnums.PeelStickPocket.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 54:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.SpinePocketLabelSet.GetType().GetField(CatalogSubjectOptionEnums.SpinePocketLabelSet.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 55:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.CheckoutCard.GetType().GetField(CatalogSubjectOptionEnums.CheckoutCard.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 57:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.ARSpineLabel.GetType().GetField(CatalogSubjectOptionEnums.ARSpineLabel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 58:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.ARLabelSet.GetType().GetField(CatalogSubjectOptionEnums.ARLabelSet.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 59:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.RCSpineLabel.GetType().GetField(CatalogSubjectOptionEnums.RCSpineLabel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 60:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.RCLabelSet.GetType().GetField(CatalogSubjectOptionEnums.RCLabelSet.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 61:
                    {
                        return ((DisplayAttribute)CatalogSubjectOptionEnums.LexileSpineLabel.GetType().GetField(CatalogSubjectOptionEnums.LexileSpineLabel.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName;
                    }
                case 62:
                    {
                        return strInsertionType == "File" ? ((DisplayAttribute)CatalogSubjectOptionEnums.Protector.GetType().GetField(CatalogSubjectOptionEnums.Protector.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName :
                                                            ((DisplayAttribute)CatalogSubjectOptionEnums.Protector.GetType().GetField(CatalogSubjectOptionEnums.Protector.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name;
                    }
                case 63:
                    {
                        return strInsertionType == "File" ? ((DisplayAttribute)CatalogSubjectOptionEnums.ShelfReadyUnit.GetType().GetField(CatalogSubjectOptionEnums.ShelfReadyUnit.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName
                            : ((DisplayAttribute)CatalogSubjectOptionEnums.ShelfReadyUnit.GetType().GetField(CatalogSubjectOptionEnums.ShelfReadyUnit.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name;
                    }
                case 64:
                    {
                        return strInsertionType == "File" ? ((DisplayAttribute)CatalogSubjectOptionEnums.ExtraProtectors.GetType().GetField(CatalogSubjectOptionEnums.ExtraProtectors.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName :
                                                           ((DisplayAttribute)CatalogSubjectOptionEnums.ExtraProtectors.GetType().GetField(CatalogSubjectOptionEnums.ExtraProtectors.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name;
                    }
                case 66:
                    {

                        return strInsertionType == "File" ? ((DisplayAttribute)CatalogSubjectOptionEnums.SpecialPerBookCharge.GetType().GetField(CatalogSubjectOptionEnums.SpecialPerBookCharge.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName :
                                                     ((DisplayAttribute)CatalogSubjectOptionEnums.SpecialPerBookCharge.GetType().GetField(CatalogSubjectOptionEnums.SpecialPerBookCharge.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name; ;

                    }
                case 68:
                    {
                        return strInsertionType == "File" ? ((DisplayAttribute)CatalogSubjectOptionEnums.SpecialBulkCharge.GetType().GetField(CatalogSubjectOptionEnums.SpecialBulkCharge.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).ShortName :
                                                        ((DisplayAttribute)CatalogSubjectOptionEnums.SpecialBulkCharge.GetType().GetField(CatalogSubjectOptionEnums.SpecialBulkCharge.ToString()).GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault()).Name;
                    }
                default:
                    {
                        return strItemNumber;
                    }
            }
        }
        #endregion
        public bool CalTagFileCreation(CalTagViewModel calTagVM)
        {
            CallTagFileViewModel calTagFilVM = FillCalTagModel(calTagVM);
            GenerateCalTaagCsvFiles(calTagFilVM);
            return true;
        }
        private CallTagFileViewModel FillCalTagModel(CalTagViewModel calTagVM)
        {
            // int custAutoId = Convert.ToInt32(calTagVM.InvoiceTo);
            // Customer shipToCusAddress = _Context.Customer.GetSingle(e => e.CustAutoID == custAutoId);
            //if (shipToCusAddress != null)
            //{
            //    calTagVM.ShipToName = shipToCusAddress.CustomerName;

            //}
            //Customer cusAddress = _Context.Customer.GetSingle(e => e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            //if (cusAddress != null && cusAddress.CustomerAddress != null)
            //{
            //    calTagVM.PostalCode = cusAddress.CustomerAddress.ZipCode;
            //    calTagVM.AddressLine2 = cusAddress.CustomerAddress.AddressLine2;
            //    calTagVM.AddressLine3 = cusAddress.CustomerAddress.AddressLine3;
            //    calTagVM.City = cusAddress.CustomerAddress.City;
            //    calTagVM.State = cusAddress.CustomerAddress.State;
            //    calTagVM.PrimaryContact = cusAddress.CustomerName;

            //}
            //Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == calTagVM.QuoteID);
            //if (quote != null)
            //{
            //    calTagVM.ActualWeight = quote.QuoteDetails != null && quote.QuoteDetails.Count() > 0 ? quote.QuoteDetails.Sum(e => e.Quantity) * 1 : 0;
            //}
            //else
            //{
            //    calTagVM.ActualWeight = 0;
            //}
            //calTagVM.MerchandiseDescription = "LIBY BOOKS";
            //calTagVM.Code = "MSCT";
            //calTagVM.ReturnSrvType = calTagVM.LstSendOptions.FirstOrDefault() != null ? calTagVM.LstSendOptions.FirstOrDefault().ItemID == "2" ? "ERL" : "RS1" : "RS1";
            CallTagFileViewModel calTagFileVM = new CallTagFileViewModel();
            calTagFileVM.FutureDate = DateTime.Now;
            calTagFileVM.QuoteID = calTagVM.QuoteID;
            Customer CusAddress = _Context.Customer.GetSingle(e => e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            if (CusAddress != null)
            {
                calTagFileVM.CustName = CusAddress.CustomerName;
                calTagFileVM.DivCust = CusAddress.DivisionNo.ToString() + '-' + CusAddress.CustomerNO;
            }
            return calTagFileVM;
        }
        public void GenerateCalTaagCsvFiles(CallTagFileViewModel calTagFilVM)
        {
            string filePathWithName = ConfigurationManager.AppSettings["FullStickPath"] + GetFilename(UserVM.CRMModelProperties.DivNO.ToString(), UserVM.CRMModelProperties.CustNO, calTagFilVM.QuoteID.ToString());
            using (StreamWriter writer = new StreamWriter(filePathWithName, false))
            {
                writer.WriteLine(GenerateText(new CallTagFileViewModel()));
                writer.WriteLine(PrepareFileValuesforcsv(calTagFilVM));
            }
        }
        //removable method
        public string GenerateCTFlatFiles(CalTagViewModel calTagVM)
        {
            string filePathWithName = ConfigurationManager.AppSettings["CalTagInvoicePath"] + "TESTupsctb_" + calTagVM.QuoteID + ".txt";
            string FixedCharacters = ConfigurationManager.AppSettings["CalTagInvoiceLengthString"];
            string[] aryFixedCharacters = FixedCharacters.Split(',');
            using (StreamWriter writer = new StreamWriter(filePathWithName, false))
            {
                string[] strHeaderText = GenerateText(new CalTagViewModel()).Split('|');
                string[] strRow = PrepareFileValuesforcsv(calTagVM).Split('|');

                for (int i = 0; i < strHeaderText.Length; i++)
                {
                    writer.WriteLine(strHeaderText[i] + "  " + (strRow[i].Length > Convert.ToInt32(aryFixedCharacters[i]) ? strRow[i].Remove(Convert.ToInt32(aryFixedCharacters[i])) : strRow[i].PadRight(Convert.ToInt32(aryFixedCharacters[i]))));
                }

            }
            return filePathWithName;
        }

        public string GenerateCTFlatFiles(List<CalTagViewModel> lstCalTagViewModel)
        {

            if (lstCalTagViewModel.Count > 0)
            {
                string filePathWithName = ConfigurationManager.AppSettings["CalTagInvoicePath"] + "TESTupsctb_" + lstCalTagViewModel.FirstOrDefault().QuoteID + ".txt";
                string FixedCharacters = ConfigurationManager.AppSettings["CalTagInvoiceLengthString"];
                string[] aryFixedCharacters = FixedCharacters.Split(',');
                using (StreamWriter writer = new StreamWriter(filePathWithName, false))
                {
                    foreach (CalTagViewModel calTagVM in lstCalTagViewModel)
                    {
                        string[] strRow = PrepareFileValuesforcsv(calTagVM).Split('|');

                        for (int i = 0; i < strRow.Length; i++)
                        {
                            writer.Write((strRow[i].Length > Convert.ToInt32(aryFixedCharacters[i]) ? strRow[i].Remove(Convert.ToInt32(aryFixedCharacters[i])) : strRow[i].PadRight(Convert.ToInt32(aryFixedCharacters[i]))));
                        }
                        writer.WriteLine();
                    }

                }
                return filePathWithName;
            }
            else
            {
                return string.Empty;
            }

        }
    }
}
