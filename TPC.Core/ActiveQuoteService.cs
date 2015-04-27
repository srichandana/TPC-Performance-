using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using TPC.Common.Constants;
using TPC.Common.Enumerations;
using TPC.Context.EntityModel;
using TPC.Core.Infrastructure;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Core.Models.ViewModels;

namespace TPC.Core
{
    public class ActiveQuoteService : ServiceBase<IActiveQuoteModel>, IActiveQuoteService
    {

        public ActiveQuoteViewModel GetActiveQuoteVM()
        {
            UserVM.CurrentQuoteID = 0;
            ActiveQuoteViewModel acVM = new ActiveQuoteViewModel();

            acVM.ListActiveQuote = new List<QuoteModel>();
            acVM.ListQuoteHistory = new List<QuoteModel>();

            List<Quote> lstQuotes = _Context.Quote.GetAll(e => e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID &&
                e.StatusID != (int)QuoteStatusEnum.InActive && e.StatusID != (int)QuoteStatusEnum.Sent && e.StatusID != (int)QuoteStatusEnum.Invoiced &&
                (e.QuoteTypeID != (int)QuoteTypeEnum.DecisionWhizard && e.QuoteTypeID != (int)QuoteTypeEnum.Literature) && e.QuoteTypeID != (int)QuoteTypeEnum.ShoppingCart).ToList();

            //get active quotes
            acVM.ListActiveQuote = AutoMapper.Mapper.Map<IList<Quote>, IList<QuoteModel>>(lstQuotes).ToList();

            if (acVM.ListActiveQuote.Count == 0)
            {
                acVM.ListActiveQuote = new List<QuoteModel>();
            }
            else
            {
                acVM.ListActiveQuote[0].QuoteMailDate = GetMostRecentInitiatedRepEmailDate(acVM.ListActiveQuote.Select(e => e.QuoteID).ToList(), "Quote");
            }

            //get quote history
            List<Quote> lstQuoteHistory = _Context.Quote.GetAll(e => e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID && (e.QuoteTypeID == (int)QuoteTypeEnum.Direct || e.QuoteTypeID == (int)QuoteTypeEnum.Preview || e.QuoteTypeID == (int)QuoteTypeEnum.Web) && e.StatusID == (int)QuoteStatusEnum.Invoiced).OrderByDescending(e => e.UpdateDate).ToList();
            List<InvoiceHistory> lstInvoiceHistory = _Context.InvoiceHistory.GetAll(e => e.CustAutoId == UserVM.CRMModelProperties.CustAutoID).ToList();

            foreach (InvoiceHistory invoiceHistory in lstInvoiceHistory)
            {
                QuoteModel quoteModel = new QuoteModel();
                quoteModel.QuoteID = Convert.ToInt32(invoiceHistory.QuoteID);
                Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteModel.QuoteID);

                quoteModel.QuoteTypeText = invoiceHistory != null ? invoiceHistory.QuoteType : string.Empty;
                quoteModel.TotalItems = quote != null ? (int)quote.QuoteDetails.Count : 0;
                quoteModel.TotalPrice = invoiceHistory != null ? (double)invoiceHistory.ItemsPrice : (double)0;
                quoteModel.StatusText = QuoteStatusEnum.Invoiced.ToString();
                quoteModel.QuoteName = invoiceHistory != null ? invoiceHistory.QuoteTitle : string.Empty;
                quoteModel.CreatedDate = invoiceHistory != null ? (DateTime)invoiceHistory.InvoiceDate : DateTime.UtcNow;
                quoteModel.UpdateDate = invoiceHistory != null ? (DateTime)invoiceHistory.OrderDate : DateTime.UtcNow;
                quoteModel.FileName = invoiceHistory != null ? _Context.InvoiceHistory.ExecSpWithOutputParamter("sp_GetInvoiceHisoryFileName @invoiceNo,@fileName",
                  new SqlParameter[]{
                        new SqlParameter            
                        {
                            ParameterName = "invoiceNo",
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Input,
                            Size = 20,
                            Value = invoiceHistory.InvoiceNo
                        },
                        new SqlParameter
                        {
                            ParameterName = "fileName",
                            SqlDbType = System.Data.SqlDbType.VarChar,
                            Direction = System.Data.ParameterDirection.Output,
                            Size = 50
                        }
                    }) : string.Empty;

                //if (System.IO.File.Exists(ConfigurationManager.AppSettings["InvoiceHistoryFilePath"] + filename + ".pdf"))
                //    quoteModel.FileName = filename;

                quoteModel.InvoiceNumber = invoiceHistory != null ? invoiceHistory.InvoiceNo : string.Empty;
                quoteModel.PurchaseOrderNumber = invoiceHistory != null ? invoiceHistory.CustomerPurchaseOrderNo : string.Empty;
                acVM.ListQuoteHistory.Add(quoteModel);
            }
            //  AutoMapper.Mapper.Map<IList<Quote>, IList<QuoteModel>>(lstQuoteHistory).ToList();
            if (acVM.ListQuoteHistory.Count == 0)
            {
                acVM.ListQuoteHistory = new List<QuoteModel>();
            }

            acVM.ListCustomerAccountSCDWQuoteInfo = GetCustomerSCDWInfo();
            //  acVM.LstCustomerEmails = GetCustomerEmails();// AutoMapper.Mapper.Map<IList<User>, IList<ComboBase>>(_context.User.GetAll(e => e.webpages_Roles.RoleId == (int)UserRolesEnum.CustomerShipTo && e.CustomerNO == UserVM.CustomerInfoObj.CustomerID).ToList()).ToList();
            acVM.UserVM = UserVM;
            return acVM;
        }

        private List<CustomerAccountSCDWModel> GetCustomerSCDWInfo()
        {
            string type = ConfigurationManager.AppSettings["DWinitialREPEmailType"];
            string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
            string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
            List<CustomerAccountSCDWModel> lstcustomerAccountSCDWM = new List<CustomerAccountSCDWModel>();
            List<User> lstCustomerAccountbyRepID = _Context.User.GetAll(e => e.CustAutoID == UserVM.CRMModelProperties.CustAutoID && e.UserId == UserVM.CRMModelProperties.LoggedINCustomerUserID).ToList();

            foreach (User customer in lstCustomerAccountbyRepID)
            {
                CustomerAccountSCDWModel customerAccountSCDWM = new CustomerAccountSCDWModel();
                int custUserID = Convert.ToInt32(customer.UserId);
                int custAutoId = (int)customer.CustAutoID;
                customerAccountSCDWM.CustomerAccountID = customer.UserId.ToString();
                customerAccountSCDWM.CustomerAccountName = customer.UserName;
                customerAccountSCDWM.UserEmail = customer.Email;
                customerAccountSCDWM.CommunicationEmail = customer.CommunicationEmail;
                List<Quote> lstQuotes = _Context.Quote.GetAll(e => e.UserID == custUserID && e.StatusID == (int)QuoteStatusEnum.Open).ToList();
                customerAccountSCDWM.ShopingCartInfo = lstQuotes.Where(e => e.UserID == custUserID && e.QuoteTypeID == (int)QuoteTypeEnum.ShoppingCart).Select(e => new CartWhizardInfoModel
               {

                   QuoteID = e.QuoteID,
                   NumberOfBooks = e.QuoteDetails.Where(y => (y.Item.Status == statusenumD || y.Item.Status == statusenumB) && y.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(x => x.Quantity),
                   TotalPrice = e.QuoteDetails.Where(m => (m.Item.Status == statusenumD || m.Item.Status == statusenumB) && m.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Sum(z => Convert.ToDouble(z.Item.Price * z.Quantity)),
                   CustomerUpdatedDate = Convert.ToDateTime(e.CustomerUpdatedDate),
                   PenworthyUpdatedDate = Convert.ToDateTime(e.PenworthyUpdatedDate),

               }).FirstOrDefault();
                customerAccountSCDWM.DesicionWhizardInfo = lstQuotes.Where(e => e.UserID == custUserID && e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == (int)QuoteStatusEnum.Open).Select(e => new CartWhizardInfoModel
                {
                    QuoteID = e.QuoteID,
                    NumberOfBooks = e.QuoteDetails.Count(),
                    TotalPrice = e.QuoteDetails.Sum(z => Convert.ToDouble(z.Item.Price * z.Quantity)),
                    CustomerUpdatedDate = Convert.ToDateTime(e.CustomerUpdatedDate),
                    PenworthyUpdatedDate = Convert.ToDateTime(e.PenworthyUpdatedDate),
                    NoofMaybCount = e.QuoteDetails.Where(f => f.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe).Count(),
                    NoofNoCount = e.QuoteDetails.Where(f => f.DWSelectionID == (int)DecisionWhizardStatusEnum.No).Count(),
                    NoofYesCount = e.QuoteDetails.Where(f => f.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes).Count(),
                    DWName = e.QuoteTitle,
                }).ToList();
                customerAccountSCDWM.CatalogStatus = CheckCatalogInfoforCustomer(custAutoId);

                customerAccountSCDWM.DwInitiatedMailDate = GetMostRecentInitiatedRepEmailDate(customerAccountSCDWM.DesicionWhizardInfo.Select(e => e.QuoteID).ToList(), type);
                lstcustomerAccountSCDWM.Add(customerAccountSCDWM);
            }

            return lstcustomerAccountSCDWM;

        }

        public DateTime? GetMostRecentInitiatedRepEmailDate(List<int> lstQuoteID, string type)
        {
            DateTime? dwMostRecentInitiatedDate = null;

            List<MailHistory> lstMailHistory = _Context.EmailHistory.GetAll(e => lstQuoteID.Contains((int)e.quoteId) && e.type == type).OrderByDescending(e => e.SendDate).ToList();
            if (lstMailHistory != null && lstMailHistory.Count() > 0)
            {
                dwMostRecentInitiatedDate = lstMailHistory.FirstOrDefault().SendDate;
            }
            return dwMostRecentInitiatedDate;
        }

        public string CheckCatalogInfoforCustomer(int custAutoId)
        {
            string catalogStatus = string.Empty;
            bool catalogInfoDataExists = CheckCataalogInfo(custAutoId);
            if (catalogInfoDataExists == false)
            {
                catalogStatus = "InComplete";
            }
            else
            {
                catalogStatus = "Complete";
            }
            return catalogStatus;
        }

        public bool CreateQuote(QuoteModel qoteModel)
        {
            Quote quoteEntity = new Quote
            {
                UserID = Convert.ToInt32(qoteModel.CustUserID),
                QuoteTitle = qoteModel.QuoteText,
                QuoteTypeID = qoteModel.QuoteTypeID,
                StatusID = qoteModel.StatusID,
                CreatedDate = qoteModel.CreatedDate,
                UpdateDate = qoteModel.UpdateDate,
                CustomerUpdatedDate = qoteModel.UpdateDate,
                PenworthyUpdatedDate = qoteModel.UpdateDate,
                QuoteType = _Context.QuoteType.GetSingle(e => e.QuoteTypeID == qoteModel.QuoteTypeID),
                Status = _Context.Status.GetSingle(e => e.StatusID == qoteModel.StatusID),
                IncludeCatalogStatus = false
            };
            _Context.Quote.Add(quoteEntity);
            _Context.Quote.SaveChanges();
            qoteModel.QuoteID = quoteEntity.QuoteID;
            return true;
        }

        public string GetQuoteTypeTextByQuoteTypeID(int quoteTpeID)
        {
            return _Context.QuoteType.GetSingle(e => e.QuoteTypeID == quoteTpeID).QuoteTypeText;
        }

        public string GetQuoteStatusbyStatusID(int statusID)
        {
            return _Context.Status.GetSingle(e => e.StatusID == statusID).StatusDescription;
        }

        public CreateQuoteViewModel GetQuoteTypes()
        {
            CreateQuoteViewModel acVM = new CreateQuoteViewModel();
            List<QuoteType> lstQuote = new List<QuoteType>();
            if (UserVM.CRMModelProperties.DivNO == 11)
            {
                lstQuote = _Context.QuoteType.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.Direct).OrderBy(e => e.QuoteTypeText).ToList();
            }
            else
            {
                lstQuote = _Context.QuoteType.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.Direct || e.QuoteTypeID == (int)QuoteTypeEnum.Preview).OrderBy(e => e.QuoteTypeText).ToList();
            }
            acVM.QuoteTypes = AutoMapper.Mapper.Map<IList<QuoteType>, IList<ComboBase>>(lstQuote).ToList();
            acVM.UserVM = UserVM;
            return acVM;
        }

        public CreateDecisionWizardViewModel GetDWInfo()
        {
            CreateDecisionWizardViewModel dwVM = new CreateDecisionWizardViewModel();
            //  dwVM.lstCustomers = GetCustomerEmails();
            dwVM.UserVM = UserVM;
            return dwVM;
        }

        public bool DeleteQuote(QuoteModel quoteModel)
        {
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteModel.QuoteID);
            quote.StatusID = 2;
            _Context.Quote.SaveChanges();
            return true;
        }

        public bool CheckCataalogInfo(int custAutoId)
        {
            List<CustomerCatalogInformation> custCatInfo = _Context.CustomerCatalogInformation.GetAll(e => e.CustAutoID == custAutoId && e.Status == true && (e.Comments == null || e.Comments != string.Empty)).ToList();
            bool exeecdsBlock = false;
            if (custCatInfo != null && custCatInfo.Count() != 0)
            {
                List<int> lstCatalogSelectedOptions = custCatInfo.Where(e => e.CatalogSubjectOptionValue.CatalogSubjectOption.CatalogSubjectID == 16 && e.Comments != string.Empty
                    && e.Comments == null && e.Status == true).ToList().Select(e => Convert.ToInt32(e.CatalogSubjectOptionValue.CatalogSubjectOptionID)).ToList();

                if (custCatInfo.Where(e =>
                    e.CatalogSubjectOptionValue.CatalogSubjectOptionID == 75 && e.Comments == "ERROR, EXCEEDS BLOCK"
                   && e.Status == true).FirstOrDefault() != null)
                {
                    exeecdsBlock = true;
                }
                if (lstCatalogSelectedOptions.Count > 0)
                {
                    List<CatalogProfileValidation> lstCustProfileValidation = _Context.CatalogProfileValidation.GetAll(e => lstCatalogSelectedOptions.Contains((int)e.CatalogSubjectOptionID)).Distinct().ToList();
                    List<int> lstProfileValidtionIDs = lstCustProfileValidation.Select(e => (int)e.CatalogSubjectOptionRequiredID).Distinct().ToList();
                    custCatInfo = custCatInfo.GroupBy(a => a.CatalogSubjectOptionValue.CatalogSubjectOptionID).Select(a => a.FirstOrDefault()).ToList();

                    custCatInfo.RemoveAll(e => !lstCustProfileValidation.Select(x => x.CatalogSubjectOptionRequiredID).Distinct().Contains(e.CatalogSubjectOptionValue.CatalogSubjectOptionID));
                    if (lstProfileValidtionIDs.Contains(69))
                    {
                        if (custCatInfo.Where(e => e.CatalogSubjectOptionValueID == 33).FirstOrDefault() != null || custCatInfo.Where(e => e.CatalogSubjectOptionValueID == 34).FirstOrDefault() != null)
                        {
                            lstProfileValidtionIDs.Remove(69);
                        }
                        //custCatInfo.Remove(lstCustProfileValidation.Where(e=>e.CatalogSubjectOption1 == 69));
                    }
                    if (custCatInfo.Count() >= lstProfileValidtionIDs.Count())
                    {
                        if (exeecdsBlock)
                        {
                            return false;
                        }

                        return true;
                    }
                }

            }
            return false;
        }

        private List<ComboBase> GetCustomerEmails()
        {
            //string[] strCustomerBilltoShipTo = UserVM.CRMModelProperties.CustID.Split('_');
            //string strBillTo = strCustomerBilltoShipTo.Count() > 0 ? strCustomerBilltoShipTo[0] : string.Empty;
            List<User> lstCustomers = _Context.User.GetAll(e => e.Email != string.Empty && e.Email != null && e.CustAutoID != null && e.CustAutoID == UserVM.CRMModelProperties.CustAutoID).ToList();
            return AutoMapper.Mapper.Map<IList<User>, IList<ComboBase>>(lstCustomers).ToList();
        }

        public QuoteModel InsertMergeQuoteDetails(List<int?> lstQuoteIds, int customerID, int currentmergeQuoteID)
        {
            List<QuoteDetail> lstQuoteDetails = _Context.QuoteDetail.GetAll(e => lstQuoteIds.Contains(e.QuoteID)).GroupBy(a => a.ItemID).Select(a => a.FirstOrDefault()).ToList();

            foreach (QuoteDetail qdItem in lstQuoteDetails)
            {
                QuoteDetail quoteDetail = new QuoteDetail();
                quoteDetail.QuoteID = currentmergeQuoteID;
                quoteDetail.ItemID = qdItem.ItemID;
                quoteDetail.DWSelectionID = qdItem.DWSelectionID;
                quoteDetail.Quantity = qdItem.Quantity;
                quoteDetail.CreatedDate = DateTime.Now;
                quoteDetail.UpdateDate = DateTime.Now;
                _Context.QuoteDetail.Add(quoteDetail);
                quoteDetail.Item = qdItem.Item;
            }
            _Context.QuoteDetail.SaveChanges();
            Quote mergeQuote = _Context.Quote.GetSingle(e => e.QuoteID == currentmergeQuoteID);

            return AutoMapper.Mapper.Map<Quote, QuoteModel>(mergeQuote);
        }

        public string UpdateRecentContactedByQuoteIDs(string quoteIDs, string selectedTemplate, string repComment)
        {
            int quoteID = Convert.ToInt32(quoteIDs);
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);

            EmailTemplateViewModel lstEmailTemplateVM = new EmailTemplateViewModel();
            User user = quote.User;
            List<Quote> lstDWQuotes = user.Quotes.Where(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == (int)QuoteStatusEnum.Open && e.QuoteDetails.Count > 0).ToList();

            List<string> lstItems = quote.QuoteDetails.Select(e => e.Item.ISBN).ToList();

            if (lstItems.Count > 0)
            {
                lstEmailTemplateVM.EmailDWTemplateList = lstDWQuotes.OrderByDescending(o => o.PenworthyUpdatedDate).Select(q => new EmailTemplateModel
                {
                    QuoteID = q.QuoteID,
                    PenworthyUpdatedDate = (DateTime)q.PenworthyUpdatedDate,
                    DWName = q.QuoteTitle,
                    NoOfDays = (int)(DateTime.UtcNow - (DateTime)q.CreatedDate).TotalDays,
                    LstISBN = GetImageISBNsForEmail(q),//q.QuoteDetails.OrderByDescending(qt => qt.UpdateDate).Select(qt => qt.Item).Select(qt => qt.ISBN).Distinct().Take(5).ToList(),
                    IsActive = q.QuoteDetails != null && q.QuoteDetails.Count > 0 ? q.QuoteDetails.Where(d => d.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes || d.DWSelectionID == (int)DecisionWhizardStatusEnum.No || d.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe).FirstOrDefault() != null ? true : false : false
                }).ToList();

                //lstEmailTemplateVM.QuoteId = quoteID;
                lstEmailTemplateVM.ToAddress = user.Email;
                lstEmailTemplateVM.RepComments = repComment;
                RepUser repUser = _Context.RepUser.GetSingle(e => e.RepID == UserVM.CRMModelProperties.RepID);
                lstEmailTemplateVM.FromAddress = repUser.User.Email;
                lstEmailTemplateVM.DisplayName = repUser.User.UserName;
                lstEmailTemplateVM.PersonID = (int)user.PersonID;
                lstEmailTemplateVM.RepPhoneFax = repUser.PhoneDirect;
                lstEmailTemplateVM.RepFirstName = repUser.User.FirstName;

                if (lstEmailTemplateVM.EmailDWTemplateList.Count > 1)
                {
                    var index = lstEmailTemplateVM.EmailDWTemplateList.FindIndex(x => x.QuoteID == quoteID);
                    if (index > 0)
                    {
                        var selectedQuote = lstEmailTemplateVM.EmailDWTemplateList[index];
                        lstEmailTemplateVM.EmailDWTemplateList[index] = lstEmailTemplateVM.EmailDWTemplateList[0];
                        lstEmailTemplateVM.EmailDWTemplateList[0] = selectedQuote;
                    }
                }
                EmailBodyGeneratorService emailService = new EmailBodyGeneratorService();

                //bool isInitial = selectedTemplate.Trim().ToLower() == "initial" ? true : false;
                if (selectedTemplate.Trim().ToLower() == "initial")
                    emailService.InitialiseHTMLParser(lstEmailTemplateVM, "initial", ConfigurationManager.AppSettings["DWInitialRepSubjectLine"], ConfigurationManager.AppSettings["DWinitialREPEmailType"]);
                else if (selectedTemplate.Trim().ToLower() == "initialtext")
                    emailService.InitialiseHTMLParser(lstEmailTemplateVM, "initialtext", ConfigurationManager.AppSettings["DWInitialRepSubjectLine"], ConfigurationManager.AppSettings["DWinitialREPEmailType"]);
                else if (selectedTemplate.Trim().ToLower() == "initialnewuser")
                    emailService.InitialiseHTMLParser(lstEmailTemplateVM, "initialnewuser", ConfigurationManager.AppSettings["DWInitialNewUserSubjectLine"], ConfigurationManager.AppSettings["DWinitialNewUser"]);
                else
                    emailService.InitialiseHTMLParser(lstEmailTemplateVM, "idle", ConfigurationManager.AppSettings["DWReminderRepSubjectLine"], ConfigurationManager.AppSettings["DWremindEmailType"]);

                if (selectedTemplate.Trim().ToLower().Contains("initial"))
                {
                    if (quote != null) quote.PenworthyUpdatedDate = DateTime.UtcNow;
                    _Context.Quote.SaveChanges();
                }
            }
            //if (quote != null) quote.PenworthyUpdatedDate = DateTime.UtcNow;
            //_Context.Quote.SaveChanges();
            return quote != null && quote.PenworthyUpdatedDate != null ? ((DateTime)quote.PenworthyUpdatedDate).ToLocalTime().ToString("M/d/yyyy h:mm tt") : string.Empty;
        }

        public SubmitQuoteViewModel GetUserAddressInfo(int quoteID)
        {
            SubmitQuoteViewModel sQVM = new SubmitQuoteViewModel();
            List<QuoteType> lstQuotetype = _Context.QuoteType.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.Direct || e.QuoteTypeID == (int)QuoteTypeEnum.Web || e.QuoteTypeID == (int)QuoteTypeEnum.Preview).OrderBy(e => e.QuoteTypeText).ToList();
            List<SourceType> lstSourceType = _Context.SourceType.GetAll().ToList();
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            sQVM.CustomerNo = Convert.ToString(UserVM.CRMModelProperties.CustNO);
            sQVM.Type = quote.QuoteType.QuoteTypeText;
            sQVM.Division = UserVM.CRMModelProperties.DivNO.ToString().Length == 1 ? "0" + UserVM.CRMModelProperties.DivNO.ToString() : UserVM.CRMModelProperties.DivNO.ToString();
            sQVM.LstInvoiceRecipient = GetCustomerEmails();
            sQVM.Comments = string.Empty;
            sQVM.CustAddressesModel = new CustomerAddressesModel();
            sQVM.CustAddressesModel = FillCustomerAddresses();
            sQVM.LstPayer = new List<ComboBase>();
            ComboBase parentInfo = new ComboBase();
            ComboBase parentInfoForAddInvoice = new ComboBase();
            if (sQVM.CustAddressesModel.ParentAddressModel != null)
            {
                AddressBaseModel ptAddBM = new AddressBaseModel();
                ptAddBM = sQVM.CustAddressesModel.ParentAddressModel;
                parentInfo = new ComboBase { ItemID = ptAddBM.AddressID, ItemValue = "(" + ptAddBM.CustomerNo + ") " + ptAddBM.CustomerName + ',' + ptAddBM.AddressLine1 + ',' + ptAddBM.AddressLine2 + ',' + ptAddBM.AddressLine3 + ',' + ptAddBM.City + ',' + ptAddBM.State + ',' + ptAddBM.ZipCode, Selected = false }; //AutoMapper.Mapper.Map<AddressBaseModel, ComboBase>(sQVM.CustAddressesModel.ParentAddressModel);
                parentInfoForAddInvoice = new ComboBase { ItemID = ptAddBM.AddressID, ItemValue = "(" + ptAddBM.CustomerNo + ") " + ptAddBM.CustomerName + ',' + ptAddBM.AddressLine1 + ',' + ptAddBM.AddressLine2 + ',' + ptAddBM.AddressLine3 + ',' + ptAddBM.City + ',' + ptAddBM.State + ',' + ptAddBM.ZipCode, Selected = false }; //AutoMapper.Mapper.Map<AddressBaseModel, ComboBase>(sQVM.CustAddressesModel.ParentAddressModel);
            }
            if (parentInfo.ItemID != null) sQVM.LstPayer.Add(parentInfo);
            ComboBase childinfo = new ComboBase();
            ComboBase childinfoForAddInvoice = new ComboBase();
            if (sQVM.CustAddressesModel.ChildAddressInfo != null)
            {
                AddressBaseModel chAddBM = new AddressBaseModel();
                chAddBM = sQVM.CustAddressesModel.ChildAddressInfo;
                childinfo = new ComboBase { ItemID = chAddBM.AddressID, ItemValue = "(" + chAddBM.CustomerNo + ") " + chAddBM.CustomerName + ',' + chAddBM.AddressLine1 + ',' + chAddBM.AddressLine2 + ',' + chAddBM.AddressLine3 + ',' + chAddBM.City + ',' + chAddBM.State + ',' + chAddBM.ZipCode, Selected = false };// AutoMapper.Mapper.Map<AddressBaseModel, ComboBase>(sQVM.CustAddressesModel.ChildAddressInfo);
                childinfoForAddInvoice = new ComboBase { ItemID = chAddBM.AddressID, ItemValue = "(" + chAddBM.CustomerNo + ") " + chAddBM.CustomerName + ',' + chAddBM.AddressLine1 + ',' + chAddBM.AddressLine2 + ',' + chAddBM.AddressLine3 + ',' + chAddBM.City + ',' + chAddBM.State + ',' + chAddBM.ZipCode, Selected = false };// AutoMapper.Mapper.Map<AddressBaseModel, ComboBase>(sQVM.CustAddressesModel.ChildAddressInfo);
            }
            if (childinfo.ItemID != null) sQVM.LstPayer.Add(childinfo);
            sQVM.LstShipItemsTo = new List<ComboBase>();
            if (childinfo.ItemID != null) sQVM.LstShipItemsTo.Add(childinfo);

            ComboBase childShipInfo = new ComboBase();
            childShipInfo = AutoMapper.Mapper.Map<ShipToAddressModel, ComboBase>(sQVM.CustAddressesModel.ShipToAddressInfo);
            if (childShipInfo.ItemID != null)
            {
                sQVM.LstShipItemsTo.Add(childShipInfo);
            }
            sQVM.FutureBillingDate = System.DateTime.Today;
            sQVM.LstSource = AutoMapper.Mapper.Map<IList<SourceType>, IList<ComboBase>>(lstSourceType).ToList();
            sQVM.ValidationStatus = ValidateQuote(quoteID);
            sQVM.LstAddInvRecipent = new List<ComboBase>();
            ComboBase cmbEmpty = new ComboBase { ItemID = "0", ItemValue = string.Empty };
            sQVM.LstAddInvRecipent.Insert(0, cmbEmpty);

            if (parentInfoForAddInvoice.ItemID != null) sQVM.LstAddInvRecipent.Add(parentInfoForAddInvoice);
            if (childinfoForAddInvoice.ItemID != null) sQVM.LstAddInvRecipent.Add(childinfoForAddInvoice);
            if (childShipInfo.ItemID != null)
            {
                sQVM.LstAddInvRecipent.Add(childShipInfo);
            }
            if (quote.QuoteTypeID == (int)QuoteTypeEnum.Web)
            {
                sQVM.Comments = quote.Comments;
                sQVM.PONo = quote.POText;
            }
            sQVM.QuoteID = quoteID;
            sQVM.UserVM = UserVM;
            sQVM.DBValue = 0;
            sQVM = FillSubmitQuoteViewModel(sQVM, quote);
            return sQVM;
        }

        private string GetCustomerDivNo(string customerNo)
        {
            var outPutPar = new SqlParameter
            {
                ParameterName = "DivNo",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size = 20
            };

            var Inputparameter = new SqlParameter
            {
                ParameterName = "CustomerNo",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Input,
                Size = 20,
                Value = customerNo

            };
            SqlParameter[] parameters = new SqlParameter[] { Inputparameter, outPutPar };


            // var List = db.ExecuteStoreQuery<ENT_SearchBusinessResult>("exec usp_BusinessUser_Search", Business, Location, PageNumber, RecordsPerPage, parm);
            string cutDivNo = _Context.Customer.ExecSpWithOutputParamter("[sp_GetCustomerDivNo] @CustomerNo,@DivNo OUTPUT", parameters);

            return cutDivNo;
        }

        private SubmitQuoteViewModel FillSubmitQuoteViewModel(SubmitQuoteViewModel sQVM, Quote quote)
        {
            if (quote.QuoteSubmitSaveInfo != null)
            {
                QuoteSubmitSaveInfo qssInfo = quote.QuoteSubmitSaveInfo;
                if (qssInfo.ShipItemsToID != null)
                {
                    if (sQVM.LstSource.Where(e => e.ItemID == qssInfo.SourceTypeID.ToString()).FirstOrDefault() != null)
                    {
                        sQVM.LstSource.Where(e => e.ItemID == qssInfo.SourceTypeID.ToString()).FirstOrDefault().Selected = true;
                    }
                    if (sQVM.LstInvoiceRecipient.Where(e => e.ItemID == qssInfo.InvoiceRecipientID.ToString()).FirstOrDefault() != null)
                    {
                        sQVM.LstInvoiceRecipient.Where(e => e.ItemID == qssInfo.InvoiceRecipientID.ToString()).FirstOrDefault().Selected = true;
                    }
                    sQVM.PONo = qssInfo.PoNo;
                    sQVM.FutureBillingDate = (DateTime?)qssInfo.FutureBillDate;
                    sQVM.Comments = qssInfo.QuoteSubmitComments;
                    sQVM.BillingReference = qssInfo.QuoteBillReference;
                    if (sQVM.LstPayer.Where(e => e.ItemID == qssInfo.PayerID).FirstOrDefault() != null)
                    {
                        sQVM.LstPayer.Where(e => e.ItemID == qssInfo.PayerID).FirstOrDefault().Selected = true;
                    }
                    if (sQVM.LstShipItemsTo.Where(e => e.ItemID == qssInfo.ShipItemsToID).FirstOrDefault() != null)
                    {
                        sQVM.LstShipItemsTo.Where(e => e.ItemID == qssInfo.ShipItemsToID).FirstOrDefault().Selected = true;
                    }
                    if (sQVM.LstAddInvRecipent.Where(e => e.ItemID == qssInfo.AdditionalInvoiceRecipientId).FirstOrDefault() != null)
                    {
                        sQVM.LstAddInvRecipent.Where(e => e.ItemID == qssInfo.AdditionalInvoiceRecipientId).FirstOrDefault().Selected = true;
                    }
                    sQVM.DBValue = (double)qssInfo.DBPrice;
                }
            }

            return sQVM;
        }


        public Dictionary<string, bool> ValidateQuote(int quoteID)
        {
            Dictionary<string, bool> dictQValidationFlagValues = new Dictionary<string, bool>();
            //Check for HREP
            dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_Representative, ValidateHREPStaus(quoteID));
            //check For Hold-PO
            dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_PO, ValidateHPOStaus(quoteID));
            //check For Hold-Credit
            dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_Credit, ValidateHCRDTStaus(quoteID));
            //check For Hold-2 previews
            dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_2_previews, ValidateH2PRVStaus(quoteID));
            //  //check For Hold-stick rate
            //dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_stick_rate, ValidateHSTICKStaus(quoteID));
            //check For Hold-Audit
            dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_Audit, ValidateHADTStaus(quoteID));
            //check For Hold-Profile Incomplete
            dictQValidationFlagValues.Add(QuoteValidationConstants.Hold_Profile_Incomplete, ValidateHPROFStaus(quoteID));
            return dictQValidationFlagValues;
        }

        private bool ValidateHPROFStaus(int quoteID)
        {
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            if (quote != null && (bool)quote.IncludeCatalogStatus)
            {
                return CheckCataalogInfo(UserVM.CRMModelProperties.CustAutoID);
            }
            return true;
        }

        private bool ValidateHADTStaus(int quoteID)
        {
            RepUser custRepo = _Context.RepUser.GetSingle(e => e.RepID == UserVM.CRMModelProperties.RepID);
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            if (custRepo != null)
            {
                bool isHoldAdit;
                string auditFlag = string.IsNullOrEmpty(custRepo.AuditFlag) ? string.Empty : custRepo.AuditFlag.ToLower();
                if (auditFlag == "a")
                {
                    isHoldAdit = quote != null ? quote.QuoteTypeID == (int)QuoteTypeEnum.Direct || quote.QuoteTypeID == (int)QuoteTypeEnum.Preview ? false : true : true;
                }
                else if (auditFlag == "c")
                {
                    isHoldAdit = quote != null ? quote.QuoteTypeID == (int)QuoteTypeEnum.Direct && quote.IncludeCatalogStatus == true ? false : true : true;
                }
                else
                {
                    IShoppingCartService _shoppingCartSrv = new ShoppingCartService();
                    _shoppingCartSrv.UserVM = UserVM;
                    List<FlatFileDetailModel> lstCatalogInfo = _shoppingCartSrv.GetCatalogInfoData(UserVM.CRMModelProperties.CustAutoID, quoteID, "Quote");
                    double totalPrice = quote.QuoteDetails != null ? (double)quote.QuoteDetails.Sum(z => z.Item.Price * z.Quantity) : 0;
                    double catalogPrice = ((bool)quote.IncludeCatalogStatus == true) ? lstCatalogInfo.Sum(e => e.Price) : 0;
                    string taxschuduleID = quote.User.Customer != null ? quote.User.Customer.TaxScheduleID : null;
                    double salteTaxPrice = 0;
                    if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                        salteTaxPrice = 0;
                    else
                        salteTaxPrice = Convert.ToDouble(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate) * (totalPrice + catalogPrice);
                    isHoldAdit = quote != null ? quote.QuoteDetails != null ? (totalPrice + catalogPrice + salteTaxPrice) >= 5000 ? false : true : true : true;
                }
                return isHoldAdit;
            }
            return true;
        }

        private bool ValidateHSTICKStaus(int quoteID)
        {
            Customer custInfo = _Context.Customer.GetSingle(e => e.CustomerNO != null && e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            if (custInfo != null)
            {
                bool isHoldStickStatus;
                isHoldStickStatus = quote != null ? quote.QuoteTypeID == (int)QuoteTypeEnum.Preview ? false : true : true;
                isHoldStickStatus = isHoldStickStatus == true && custInfo.Comp_stickratecy != null ? (decimal)custInfo.Comp_stickratecy <= 25 ? false : true : true;
                isHoldStickStatus = isHoldStickStatus == true && custInfo.Comp_stickratepy != null ? (decimal)custInfo.Comp_stickratepy <= 25 ? false : true : true;
                return isHoldStickStatus;
            }
            return true;
        }

        private bool ValidateH2PRVStaus(int quoteID)
        {
            List<Quote> lstQuotes = _Context.Quote.GetAll(e => e.UserID == UserVM.CRMModelProperties.LoggedINCustomerUserID).ToList();
            Quote currQuote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            if (lstQuotes != null && lstQuotes.Count > 0)
            {
                int holdPreviewCount = lstQuotes.Where(e => e.QuoteTypeID == (int)QuoteTypeEnum.Preview && e.StatusID == (int)QuoteStatusEnum.Shipped).Count();
                bool isHold2Preiveew = currQuote != null ? currQuote.QuoteTypeID == (int)QuoteTypeEnum.Preview && holdPreviewCount >= 2
                    && lstQuotes.Where(e => e.QuoteTypeID == (int)QuoteTypeEnum.Preview && e.StatusID == (int)QuoteStatusEnum.CallTagIssued).Count() == 0 ? false : true : true;
                //  bool isHold2Preiveew = quote.QuoteTypeID == (int)QuoteTypeEnum.Preview && quote.StatusID == (int)QuoteStatusEnum.Shipped && !quote.IsCalTag ? false : true;
                return isHold2Preiveew;
            }
            return true;
        }

        private bool ValidateHCRDTStaus(int quoteID)
        {
            Customer custInfo = _Context.Customer.GetSingle(e => e.CustomerNO != null && e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            if (custInfo != null)
            {
                bool isHoldCRDt = custInfo.AgingCategories2 > 0 || custInfo.AgingCategories3 > 0 || custInfo.AgingCategories4 > 0 ? false : true;
                return isHoldCRDt;
            }
            return true;
        }

        private bool ValidateHPOStaus(int quoteID)
        {
            //SqlParameter[] parameter = {
            //                                new SqlParameter("@CustumerAutoId", UserVM.CRMModelProperties.CustAutoID)
            //                            };


            //_Context.Customer.ExecSp("[SP_UpdateCustomerPOR//equirementByCustAutoId] @CustumerAutoId", parameter);
            // Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteID);
            Customer custInfo = _Context.Customer.GetSingle(e => e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            if (custInfo != null)
            {
                bool isHoldPOStatus = custInfo.Comp_PORequirements != null ? !custInfo.Comp_PORequirements.Contains("BeforeShipping") : true;
                //isHoldPOStatus = isHoldPOStatus == false &&quote.QuoteTypeID==QuoteTypeEnum.Web && (string.IsNullOrEmpty(quote.POText)) ? false : true;
                //if (quote.QuoteSubmitSaveInfo != null)
                //{
                //   isHoldPOStatus = isHoldPOStatus == false && (string.IsNullOrEmpty(quote.QuoteSubmitSaveInfo.PoNo)) ? false : true;
                // }
                return isHoldPOStatus;
            }
            return true;
        }

        private bool ValidateHREPStaus(int quoteID)
        {
            QuoteSubmitSaveInfo quoteSubmitSaveInfo = _Context.QuoteSubmitSaveInfo.GetSingle(e => e.QuoteID == quoteID);
            if (quoteSubmitSaveInfo != null)
            {
                return !quoteSubmitSaveInfo.IsRepoHold;
            }
            return true;
        }
        private CustomerAddressesModel FillCustomerAddresses()
        {
            CustomerAddressesModel customerAddressesModel = new CustomerAddressesModel();


            Customer childCustinfo = _Context.Customer.GetSingle(e => e.CustomerNO != null && e.CustAutoID == UserVM.CRMModelProperties.CustAutoID);
            string[] strCustomerBilltoShipTo = childCustinfo.CustomerNO.Split('_');
            string strBillTo = strCustomerBilltoShipTo.Count() > 0 ? strCustomerBilltoShipTo[0] : string.Empty;
            Customer parentCustInfo = _Context.Customer.GetSingle(e => e.CustomerNO == strBillTo + "_000" && e.DivisionNo == UserVM.CRMModelProperties.DivNO);

            customerAddressesModel.ParentAddressModel = GetParentChildAddressInfo(parentCustInfo);
            customerAddressesModel.ChildAddressInfo = GetParentChildAddressInfo(childCustinfo);
            customerAddressesModel.ShipToAddressInfo = GetShipToAddressInfo(childCustinfo);
            return customerAddressesModel;
        }

        private AddressBaseModel GetParentChildAddressInfo(Customer parentCustInfo)
        {
            AddressBaseModel parentChildAddressInfo = new AddressBaseModel();
            if (parentCustInfo != null)
            {
                parentChildAddressInfo = AutoMapper.Mapper.Map<CustomerAddress, AddressBaseModel>(parentCustInfo.CustomerAddress);

            }
            return parentChildAddressInfo;
        }

        private ShipToAddressModel GetShipToAddressInfo(Customer shiptoCustInfo)
        {
            ShipToAddressModel shipToAddressInfo = new ShipToAddressModel();
            if (shiptoCustInfo.CustomerShipToAddress != null)
            {
                shipToAddressInfo = new ShipToAddressModel
                {
                    ShipToAddress = new AddressBaseModel
                    {
                        AddressID = shiptoCustInfo.CustAutoID + "-S",
                        CustomerName = shiptoCustInfo.CustomerShipToAddress.Customer != null ? shiptoCustInfo.CustomerShipToAddress.Customer.CustomerName : string.Empty,
                        AddressLine1 = shiptoCustInfo.CustomerShipToAddress.ShipToAddress1,
                        AddressLine2 = shiptoCustInfo.CustomerShipToAddress.ShipToAddress2,
                        AddressLine3 = shiptoCustInfo.CustomerShipToAddress.ShipToAddress3,
                        City = shiptoCustInfo.CustomerShipToAddress.ShipToCity,
                        State = shiptoCustInfo.CustomerShipToAddress.ShipToState,
                        ZipCode = shiptoCustInfo.CustomerShipToAddress.ShipToZipCode,
                        CustomerNo = shiptoCustInfo.CustomerShipToAddress.Customer != null ? shiptoCustInfo.CustomerShipToAddress.Customer.CustomerNO : string.Empty
                    },

                    ShipToCode = shiptoCustInfo.CustomerShipToAddress.ShipToCode,
                    ShipToName = shiptoCustInfo.CustomerShipToAddress.ShipToName
                };

            }
            return shipToAddressInfo;
        }

        public ActiveQuoteViewModel SubmitQuote(SubmitQuoteViewModel sqvm, string saveOrSubmit)
        {
            int quoteId = sqvm.QuoteID;
            QuoteSubmitSaveInfo quoteSubmitSaveInfoDB = _Context.QuoteSubmitSaveInfo.GetSingle(e => e.QuoteID == quoteId);
            if (quoteSubmitSaveInfoDB == null)
            {
                QuoteSubmitSaveInfo quoteSubmitSaveInfo = new QuoteSubmitSaveInfo();
                quoteSubmitSaveInfo = FillQuoteSubmitSaveInfo(quoteSubmitSaveInfo, sqvm);
                quoteSubmitSaveInfo.CreatedDate = DateTime.UtcNow;
                _Context.QuoteSubmitSaveInfo.Add(quoteSubmitSaveInfo);
            }
            else
            {
                quoteSubmitSaveInfoDB = FillQuoteSubmitSaveInfo(quoteSubmitSaveInfoDB, sqvm);
                // quoteSubmitSaveInfo.CreatedDate = DateTime.UtcNow;
                quoteSubmitSaveInfoDB.UpdatedDate = DateTime.UtcNow;
            }
            _Context.QuoteSubmitSaveInfo.SaveChanges();
            if (saveOrSubmit != "Submit" && saveOrSubmit != "Manager Submit")
            {
                ChangeQuoteStatus(quoteId, "Save");
            }
            else
            {
                FileCreationService _fileCreation = new FileCreationService();
                _fileCreation.UserVM = UserVM;
                _fileCreation.FileCreation(sqvm);
                ChangeQuoteStatus(quoteId, saveOrSubmit);
                CustomerCatalogBarcodeManipulation((int)UserVM.CRMModelProperties.CustAutoID, quoteId);
            }

            ActiveQuoteViewModel acVM = new ActiveQuoteViewModel();//GetActiveQuoteVM();
            return acVM;
        }

        private QuoteSubmitSaveInfo FillQuoteSubmitSaveInfo(QuoteSubmitSaveInfo quoteSubmitSaveInfo, SubmitQuoteViewModel sqvm)
        {
            // Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == sqvm.QuoteID);
            quoteSubmitSaveInfo.AdditionalInvoiceRecipientId = sqvm.AddInvRecipient;
            quoteSubmitSaveInfo.DBPrice = (decimal)sqvm.DBValue;
            quoteSubmitSaveInfo.FutureBillDate = sqvm.FutureBillingDate;
            quoteSubmitSaveInfo.InvoiceRecipientID = Convert.ToInt32(sqvm.InvUserID);
            quoteSubmitSaveInfo.PoNo = sqvm.PONo;
            quoteSubmitSaveInfo.PayerID = sqvm.Payer.ToString();
            quoteSubmitSaveInfo.QuoteBillReference = sqvm.BillingReference;
            quoteSubmitSaveInfo.QuoteID = sqvm.QuoteID;
            quoteSubmitSaveInfo.QuoteSubmitComments = sqvm.Comments;
            // quoteSubmitSaveInfo.QuoteTypeID = quote.QuoteTypeID;
            quoteSubmitSaveInfo.IsRepoHold = sqvm.ValidationStatus[QuoteValidationConstants.Hold_Representative];
            quoteSubmitSaveInfo.ShipItemsToID = sqvm.ShipItemsTo;
            quoteSubmitSaveInfo.SourceTypeID = Convert.ToInt32(sqvm.LstSource.FirstOrDefault().ItemID);
            quoteSubmitSaveInfo.UpdatedDate = DateTime.UtcNow;
            return quoteSubmitSaveInfo;
        }
        private void ChangeQuoteStatus(int quoteid, string type = "")
        {
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            int statusId = 0;
            switch (type)
            {
                case "Submit":
                    string failValidationStatus = GetFailedValidationKey(quote);//dictQValidationFlagValues.Where(k => k.Value == false).FirstOrDefault().Key;
                    User loggedInUser = _Context.User.GetSingle(e => e.UserId == UserVM.CRMModelProperties.LoggedINUserID);
                    int rollID = loggedInUser.webpages_Roles.FirstOrDefault().RoleId;
                    if (!string.IsNullOrEmpty(failValidationStatus) && rollID != (int)UserRolesEnum.AdminRep)
                    {
                        statusId = _Context.Status.GetSingle(e => e.StatusFlag == failValidationStatus).StatusID;
                    }
                    else
                    {
                        statusId = (int)QuoteStatusEnum.Transferred;
                    }
                    break;
                case "Save":
                    string failedValidationStatus = GetFailedValidationKey(quote);//dictQValidationFlagValues.Where(k => k.Value == false).FirstOrDefault().Key;
                    if (!string.IsNullOrEmpty(failedValidationStatus))
                    {
                        statusId = _Context.Status.GetSingle(e => e.StatusFlag == failedValidationStatus).StatusID;
                    }
                    else
                    {
                        if (quote.QuoteTypeID == (int)QuoteTypeEnum.Web)
                        {
                            statusId = (int)QuoteStatusEnum.Web;
                        }
                        else
                        {
                            statusId = (int)QuoteStatusEnum.Open;
                        }
                    }
                    break;
                case "CalTag":
                    statusId = (int)QuoteStatusEnum.CTPending;
                    break;
                case "Manager Submit":
                    statusId = (int)QuoteStatusEnum.Transferred;
                    break;
                default:
                    break;
            }
            quote.StatusID = statusId;
            quote.UpdateDate = DateTime.UtcNow;
            _Context.Quote.SaveChanges();


        }
        private string GetFailedValidationKey(Quote quote)
        {
            string failValidationStatus = string.Empty;
            Dictionary<string, bool> dictQValidationFlagValues = ValidateQuote(quote.QuoteID);
            failValidationStatus = dictQValidationFlagValues.Where(k => k.Value == false).FirstOrDefault().Key;
            if (failValidationStatus == QuoteValidationConstants.Hold_PO)
            {
                //if (quote.QuoteTypeID == (int)QuoteTypeEnum.Web)
                //{
                //    failValidationStatus = string.IsNullOrEmpty(quote.POText) ? failValidationStatus : string.Empty;
                //}
                if (quote.QuoteSubmitSaveInfo != null)
                {
                    failValidationStatus = string.IsNullOrEmpty(quote.QuoteSubmitSaveInfo.PoNo) ? failValidationStatus : string.Empty;
                }
                if (string.IsNullOrEmpty(failValidationStatus))
                {
                    failValidationStatus = dictQValidationFlagValues.Where(k => k.Value == false && k.Key != QuoteValidationConstants.Hold_PO).FirstOrDefault().Key;
                }
            }
            return failValidationStatus;
        }
        public Dictionary<string, bool> UpdateRepoHoldStatus(int quoteID, bool isRepoHold)
        {
            QuoteSubmitSaveInfo quoteSubmitSaveInfoDB = _Context.QuoteSubmitSaveInfo.GetSingle(e => e.QuoteID == quoteID);
            if (quoteSubmitSaveInfoDB != null)
            {
                quoteSubmitSaveInfoDB.IsRepoHold = isRepoHold;
                quoteSubmitSaveInfoDB.UpdatedDate = DateTime.UtcNow;
            }
            else
            {
                quoteSubmitSaveInfoDB = new QuoteSubmitSaveInfo();
                quoteSubmitSaveInfoDB.QuoteID = quoteID;
                quoteSubmitSaveInfoDB.IsRepoHold = isRepoHold;
                quoteSubmitSaveInfoDB.CreatedDate = DateTime.UtcNow;
                quoteSubmitSaveInfoDB.UpdatedDate = DateTime.UtcNow;
                _Context.QuoteSubmitSaveInfo.Add(quoteSubmitSaveInfoDB);
            }
            _Context.QuoteSubmitSaveInfo.SaveChanges();
            ChangeQuoteStatus(quoteID, "Save");
            Dictionary<string, bool> dictQValidationFlagValues = ValidateQuote(quoteID);
            return dictQValidationFlagValues;
        }

        public OrderViewModel GeneratePDF(int quoteId)
        {
            //for Bill To ship To address
            CustomerAddressesModel customerAddresses = new CustomerAddressesModel();

            ShoppingCartService SCService = new ShoppingCartService();
            SCService.UserVM = UserVM;

            if (UserVM != null)
            {
                customerAddresses = FillCustomerAddresses();
            }

            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteId);

            string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
            string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);

            List<Item> lstItem = quote != null ? quote.StatusID == (int)QuoteStatusEnum.Invoiced ? quote.QuoteDetails.Select(e => e.Item).ToList() : quote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).Select(e => e.Item).ToList() : new List<Item>();
            InvoiceHistory invoicHistory = _Context.InvoiceHistory.GetSingle(e => e.QuoteID == quoteId);
            invoicHistory = invoicHistory == null ? _Context.InvoiceHistory.GetSingle(e => e.CustAutoId == quote.User.CustAutoID) : invoicHistory;
            OrderViewModel orderVM = new OrderViewModel();

            orderVM.InvoiceModel = new InvoiceModel();
            orderVM.RepoAddress = new CRMModel();
            RepUser repoAddress;// = quote.User.Customer.CustomerRep.RepUser != null ? quote.User.Customer.CustomerRep.RepUser : null;
            if (quote.User.Customer.CustomerRep == null)
            {
                repoAddress = _Context.RepUser.GetSingle(e => e.RepUserID == 25);//Defualt repuerid - 25 - Julie.Plantz
            }
            else
            {
                repoAddress = quote.User.Customer.CustomerRep.RepUser != null ? quote.User.Customer.CustomerRep.RepUser : null;
            }

            orderVM.RepoAddress.RepName = repoAddress.User != null ? repoAddress.User.FirstName + "  " + repoAddress.User.LastName : string.Empty;
            orderVM.RepoAddress.RepEmail = repoAddress.User != null ? repoAddress.User.Email : string.Empty;
            orderVM.RepoAddress.CustFirstName = quote.User != null ? quote.User.FirstName : string.Empty;
            orderVM.RepoAddress.CustLastName = quote.User != null ? quote.User.LastName : string.Empty;
            orderVM.RepoAddress.Persphone = repoAddress != null ? repoAddress.PhoneCustomerService : string.Empty;
            orderVM.CustNumber = quote.User.Customer != null ? quote.User.Customer.CustomerNO : string.Empty;
            orderVM.CustDivNo = quote.User.Customer != null ? Convert.ToString(quote.User.Customer.DivisionNo).Length == 1 ? "0" + Convert.ToString(quote.User.Customer.DivisionNo) : Convert.ToString(quote.User.Customer.DivisionNo) : string.Empty;
            orderVM.CustomerName = quote.User != null ? quote.User.Customer != null ? quote.User.Customer.CustomerName : string.Empty : string.Empty;
            orderVM.QuoteID = quoteId;
            if (invoicHistory != null)
            {
                orderVM.InvoiceModel.InvoiceDate = Convert.ToDateTime(invoicHistory.InvoiceDate);
                orderVM.InvoiceModel.OrderNumber = invoicHistory.OrderNo.ToString();
                orderVM.InvoiceModel.Terms = invoicHistory.TermsCode;
                orderVM.InvoiceModel.FederalID = invoicHistory.FederalId;
                orderVM.InvoiceNumber = invoicHistory.InvoiceNo;
                // orderVM.CustNumber =UserVM!=null? UserVM.CRMModelProperties.CustNO:string.Empty;
                orderVM.InvoiceModel.Questions = UserVM != null ? UserVM.CRMModelProperties.RepName + "<br/>" + UserVM.CRMModelProperties.RepPhoneCustSrvc + "  " + UserVM.CRMModelProperties.RepPhoneDirect : string.Empty;
                //added by faraaz
                orderVM.InvoiceModel.CustomerPO = invoicHistory.CustomerPurchaseOrderNo;
                orderVM.InvoiceModel.InvoiceType = invoicHistory.InvoiceType;
            }
            if (quote.QuoteSubmitSaveInfo != null)
                orderVM.PurchaseOrderNumber = quote.QuoteSubmitSaveInfo.PoNo;
            orderVM.CartListView = lstItem.Select(c => new CartViewModel
            {
                Title = c.Title,
                ISBN = c.ISBN,
                ItemPrice = (double)c.Price,
                TotalPrice = (double)c.Price,
                Type = string.IsNullOrEmpty(c.ProductLine) ? string.Empty : c.ProductLine,
                Series = c.SeriesAndCharacter1 == null ? "" : c.SeriesAndCharacter1.SCText,
                Quantity = c.QuoteDetails.Where(e => e.ItemID == c.ItemID && e.QuoteID == quoteId).FirstOrDefault().Quantity,
                AcRcLevelText = (c.ARLevel != null && Convert.ToDouble(c.ARLevel) > 0 ? "AR" : "") + (c.ARLevel != null && c.RCLevel != null && Convert.ToDouble(c.ARLevel) > 0 && Convert.ToDouble(c.RCLevel) > 0 ? "," : "") + (c.RCLevel != null && Convert.ToDouble(c.RCLevel) > 0 ? "RC" : "")
            }).OrderBy(e => e.Title).ToList();

            if ((bool)quote.IncludeCatalogStatus == true)
            {
                List<FlatFileDetailModel> lstCatalogInfo = SCService.GetCatalogInfoData((int)quote.User.CustAutoID, quoteId, "Cart");
                orderVM.CartListView.AddRange(lstCatalogInfo.Select(c => new CartViewModel
                {
                    Title = c.ItemNumber,
                    Author = string.Empty,
                    ISBN = string.Empty,
                    AR = null,
                    Lexile = string.Empty,
                    ItemId = "Catalog",
                    Quantity = c.Quantity,
                    ItemPrice = c.ItemPrice,
                    Price = c.Price,
                    IncludeCatalog = c.ItemNumber == "Special Bulk Charge" ? false : true,
                    Type = c.ProductLine
                }).ToList());
            }
            if (customerAddresses.ParentAddressModel != null)
            {
                orderVM.BillTO = customerAddresses.ParentAddressModel;
            }
            if (customerAddresses.ShipToAddressInfo != null)
            {
                orderVM.ShipTO = customerAddresses.ShipToAddressInfo;

            }
            if (quote != null)
            {
                string taxschuduleID = quote.User.Customer != null ? quote.User.Customer.TaxScheduleID : null;
                if (string.IsNullOrEmpty(taxschuduleID) || taxschuduleID == "NONTAX")
                    orderVM.SalesTax = 0;
                else

                    orderVM.SalesTax = Convert.ToDouble(_Context.TaxSchedule.GetSingle(e => e.TaxScheduleID == taxschuduleID).TaxRate);

            }
            if (UserVM != null)
            {
                orderVM.UserVM = UserVM;
                orderVM.UserVM.CRMModelProperties.LoggedINCustomerUserID = quote.UserID;
            }
            return orderVM;



        }
        public CalTagViewModel GetCalTagInfo(int quoteId)
        {
            List<CalTagInfo> calTagInfo = _Context.CalTagInfo.GetAll().ToList();
            CalTagViewModel calTagVM = new CalTagViewModel();
            calTagVM.CalTagInfo = calTagInfo;
            calTagVM.LstInvoiceTo = new List<ComboBase>();
            CustomerAddressesModel CustAddressesModel = FillCustomerAddresses();
            ComboBase cmbEmpty = new ComboBase { ItemID = "0", ItemValue = string.Empty };
            calTagVM.LstInvoiceTo.Add(cmbEmpty);
            ComboBase parentInfo = new ComboBase();
            if (CustAddressesModel.ParentAddressModel != null)
            {
                parentInfo = new ComboBase { ItemID = CustAddressesModel.ParentAddressModel.AddressID, ItemValue = '(' + CustAddressesModel.ParentAddressModel.CustomerNo + ") " + CustAddressesModel.ParentAddressModel.CustomerName + ',' + CustAddressesModel.ParentAddressModel.AddressLine1 + ',' + CustAddressesModel.ParentAddressModel.AddressLine2 + ',' + CustAddressesModel.ParentAddressModel.AddressLine3 + ',' + CustAddressesModel.ParentAddressModel.City + ',' + CustAddressesModel.ParentAddressModel.State + ',' + CustAddressesModel.ParentAddressModel.ZipCode, Selected = false }; //AutoMapper.Mapper.Map<AddressBaseModel, ComboBase>(sQVM.CustAddressesModel.ParentAddressModel);
                calTagVM.LstInvoiceTo.Add(parentInfo);
            }
            ComboBase childinfo = new ComboBase();
            if (CustAddressesModel.ChildAddressInfo != null)
            {
                childinfo = new ComboBase { ItemID = CustAddressesModel.ChildAddressInfo.AddressID, ItemValue = '(' + CustAddressesModel.ChildAddressInfo.CustomerNo + ") " + CustAddressesModel.ChildAddressInfo.CustomerName + ',' + CustAddressesModel.ChildAddressInfo.AddressLine1 + ',' + CustAddressesModel.ChildAddressInfo.AddressLine2 + ',' + CustAddressesModel.ChildAddressInfo.AddressLine3 + ',' + CustAddressesModel.ChildAddressInfo.City + ',' + CustAddressesModel.ChildAddressInfo.State + ',' + CustAddressesModel.ChildAddressInfo.ZipCode, Selected = false };// AutoMapper.Mapper.Map<AddressBaseModel, ComboBase>(sQVM.CustAddressesModel.ChildAddressInfo);
                calTagVM.LstInvoiceTo.Add(childinfo);
            }
            ComboBase childShipInfo = new ComboBase();
            childShipInfo = AutoMapper.Mapper.Map<ShipToAddressModel, ComboBase>(CustAddressesModel.ShipToAddressInfo);
            if (childShipInfo.ItemID != null)
            {
                calTagVM.LstInvoiceTo.Add(childShipInfo);
            }
            calTagVM.QuoteID = quoteId;
            return calTagVM;
        }
        public void InsertCalTagInfo(CalTagViewModel calTagVM)
        {
            List<QuoteCallTag> quoteCatalog = _Context.QuoteCallTag.GetAll(e => e.QuoteID == calTagVM.QuoteID).ToList();
            if (quoteCatalog != null && quoteCatalog.Count > 0)
            {
                quoteCatalog[0].CalTagInfoValue = calTagVM.Email;
                quoteCatalog[1].CalTagInfoValue = calTagVM.FutureDate.ToString();
                quoteCatalog[2].CalTagInfoValue = calTagVM.PONo;
                quoteCatalog[3].CalTagInfoValue = calTagVM.IsCataloging.ToString();
                quoteCatalog[4].CalTagInfoID = Convert.ToInt32(calTagVM.LstSendOptions[0].ItemID.Trim());
                quoteCatalog[4].CalTagInfoValue = "true";
                quoteCatalog[5].CalTagInfoValue = calTagVM.InvoiceTo;
            }
            else
            {
                QuoteCallTag emailCalTagInfo = new QuoteCallTag();
                emailCalTagInfo.CalTagInfoID = 8;
                emailCalTagInfo.QuoteID = calTagVM.QuoteID;
                emailCalTagInfo.CalTagInfoValue = calTagVM.Email;
                _Context.QuoteCallTag.Add(emailCalTagInfo);
                QuoteCallTag futureDateCalTagInfo = new QuoteCallTag();
                futureDateCalTagInfo.CalTagInfoID = 7;
                futureDateCalTagInfo.QuoteID = calTagVM.QuoteID;
                futureDateCalTagInfo.CalTagInfoValue = calTagVM.FutureDate.ToString();
                _Context.QuoteCallTag.Add(futureDateCalTagInfo);
                QuoteCallTag poNoCalTagInfo = new QuoteCallTag();
                poNoCalTagInfo.CalTagInfoID = 6;
                poNoCalTagInfo.QuoteID = calTagVM.QuoteID;
                poNoCalTagInfo.CalTagInfoValue = calTagVM.PONo;
                _Context.QuoteCallTag.Add(poNoCalTagInfo);
                QuoteCallTag isCataLogCalTagInfo = new QuoteCallTag();
                isCataLogCalTagInfo.CalTagInfoID = 5;
                isCataLogCalTagInfo.QuoteID = calTagVM.QuoteID;
                isCataLogCalTagInfo.CalTagInfoValue = calTagVM.IsCataloging.ToString();
                _Context.QuoteCallTag.Add(isCataLogCalTagInfo);
                QuoteCallTag sendOptalTagInfo = new QuoteCallTag();
                sendOptalTagInfo.CalTagInfoID = Convert.ToInt32(calTagVM.LstSendOptions[0].ItemID.Trim());
                sendOptalTagInfo.QuoteID = calTagVM.QuoteID;
                sendOptalTagInfo.CalTagInfoValue = "true";
                _Context.QuoteCallTag.Add(sendOptalTagInfo);
                QuoteCallTag invoiceCalTagInfo = new QuoteCallTag();
                invoiceCalTagInfo.CalTagInfoID = 4;
                invoiceCalTagInfo.QuoteID = calTagVM.QuoteID;
                invoiceCalTagInfo.CalTagInfoValue = calTagVM.InvoiceTo;
                _Context.QuoteCallTag.Add(invoiceCalTagInfo);
            }
            if (calTagVM.LstSendOptions[0].ItemID.Trim() == "3")
            {
                //flat File Generation

                FileCreationService _fileCreation = new FileCreationService();
                _fileCreation.UserVM = UserVM;
                _fileCreation.CalTagFileCreation(calTagVM);
            }
            else
            {
                ChangeQuoteStatus(calTagVM.QuoteID, "CalTag");
            }
            //if (calTagVM.IsCataloging)
            //{
            //    QuoteModel quoteModel = new QuoteModel();
            //    quoteModel.CreatedDate = DateTime.UtcNow;
            //    quoteModel.QuoteTypeID = (int)QuoteTypeEnum.DecisionWhizard;
            //    quoteModel.QuoteText = "CalTagInfo";
            //    quoteModel.TotalItems = 0;
            //    quoteModel.TotalPrice = 0;
            //    quoteModel.UpdateDate = DateTime.UtcNow;
            //    quoteModel.StatusID = 1;
            //    quoteModel.CustUserID = UserVM.CRMModelProperties.LoggedINCustomerUserID;
            //    quoteModel.ARDivisionNo = UserVM.CRMModelProperties.DivNO;
            //    CreateQuote(quoteModel);
            //}
            _Context.QuoteCallTag.SaveChanges();

        }

        public List<CalTagViewModel> GetQuoteByCTStatus()
        {
            List<Quote> lstQuotes = _Context.Quote.GetAll(x => x.StatusID == (int)QuoteStatusEnum.CTPending).ToList();
            List<User> lstusers = lstQuotes.Select(u => u.User).ToList();
            List<CalTagViewModel> lstCallTagViewModel = new List<CalTagViewModel>();


            foreach (Quote oQuote in lstQuotes)
            {
                CalTagViewModel calTagVM = new CalTagViewModel();
                calTagVM.QuoteID = oQuote.QuoteID;
                calTagVM.PrimaryContact = lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault() == null ? "" : lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault().Customer.CustomerName;
                calTagVM.AddressLine2 = lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault() == null ? "" : lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault().Customer.CustomerAddress.AddressLine2;
                calTagVM.AddressLine3 = lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault() == null ? "" : lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault().Customer.CustomerAddress.AddressLine3;
                calTagVM.City = lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault() == null ? "" : lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault().Customer.CustomerAddress.City;
                calTagVM.State = lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault() == null ? "" : lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault().Customer.CustomerAddress.State;
                calTagVM.PostalCode = lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault() == null ? "" : lstusers.Where(u => u.UserId == oQuote.UserID).FirstOrDefault().Customer.CustomerAddress.ZipCode;
                calTagVM.ActualWeight = oQuote.QuoteDetails.Sum(e => e.Quantity) * 1;
                //calTagVM.ShipToName = "";
                calTagVM.MerchandiseDescription = "LIBY BOOKS";
                calTagVM.Code = "MSCT";
                calTagVM.ReturnSrvType = oQuote.QuoteCallTags.Where(q => q.CalTagInfoValue == "true").FirstOrDefault() != null ? oQuote.QuoteCallTags.Where(q => q.CalTagInfoValue == "true").FirstOrDefault().CalTagInfoID == 2 ? "ERL" : "RS1" : "RS1";
                lstCallTagViewModel.Add(calTagVM);
            }

            return lstCallTagViewModel;
        }

        public void ChangeCTStatus()
        {
            List<Quote> lstQuotes = _Context.Quote.GetAll(x => x.StatusID == (int)QuoteStatusEnum.CTPending).ToList();
            for (int i = 0; i < lstQuotes.Count; i++)
            {
                lstQuotes[i].StatusID = (int)QuoteStatusEnum.CallTagIssued;
            }
            _Context.Quote.SaveChanges();

        }

        public List<EmailTemplateViewModel> GetDwTemplateEmail()
        {
            int days = ConfigurationManager.AppSettings["GetDWQuotesFromDays"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings["GetDWQuotesFromDays"]) : 60;
            DateTime subtractedDate = DateTime.UtcNow.AddDays(-days);//Check for 60days from the date of creation
            DateTime dtlast24hours = DateTime.UtcNow.AddHours(-24);
            List<Quote> lstQuotes = new List<Quote>();
            //List<Quote> lstQuotes = _Context
            //  .Quote.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == 1 && e.QuoteDetails.Count > 0 && e.PenworthyUpdatedDate >= subtractedDate && e.User.Customer.CustomerRep != null).Take(20).ToList();

            if (Convert.ToInt32(ConfigurationManager.AppSettings["Testing"]) == 1)
            {
                //List<Quote> lstQuotesa = _Context
                //    .Quote.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == 1 && e.QuoteDetails.Count > 0 && e.User.Customer.CustomerRep != null).ToList();
                //if (lstQuotesa != null && lstQuotesa.Count > 0)
                //{
                //    lstQuotes = lstQuotesa.Where(e => (int)(e.PenworthyUpdatedDate.Value - DateTime.UtcNow).TotalDays == -4).Take(5).ToList();
                //    lstQuotes.AddRange(lstQuotesa.Where(e => (int)(e.PenworthyUpdatedDate.Value - DateTime.UtcNow).TotalDays == -10).Take(5).ToList());
                //    lstQuotes.AddRange(lstQuotesa.Where(e => (int)(e.PenworthyUpdatedDate.Value - DateTime.UtcNow).TotalDays == -19).Take(5).ToList());
                //    lstQuotes.AddRange(lstQuotesa.Where(e => (int)(e.PenworthyUpdatedDate.Value - DateTime.UtcNow).TotalDays == -45).Take(5).ToList());
                //    lstQuotes.AddRange(lstQuotesa.Where(e => (int)(e.PenworthyUpdatedDate.Value - DateTime.UtcNow).TotalDays == -60).Take(5).ToList());
                //}
                lstQuotes = _Context.Quote.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == 1 && e.QuoteDetails.Count > 0 && e.PenworthyUpdatedDate >= subtractedDate && e.User.Customer.CustomerRep != null).ToList();
            }
            else
            {
                lstQuotes = _Context
                    .Quote.GetAll(e => e.QuoteTypeID == (int)QuoteTypeEnum.DecisionWhizard && e.StatusID == 1 && e.QuoteDetails.Count > 0 && e.PenworthyUpdatedDate >= subtractedDate && e.User.Customer.CustomerRep != null).ToList();

            }

            List<User> lstUsers = lstQuotes.GroupBy(e => e.User).Select(e => e.Key).ToList();
            List<EmailTemplateViewModel> lstEmailTemplateVM = new List<EmailTemplateViewModel>();
            foreach (User user in lstUsers)
            {
                List<Quote> lstDWQuotes = user.Quotes.Where(e => e.QuoteTypeID == 2 && e.StatusID != 2 && e.QuoteDetails.Count > 0).OrderByDescending(e => e.PenworthyUpdatedDate).ToList();
                List<Quote> lstWebSubInvoiceQuotes = user.Quotes.Where(e => e.QuoteTypeID == 22 && e.QuoteDetails.Count > 0 && (e.StatusID == 7 || e.StatusID == 10) && e.PenworthyUpdatedDate > lstDWQuotes.FirstOrDefault().PenworthyUpdatedDate).ToList();

                if (lstWebSubInvoiceQuotes != null && lstWebSubInvoiceQuotes.Count == 0)
                {
                    EmailTemplateViewModel emailVM = new EmailTemplateViewModel();
                    emailVM.EmailDWTemplateList = lstDWQuotes.Select(q => new EmailTemplateModel
                    {
                        QuoteID = q.QuoteID,
                        PenworthyUpdatedDate = (DateTime)q.PenworthyUpdatedDate,
                        DWName = q.QuoteTitle,
                        NoOfDays = (int)(DateTime.UtcNow - (DateTime)q.PenworthyUpdatedDate).TotalDays,

                    }).ToList();
                    //emailVM.QuoteId = lstUserDWQuotes.Select(e => e.QuoteID).FirstOrDefault();
                    emailVM.FromAddress = user.Customer.CustomerRep.RepUser.User.Email;
                    emailVM.DisplayName = user.Customer.CustomerRep.RepUser.User.UserName;
                    emailVM.ToAddress = user.Email;
                    emailVM.PersonID = user.PersonID == null ? 0 : (int)user.PersonID;
                    //emailVM.IsMailSent = lstEmailHistory.Where(e => e.quoteId != null && e.quoteId != 0 && e.quoteId != emailVM.QuoteId && e.To == userIDAndQuotes.Key.Email).ToList().Count == 0 ? false : true;
                    emailVM.EmailDWTemplateList.FirstOrDefault().LstISBN = GetImageISBNsForEmail(lstDWQuotes.FirstOrDefault());
                    emailVM.EmailDWTemplateList.FirstOrDefault().IsActive = SetActiveStatusFromQuote(lstDWQuotes.FirstOrDefault());
                    lstEmailTemplateVM.Add(emailVM);
                }
            }
            lstEmailTemplateVM = lstEmailTemplateVM.GroupBy(e => e.EmailDWTemplateList.GroupBy(g => g.QuoteID)).Select(a => a.FirstOrDefault()).ToList();
            return lstEmailTemplateVM;

        }

        private bool SetActiveStatusFromQuote(Quote quote)
        {
            return quote.QuoteDetails != null && quote.QuoteDetails.Where(e => e.DWSelectionID == (int)DecisionWhizardStatusEnum.MayBe || e.DWSelectionID == (int)DecisionWhizardStatusEnum.Yes || e.DWSelectionID == (int)DecisionWhizardStatusEnum.No).FirstOrDefault() != null ? true : false;
        }

        private List<string> GetImageISBNsForEmail(Quote quote)
        {
            List<string> lstIsbns = new List<string>();

            List<QuoteDetail> lstDwSeries = quote != null && quote.QuoteDetails != null && quote.QuoteDetails.Count > 0 ? quote.QuoteDetails.Where(e => e.DWSelectionID != (int)DecisionWhizardStatusEnum.No).ToList() : null;

            if (lstDwSeries != null && lstDwSeries.Count > 0)
            {
                lstIsbns.AddRange(GetIsbnByTypeAndDWSelection(lstDwSeries, "ppb", DecisionWhizardStatusEnum.New));
                if (lstIsbns.Count < 5)
                {
                    lstIsbns.AddRange(GetIsbnByTypeAndDWSelection(lstDwSeries, "ppb", DecisionWhizardStatusEnum.Yes));
                    if (lstIsbns.Count < 5)
                    {
                        lstIsbns.AddRange(GetIsbnByTypeAndDWSelection(lstDwSeries, "ppb", DecisionWhizardStatusEnum.MayBe));
                        if (lstIsbns.Count < 5)
                        {
                            lstDwSeries.RemoveAll(e => lstIsbns.Contains(e.Item.ISBN));
                            lstIsbns.AddRange(lstDwSeries.Select(e => e.Item.ISBN));
                        }
                    }
                }
            }

            return lstIsbns.Take(5).ToList();
        }

        private List<string> GetIsbnByTypeAndDWSelection(List<QuoteDetail> lstDwSeries, string type, DecisionWhizardStatusEnum decisionWhizardStatusEnum)
        {
            List<string> lstIsbns = new List<string>();
            var lstSeries = lstDwSeries.Where(e => e.Item.ProductLine.Trim().ToLower() == type && e.DWSelectionID == (int)decisionWhizardStatusEnum).GroupBy(e => e.Item.Series).ToList();
            foreach (var item in lstSeries)
            {
                lstIsbns.AddRange(item.Select(e => e.Item.ISBN).ToList());

            }
            return lstIsbns;

        }

        string statusenumB = Convert.ToString((char)ItemStatusEnum.OnListButNotPreViewable);
        string statusenumD = Convert.ToString((char)ItemStatusEnum.OnListAndPreViewable);
        public RapidEntryModel getRapidEntry(int quoteid)
        {
            Quote quote = _Context.Quote.GetSingle(e => e.QuoteID == quoteid);
            RapidEntryModel rapidEntryModel = new RapidEntryModel();
            rapidEntryModel.QuoteID = quoteid;
            rapidEntryModel.QuoteTotalBefortaxandCatlog = quote.QuoteDetails != null ? (double)quote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).Sum(z => z.Item.Price * z.Quantity) : 0;
            rapidEntryModel.QuantityPerItem = 1;
            rapidEntryModel.NoOfTitles = quote.QuoteDetails != null ? quote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).ToList().Count() : 0;
            rapidEntryModel.NoOfBooks = quote.QuoteDetails != null ? quote.QuoteDetails.Where(e => e.Item.Status == statusenumB || e.Item.Status == statusenumD).Sum(x => x.Quantity) : 0;
            rapidEntryModel.QuoteTypeID = (int)quote.QuoteTypeID;
            return rapidEntryModel;
        }
    }

}
