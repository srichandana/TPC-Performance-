using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Interfaces;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using TPC.Context;
using TPC.Context.EntityModel;
using TPC.Context.Interfaces;
using System.Linq.Expressions;
using TPC.Core.Infrastructure;
using TPC.Core.Models;
using TPC.Common.Enumerations;

namespace TPC.Core
{
    public class CatalogInfoService : ServiceBase<ICatalogInfoModel>, ICatalogInfoService
    {
        public CatalogInfoViewModel getCatalogInfoValues(int custAutoID)
        {
            CatalogInfoViewModel catalogInfoVM = new CatalogInfoViewModel();
            QuoteViewService quoteService = new QuoteViewService();
            catalogInfoVM.ThisUserID = custAutoID;
            catalogInfoVM.CatalogOptions = FillCatalogOptionsInfo(catalogInfoVM.ThisUserID);
            catalogInfoVM.ValidationCatalogBasicProfileModel = FillValidationsInfo();
            CustomerCatalogInformation ccInfo = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoID).OrderByDescending(e => e.UpdatedDate).FirstOrDefault();
            catalogInfoVM.UpdatedDate = ccInfo != null ? ccInfo.UpdatedDate : (DateTime?)null;
            catalogInfoVM.UpdatedUserName = ccInfo != null ? ccInfo.User != null ? ccInfo.User.UserName.ToString() : string.Empty : string.Empty;
            quoteService.UserVM = UserVM;
            UserVM.CurrentQuoteID = quoteService.getCustomerSCQuoteID();
            catalogInfoVM.UserVM = UserVM;
            return catalogInfoVM;
        }

        private Dictionary<string, Dictionary<string, Dictionary<CatalogBaseModel, List<CatalogBaseModel>>>> FillCatalogOptionsInfo(int custAutoID)
        {
            Dictionary<string, Dictionary<string, Dictionary<CatalogBaseModel, List<CatalogBaseModel>>>> catalogInfo = new Dictionary<string, Dictionary<string, Dictionary<CatalogBaseModel, List<CatalogBaseModel>>>>(); ;
            Dictionary<CatalogBaseModel, List<CatalogBaseModel>> catalogSubOptionsModel = null;
            List<CustomerCatalogInformation> lstCustCatInfo = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoID).ToList();
            List<CatalogInformation> lstCataloginfoObj = _Context.CatalogInformation.GetAll().OrderBy(e => e.Priority).ToList();

            foreach (CatalogInformation CataloginfoObj in lstCataloginfoObj)
            {
                Dictionary<string, Dictionary<CatalogBaseModel, List<CatalogBaseModel>>> catalogOptionsModel = new Dictionary<string, Dictionary<CatalogBaseModel, List<CatalogBaseModel>>>();

                foreach (CatalogSubject catSubj in CataloginfoObj.CatalogSubjects.Where(e => e.Priority != 0).OrderBy(e => e.Priority))
                {
                    catalogSubOptionsModel = new Dictionary<CatalogBaseModel, List<CatalogBaseModel>>();
                    foreach (CatalogSubjectOption catSubjOpt in catSubj.CatalogSubjectOptions.Where(e => e.Priority != 0).OrderBy(e => e.Priority).ToList())
                    {
                        int catalogItemSubjectOptionProtectorId = 0, catalogItemSubjectOptionShelfReadyId = 0;
                        int protectorsCheckedCount = 0;
                        CatalogBaseModel catSubOptBModel = new CatalogBaseModel();
                        List<CatalogBaseModel> lstCatSubOptValBModel = new List<CatalogBaseModel>();
                        catSubOptBModel.ID = catSubjOpt.CatalogSubjectOptionID;
                        catSubOptBModel.Text = catSubjOpt.CatalogSubjectOption1;
                        catSubOptBModel.Type = (int)catSubjOpt.ColumnType;
                        catSubOptBModel.IsSelected = catSubjOpt.CatalogSubjectID == 16 ?
                            catSubOptBModel.Type == (int)CatalogColumnTypeEnum.CheckBox ?
                            lstCustCatInfo.Where(e => e.CatalogSubjectOptionValueID == catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault().CatalogSubjectOptionValueID).FirstOrDefault() != null &&
                        lstCustCatInfo.Where(e => e.CatalogSubjectOptionValueID == catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault().CatalogSubjectOptionValueID).FirstOrDefault().Status != false ? true : false
                            : false : false;

                        CatalogBaseModel catSubOptValBModel = new CatalogBaseModel();
                        catSubOptValBModel = FillCatalogSubjectOptionValueBaseModel(catSubOptValBModel, catSubjOpt, lstCustCatInfo, custAutoID);
                        lstCatSubOptValBModel.Add(catSubOptValBModel);

                        if (catSubjOpt.CatalogSubjectOptionProtectors != null && catSubjOpt.CatalogSubjectOptionProtectors.Count() > 0)
                        {
                            CatalogBaseModel catSubOptProtectorValBModel = new CatalogBaseModel();
                            catSubOptProtectorValBModel = FillCatalogProtectorInformation(catSubOptProtectorValBModel, catSubjOpt, lstCustCatInfo, custAutoID);
                            if (catSubOptProtectorValBModel.IsSelected)
                            {
                                catalogItemSubjectOptionProtectorId = catSubOptProtectorValBModel.ID;
                                protectorsCheckedCount = Convert.ToInt32(catSubOptProtectorValBModel.Text);
                            }
                            lstCatSubOptValBModel.Add(catSubOptProtectorValBModel);
                        }
                        if (catSubjOpt.CatalogSubjectoptionShelfReadies != null && catSubjOpt.CatalogSubjectoptionShelfReadies.Count() > 0)
                        {
                            CatalogBaseModel catSubOptShelfValBModel = new CatalogBaseModel();
                            catSubOptShelfValBModel = FillCatalogShelfReadyInformation(catSubOptShelfValBModel, catSubjOpt, lstCustCatInfo, custAutoID);
                            if (catSubOptShelfValBModel.IsSelected)
                            {
                                catalogItemSubjectOptionShelfReadyId = catSubOptShelfValBModel.ID;
                            }
                            lstCatSubOptValBModel.Add(catSubOptShelfValBModel);
                        }

                        catSubOptBModel.Value = catSubj.CatalogSubjectID == 16 ? catSubOptBModel.IsSelected ? calculatePriceForCatalog(catSubOptBModel.ID, catalogItemSubjectOptionProtectorId, catalogItemSubjectOptionShelfReadyId, protectorsCheckedCount) : string.Format("{0:C}", 0) : string.Empty;
                        catalogSubOptionsModel.Add(catSubOptBModel, lstCatSubOptValBModel);
                    }
                    catalogOptionsModel.Add(catSubj.CatalogSubject1, catalogSubOptionsModel);
                }
                catalogInfo.Add(CataloginfoObj.CatalogInfoName, catalogOptionsModel);
            }
            return catalogInfo;
        }

        private Dictionary<string, string> FillValidationsInfo()
        {
            List<CatalogProfileValidation> lstcatalogSubOptions = _Context.CatalogProfileValidation.GetAll().ToList();
            int tempIndex = 0;
            return lstcatalogSubOptions.ToDictionary(cso => cso.CatalogSubjectOption.CatalogSubjectOptionID + "-" + tempIndex++, cso => cso.CatalogSubjectOption1.CatalogSubjectOptionID.ToString());
        }

        private CatalogBaseModel FillCatalogSubjectOptionValueBaseModel(CatalogBaseModel catSubOptValBModel, CatalogSubjectOption catSubjOpt, List<CustomerCatalogInformation> lstCustCatInfo, int custUserID)
        {
            if (catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.ComboBox || catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.CheckBox)
            {
                catSubOptValBModel.comboBase = new List<ComboBase>();
                if (catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.ComboBox)
                {
                    catSubOptValBModel.comboBase.Insert(0, new ComboBase { ItemValue = "--Select--", ItemID = "0" });
                }
            }
            string strComments = string.Empty;

            List<CatalogSubjectOptionValue> lstCatalogSubOptValues = catSubjOpt.CatalogSubjectOptionValues.OrderBy(e => e.Priority).ToList();
            if (catSubjOpt.CatalogSubjectOptionValues.Where(e => e.CatalogSubjectOptionID == 33).FirstOrDefault() != null)
            {
                lstCatalogSubOptValues = FillVersionValues(lstCustCatInfo, catSubjOpt);
            }
            if (lstCatalogSubOptValues != null && lstCatalogSubOptValues.Count() > 0)
            {
                foreach (CatalogSubjectOptionValue catSubjOptVal in lstCatalogSubOptValues)
                {
                    catSubOptValBModel.ID = catSubjOpt.CatalogSubjectOptionID;
                    catSubOptValBModel.Type = (int)catSubjOpt.ColumnType;
                    if (catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.ComboBox || catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.CheckBox)
                    {
                        ComboBase objCombo = new ComboBase();
                        objCombo.ItemID = catSubjOptVal.CatalogSubjectOptionValueID.ToString();
                        objCombo.ItemValue = catSubjOptVal.CatalogSubjectOptionValue1;
                        objCombo.Selected = lstCustCatInfo.Where(e => e.CatalogSubjectOptionValueID == catSubjOptVal.CatalogSubjectOptionValueID && e.Status != false).FirstOrDefault() != null ? true : false;
                        catSubOptValBModel.comboBase.Add(objCombo);
                    }
                    strComments = lstCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionValueID == catSubjOptVal.CatalogSubjectOptionValueID && e.Status != false).FirstOrDefault() != null ? lstCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionValueID == catSubjOptVal.CatalogSubjectOptionValueID && e.Status != false).FirstOrDefault().Comments : strComments;
                    catSubOptValBModel.Value = strComments;
                    if (catSubjOpt.CatalogSubjectID == 16)
                    {
                        catSubOptValBModel.Type = catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.CheckBox ? (int)CatalogColumnTypeEnum.Label : (int)catSubjOpt.ColumnType;
                        if (catSubjOpt.ColumnType == (int)CatalogColumnTypeEnum.CheckBox)
                        {
                            catSubOptValBModel.Value = catSubjOptVal.CatalogSubjectOptionValue1;
                        }
                    }
                }
            }
            else
            {
                catSubOptValBModel.ID = catSubjOpt.CatalogSubjectOptionID;
                catSubOptValBModel.Type = (int)catSubjOpt.ColumnType;
            }
            return catSubOptValBModel;
        }

        private CatalogBaseModel FillCatalogProtectorInformation(CatalogBaseModel catSubOptProtectorValBModel, CatalogSubjectOption catSubjOpt, List<CustomerCatalogInformation> lstCustCatInfo, int custAutoID)
        {
            catSubOptProtectorValBModel.ID = catSubjOpt.CatalogSubjectOptionProtectors.FirstOrDefault().CSOProtectorID;
            catSubOptProtectorValBModel.Text = catSubjOpt.CatalogSubjectOptionProtectors.FirstOrDefault().ProtectorCount.ToString();
            catSubOptProtectorValBModel.CatalogType = "P";
            catSubOptProtectorValBModel.Type = (int)catSubjOpt.ColumnType;

            catSubOptProtectorValBModel.IsSelected = catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault() != null ? catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault().CustomerCatalogInformations.Where(e => e.CustAutoID == custAutoID).Count() > 0 ? catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault().CustomerCatalogInformations.Where(e => e.CustAutoID == custAutoID).FirstOrDefault().CustomerCatalogProtectorInformations.Where(e => e.Status == true).Count() > 0 ? true : false : false : false;
            catSubOptProtectorValBModel.Value = catSubjOpt.CatalogSubjectOptionProtectors.FirstOrDefault().CatalogSubjectOptionProtectorValues.FirstOrDefault().CSOProtectorValue;
            return catSubOptProtectorValBModel;
        }

        private CatalogBaseModel FillCatalogShelfReadyInformation(CatalogBaseModel catSubOptShelfValBModel, CatalogSubjectOption catSubjOpt, List<CustomerCatalogInformation> lstCustCatInfo, int custAutoID)
        {
            catSubOptShelfValBModel.ID = catSubjOpt.CatalogSubjectoptionShelfReadies.FirstOrDefault().CSOShelfReadyID;
            catSubOptShelfValBModel.Text = "1";
            catSubOptShelfValBModel.CatalogType = "S";
            catSubOptShelfValBModel.IsSelected = catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault() != null ? catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault().CustomerCatalogInformations.Where(e => e.CustAutoID == custAutoID).Count() > 0 ? catSubjOpt.CatalogSubjectOptionValues.FirstOrDefault().CustomerCatalogInformations.Where(e => e.CustAutoID == custAutoID).FirstOrDefault().CustomerCatalogShelfReadyInformations.Where(e => e.Status == true).Count() > 0 ? true : false : false : false;
            catSubOptShelfValBModel.Type = (int)catSubjOpt.ColumnType;
            catSubOptShelfValBModel.Value = (catSubjOpt.CatalogSubjectoptionShelfReadies.FirstOrDefault().ShelfReadyCount * Convert.ToDouble(catSubjOpt.CatalogSubjectoptionShelfReadies.FirstOrDefault().CatalogSubjectOptionShelfReadyValues.FirstOrDefault().CSOShelfReadyValue)).ToString();
            return catSubOptShelfValBModel;
        }

        public CatalogSubjectItemIDMapping GetCatalogSubjectMappingBySelectionofCatalogItem(int catalogSubjectOptID, bool isCatalogLabelChecked, bool isCatalogProChecked, bool isCatalogExtProChecked, bool isCatalogSRChecked)
        {
            CatalogSubjectItemIDMapping catSubjItemIDMapping = _Context.CatalogSubjectItemIDMapping.GetSingle(e => e.CatalogSubjectOptionID == catalogSubjectOptID &&
                    e.IsCLabelChecked == isCatalogLabelChecked && e.ISCSubjProtectorChecked == isCatalogProChecked && e.IsCExtraProtectorChecked == isCatalogExtProChecked
                    && e.IsCSubjSRChecked == isCatalogSRChecked);
            if (catSubjItemIDMapping != null)
            {
                return catSubjItemIDMapping;
            }
            return null;
        }

        public string calculatePriceForCatalog(int catalogItemSubjectOptionId, int catalogItemSubjectOptionProtectorId, int catalogItemSubjectOptionShelfReadyId, int protectorsCheckedCount)
        {
            double catalogPrice = 0;
            bool isLabelChecked= catalogItemSubjectOptionId!=0?true:false;
            bool isProtectorChecked = catalogItemSubjectOptionProtectorId != 0 ? true : false;
            bool isExtraProtectorChecked = protectorsCheckedCount > 1 ? true : false;
            bool isSRChecked = catalogItemSubjectOptionShelfReadyId !=0 ? true : false;
            CatalogSubjectItemIDMapping catSubjItemMapping = GetCatalogSubjectMappingBySelectionofCatalogItem(catalogItemSubjectOptionId, isLabelChecked, isProtectorChecked, isExtraProtectorChecked, isSRChecked);
            if (catSubjItemMapping != null)
            {
                catalogPrice = Convert.ToDouble(catSubjItemMapping.Item.Price);
            }
            return string.Format("{0:C}", catalogPrice);
        }

        private List<CatalogSubjectOptionValue> FillVersionValues(List<CustomerCatalogInformation> lstCustCatInfo, CatalogSubjectOption catSubjOpt)
        {
            bool isSoftwareSelected = false;
            int softwarevalID = 0;
            List<CatalogSubjectOptionValue> lstCatalogSuboptVersionVal = new List<CatalogSubjectOptionValue>();
            isSoftwareSelected = lstCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == 15 && e.Status != false).FirstOrDefault() != null ? true : false;
            if (isSoftwareSelected) softwarevalID = lstCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == 15 && e.Status != false).FirstOrDefault() != null ? (int)lstCustCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOptionID == 15 && e.Status != false).FirstOrDefault().CatalogSubjectOptionValueID : 0;
            if (softwarevalID != 0)
            {
                lstCatalogSuboptVersionVal = GetVersionListBySoftwareID(softwarevalID);
                return catSubjOpt.CatalogSubjectOptionValues.Where(e => lstCatalogSuboptVersionVal.Contains(e)).Select(e => e).OrderBy(e => e.CatalogSubjectOptionValue1).ToList();
            }
            return null;
        }

        public List<CatalogSubjectOptionValue> GetVersionListBySoftwareID(int softwareValueID)
        {
            return _Context.CatalogSoftwareVersionMapping.GetAll(e => e.CatSubOptSoftwareValID == softwareValueID).Select(e => e.CatalogSubjectOptionValue1).OrderBy(e => e.CatalogSubjectOptionValue1).ToList();
        }

        public List<ComboBase> GetVersionListStringBySoftwareID(int softwareValueID)
        {
            return _Context.CatalogSoftwareVersionMapping.GetAll(e => e.CatSubOptSoftwareValID == softwareValueID).Select(t =>
                new ComboBase { ItemID = t.CatSubOptVersionValID.ToString(), ItemValue = t.CatalogSubjectOptionValue1.CatalogSubjectOptionValue1 }).OrderBy(e => e.ItemValue).ToList();
        }

        public bool CheckARRCLevelExitsForTitlesByQuoteID(int quoteid)
        {
            List<QuoteDetail> lstQd = _Context.QuoteDetail.GetAll(e => e.QuoteID == quoteid).Where(e => e.Item.ARLevel != null || e.Item.RCLevel != null).ToList();
            if (lstQd.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public CreateCatalogInfoModel CreateCatalogInfo(Dictionary<string, string> dctSelectionValues, int custAutoID)
        {
            List<CreateCatalogInfoModel> lstcreatecatInfoModel = new List<CreateCatalogInfoModel>();
            List<CreateCatalogInfoModel> lstcreateProtectorInfoModel = new List<CreateCatalogInfoModel>();
            List<CreateCatalogInfoModel> lstcreateShelfReadyInfoModel = new List<CreateCatalogInfoModel>();
            foreach (string key in dctSelectionValues.Keys)
            {
                CreateCatalogInfoModel createcatInfoNewModel = null;
                if (!key.Contains("-"))
                {
                    createcatInfoNewModel = new CreateCatalogInfoModel();
                    createcatInfoNewModel.CustUserID = custAutoID;
                    int catalogSubjectOptID = Convert.ToInt32(key);
                    CatalogSubjectOptionValue catalogSubOptionvalue = _Context.CatalogSubjectOptionValue.GetSingle(e => e.CatalogSubjectOptionID == catalogSubjectOptID);
                    if (catalogSubOptionvalue.CatalogSubjectOption.CatalogSubjectID == 16)
                    {
                        createcatInfoNewModel.CatalogSubjectOptionValueID = catalogSubOptionvalue.CatalogSubjectOptionValueID;
                    }
                    else
                    {
                        createcatInfoNewModel.CatalogSubjectOptionValueID = catalogSubOptionvalue.CatalogSubjectOptionValueID;
                        if (catalogSubOptionvalue.CatalogSubjectOption.ColumnType == (int)CatalogColumnTypeEnum.ComboBox)
                        {
                            createcatInfoNewModel.CatalogSubjectOptionValueID = Convert.ToInt32(dctSelectionValues[key]);
                        }
                    }
                    if (catalogSubOptionvalue.CatalogSubjectOption.ColumnType == (int)CatalogColumnTypeEnum.TextBox
                        || catalogSubOptionvalue.CatalogSubjectOption.ColumnType == (int)CatalogColumnTypeEnum.TextArea)
                    {
                        if (dctSelectionValues[key] != string.Empty && dctSelectionValues[key] != "true")
                        {
                            createcatInfoNewModel.Comments = dctSelectionValues[key];
                        }
                    }
                }
                else
                {
                    createcatInfoNewModel = new CreateCatalogInfoModel();
                    createcatInfoNewModel.CustUserID = custAutoID;
                    if (key.Contains("P"))
                    {
                        //for protector
                        int catalogProtectorID = Convert.ToInt32(key.Split('-')[1]);
                        CatalogSubjectOptionProtectorValue catSubjOptProtectorValueInfo = _Context.CatalogSubjectOptionProtectorValue.GetSingle(e => e.CSOProtectorID == catalogProtectorID);
                        createcatInfoNewModel.CatalogSubjectOptionValueID = catSubjOptProtectorValueInfo.CatalogSubjectOptionValue.CatalogSubjectOptionValueID;
                        createcatInfoNewModel.CCProtectorValueID = catSubjOptProtectorValueInfo.CSOProtectorvalueID;
                        lstcreateProtectorInfoModel.Add(createcatInfoNewModel);
                    }
                    else if (key.Contains("S"))
                    {
                        //for Shelf Ready
                        int catalogShelfReadyID = Convert.ToInt32(key.Split('-')[1]);
                        CatalogSubjectOptionShelfReadyValue catSubjOptShelfReadyValueInfo = _Context.CatalogSubjectOptionShelfReadyValue.GetSingle(e => e.CSOShelfReadyID == catalogShelfReadyID);
                        createcatInfoNewModel.CatalogSubjectOptionValueID = catSubjOptShelfReadyValueInfo.CatalogSubjectOptionValue.CatalogSubjectOptionValueID;
                        createcatInfoNewModel.CCShelfReadyValueID = catSubjOptShelfReadyValueInfo.CSOShelfReadyvalueID;
                        lstcreateShelfReadyInfoModel.Add(createcatInfoNewModel);
                    }
                    else if (key.Contains("-B"))
                    {
                        //for Bottom Label Placement
                        int catalogSubjectOptID = Convert.ToInt32(key.Split('-')[0]);
                        CatalogSubjectOptionValue catalogSubOptionvalue = _Context.CatalogSubjectOptionValue.GetSingle(e => e.CatalogSubjectOptionID == catalogSubjectOptID && e.CatalogSubjectOptionValue1 == "Other");
                        lstcreatecatInfoModel.Where(e => e.CatalogSubjectOptionValueID == catalogSubOptionvalue.CatalogSubjectOptionValueID).FirstOrDefault().Comments = dctSelectionValues[key];
                    }
                    else
                    {
                        //for checkboxes storing multple values.
                        int catalogSubjectOptionValueID = Convert.ToInt32(key.Split('-')[1]);
                        createcatInfoNewModel.CatalogSubjectOptionValueID = catalogSubjectOptionValueID;
                        lstcreatecatInfoModel.Add(createcatInfoNewModel);
                    }
                }
                if (createcatInfoNewModel != null)
                {
                    if (!key.Contains("-"))
                    {
                        lstcreatecatInfoModel.Add(createcatInfoNewModel);
                    }
                }
            }
            CheckingandInsertorUpdateCatalogData(lstcreatecatInfoModel, lstcreateProtectorInfoModel, lstcreateShelfReadyInfoModel, custAutoID);
            return null;
        }

        private bool CheckingandInsertorUpdateCatalogData(List<CreateCatalogInfoModel> lstcreatecatInfoModel, List<CreateCatalogInfoModel> lstCatalogProtectorInfoModel, List<CreateCatalogInfoModel> lstCatalogShelfReadyInfoModel, int custAutoID, string flag = "")
        {
            List<CustomerCatalogInformation> lstCustomerCatalogInfo = null;
            lstCustomerCatalogInfo = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoID).ToList();
            if (!UserVM.CRMModelProperties.IsRepLoggedIN)
            {
                lstCustomerCatalogInfo = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoID && e.CatalogSubjectOptionValue.CatalogSubjectOption.CatalogSubjectID == 16).ToList();
            }
            List<CustomerCatalogProtectorInformation> lstCCProtectorInfo = lstCustomerCatalogInfo.Select(e => e.CustomerCatalogProtectorInformations.FirstOrDefault()).ToList();
            lstCCProtectorInfo.RemoveAll(e => e == null);
            List<CustomerCatalogShelfReadyInformation> lstCCShelfReadyInfo = lstCustomerCatalogInfo.Select(e => e.CustomerCatalogShelfReadyInformations.FirstOrDefault()).ToList();
            lstCCShelfReadyInfo.RemoveAll(e => e == null);

            UpdateStatusandInsertforCustomerCatalogInformation(lstCustomerCatalogInfo, lstcreatecatInfoModel, custAutoID);
            UpdateStatusAndInsertForCustomerCatalogProtectorInformation(lstCCProtectorInfo, lstCatalogProtectorInfoModel, custAutoID);
            UpdateStatusAndInsertforCustomerCatalogShelfReadyInformation(lstCCShelfReadyInfo, lstCatalogShelfReadyInfoModel, custAutoID);
            return true;
        }

        #region Update Status for Customer Catalog Information,Customer Catalog Protector Information,Customer Catalog Shelf Ready Information

        public void UpdateStatusandInsertforCustomerCatalogInformation(List<CustomerCatalogInformation> lstCustomerCatalogInfo, List<CreateCatalogInfoModel> lstcreatecatInfoModel, int custAutoID)
        {
            CustomerCatalogInformation createCustomerCatalogInfo = null;
            List<int> lstCSOVIDs = lstcreatecatInfoModel.Select(e => e.CatalogSubjectOptionValueID).ToList();
            //updating the status to Inactive for already existing record if it is not existing- in Customer Catalog Information
            if (lstCustomerCatalogInfo != null && lstCustomerCatalogInfo.Count() > 0)
            {
                List<CustomerCatalogInformation> lstNotMatchedCustomerCatInfo = lstCustomerCatalogInfo.Where(e => !lstCSOVIDs.Contains((int)e.CatalogSubjectOptionValueID)).Select(e => e).ToList();
                foreach (CustomerCatalogInformation CCInfo in lstNotMatchedCustomerCatInfo)
                {
                    CCInfo.Status = false;
                    _Context.CustomerCatalogInformation.SaveChanges();
                }
            }
            //Insertion
            foreach (CreateCatalogInfoModel ccInfoModel in lstcreatecatInfoModel)
            {
                if (lstCustomerCatalogInfo.Where(e => e.CatalogSubjectOptionValueID == ccInfoModel.CatalogSubjectOptionValueID).FirstOrDefault() == null)
                {
                    createCustomerCatalogInfo = new CustomerCatalogInformation();
                    createCustomerCatalogInfo.CatalogSubjectOptionValueID = ccInfoModel.CatalogSubjectOptionValueID;
                    createCustomerCatalogInfo.CustAutoID = custAutoID;
                    createCustomerCatalogInfo.Comments = ccInfoModel.Comments;
                    createCustomerCatalogInfo.CreatedDate = DateTime.UtcNow;
                    createCustomerCatalogInfo.UpdatedDate = DateTime.UtcNow;
                    createCustomerCatalogInfo.Status = true;
                    createCustomerCatalogInfo.LoggedInUserID = UserVM.CRMModelProperties.LoggedINUserID;
                    _Context.CustomerCatalogInformation.Add(createCustomerCatalogInfo);
                }
                else
                {
                    CustomerCatalogInformation updateCustomerCatalogInfo = lstCustomerCatalogInfo.Where(e => e.CatalogSubjectOptionValueID == ccInfoModel.CatalogSubjectOptionValueID).FirstOrDefault();
                    updateCustomerCatalogInfo.CustomerCatID = updateCustomerCatalogInfo.CustomerCatID;
                    updateCustomerCatalogInfo.Comments = ccInfoModel.Comments;
                    updateCustomerCatalogInfo.Status = true;
                    updateCustomerCatalogInfo.UpdatedDate = DateTime.UtcNow;
                    updateCustomerCatalogInfo.LoggedInUserID = UserVM.CRMModelProperties.LoggedINUserID;
                }
            }
            _Context.CustomerCatalogInformation.SaveChanges();
        }

        private void UpdateStatusAndInsertForCustomerCatalogProtectorInformation(List<CustomerCatalogProtectorInformation> lstCCProtectorInfo, List<CreateCatalogInfoModel> lstCatalogProtectorInfoModel, int custAutoID)
        {
            CustomerCatalogProtectorInformation createCustomerCatalogProtectorInfo = null;
            List<int> lstProtectorIDs = lstCatalogProtectorInfoModel.Select(e => e.CCProtectorValueID).ToList();

            //updating the status to Inactive for already existing record if it is not existing from customer Catalog protector Information
            if (lstCCProtectorInfo.Count() > 0)
            {
                List<CustomerCatalogProtectorInformation> lstNotMatchedProtecorInfo = lstCCProtectorInfo.Where(e => !lstProtectorIDs.Contains(e.CCProtectorValueID)).Select(e => e).ToList();
                foreach (CustomerCatalogProtectorInformation ccProtectorInfo in lstNotMatchedProtecorInfo)
                {
                    ccProtectorInfo.Status = false;
                    _Context.CustomerCatalogProtectorInformation.SaveChanges();
                }
            }

            foreach (CreateCatalogInfoModel ccprotectOrInfoModel in lstCatalogProtectorInfoModel)
            {
                if (lstCCProtectorInfo.Where(e => e.CCProtectorValueID == ccprotectOrInfoModel.CCProtectorValueID).FirstOrDefault() == null)
                {
                    createCustomerCatalogProtectorInfo = new CustomerCatalogProtectorInformation();
                    int customerCatalogID = _Context.CustomerCatalogInformation.GetSingle(e => e.CatalogSubjectOptionValueID == ccprotectOrInfoModel.CatalogSubjectOptionValueID && e.CustAutoID == custAutoID).CustomerCatID;
                    createCustomerCatalogProtectorInfo.CCatID = customerCatalogID;
                    createCustomerCatalogProtectorInfo.CCProtectorValueID = ccprotectOrInfoModel.CCProtectorValueID;
                    createCustomerCatalogProtectorInfo.Status = true;
                    _Context.CustomerCatalogProtectorInformation.Add(createCustomerCatalogProtectorInfo);
                }
                else
                {
                    CustomerCatalogProtectorInformation updateCustomerCatalogProtectorInfo = lstCCProtectorInfo.Where(e => e.CCProtectorValueID == ccprotectOrInfoModel.CCProtectorValueID).FirstOrDefault();
                    updateCustomerCatalogProtectorInfo.Status = true;
                }

            }
            _Context.CustomerCatalogProtectorInformation.SaveChanges();
        }

        private void UpdateStatusAndInsertforCustomerCatalogShelfReadyInformation(List<CustomerCatalogShelfReadyInformation> lstCCShelfReadyInfo, List<CreateCatalogInfoModel> lstCatalogShelfReadyInfoModel, int custAutoID)
        {
            CustomerCatalogShelfReadyInformation createCustomerCatalogShelfInfo = null;
            List<int> lsetShelfReadyIDS = lstCatalogShelfReadyInfoModel.Select(e => e.CCShelfReadyValueID).ToList();
            //updating the status to Inactive for already existing record if it is not existing- From Customer Shelf Ready Information
            if (lstCCShelfReadyInfo.Count() > 0)
            {
                List<CustomerCatalogShelfReadyInformation> lstNotMatchedShelfInfo = lstCCShelfReadyInfo.Where(e => !lsetShelfReadyIDS.Contains(e.CCShelfReadyValueID)).Select(e => e).ToList();
                foreach (CustomerCatalogShelfReadyInformation ccShelfInfo in lstNotMatchedShelfInfo)
                {
                    ccShelfInfo.Status = false;
                    _Context.CustomerCatalogShelfReadyInformation.SaveChanges();
                }
            }

            foreach (CreateCatalogInfoModel ccShelfReadyInfoModel in lstCatalogShelfReadyInfoModel)
            {
                if (lstCCShelfReadyInfo.Where(e => e.CCShelfReadyValueID == ccShelfReadyInfoModel.CCShelfReadyValueID).FirstOrDefault() == null)
                {
                    createCustomerCatalogShelfInfo = new CustomerCatalogShelfReadyInformation();
                    int customerCatalogID = _Context.CustomerCatalogInformation.GetSingle(e => e.CatalogSubjectOptionValueID == ccShelfReadyInfoModel.CatalogSubjectOptionValueID && e.CustAutoID == custAutoID).CustomerCatID;
                    createCustomerCatalogShelfInfo.CCatID = customerCatalogID;
                    createCustomerCatalogShelfInfo.CCShelfReadyValueID = ccShelfReadyInfoModel.CCShelfReadyValueID;
                    createCustomerCatalogShelfInfo.Status = true;
                    _Context.CustomerCatalogShelfReadyInformation.Add(createCustomerCatalogShelfInfo);
                }
                else
                {
                    CustomerCatalogShelfReadyInformation updateCustomerCatalogShelfInfo = lstCCShelfReadyInfo.Where(e => e.CCShelfReadyValueID == ccShelfReadyInfoModel.CCShelfReadyValueID).FirstOrDefault();
                    updateCustomerCatalogShelfInfo.Status = true;
                }
            }
            _Context.CustomerCatalogShelfReadyInformation.SaveChanges();
        }
        #endregion


        public int GetSubjectOptionvalueIDbySubjOptionID(int catSubjOptionID)
        {
            return _Context.CatalogSubjectOptionValue.GetSingle(e => e.CatalogSubjectOptionID == catSubjOptionID).CatalogSubjectOptionValueID;
        }
    }
}
