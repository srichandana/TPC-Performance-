using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core.Models;
using TPC.Web.Filters;
using TPC.Core;
using TPC.Core.Interfaces;
using Microsoft.Practices.Unity;
using TPC.Core.Models.ViewModels;
using TPC.Core.Models.Models;
using System.Reflection;
using TPC.Core.Infrastructure;

namespace TPC.Web.Controllers.TCPControllers
{

    [TPCAuthorize]

    public class CatalogInfoController : BaseController
    {
        private readonly ICatalogInfoService _catalogInfoSrv;

        public CatalogInfoController(CatalogInfoService catalogInfoSrv)
        {
            _catalogInfoSrv = catalogInfoSrv;
            _catalogInfoSrv.UserVM = UserVM;
        }

        public ActionResult ViewCatalogInfo(int custAutoID, string catalogInsertStatus = "", int quoteid = 0)
        {
            _catalogInfoSrv.UserVM = UserVM;
            ViewData["CatalogInsertStatus"] = !string.IsNullOrEmpty(catalogInsertStatus) ? catalogInsertStatus : "false";
            //getting the View from the Service along with the dropdwn values 
            CatalogInfoViewModel catInfoVM = _catalogInfoSrv.getCatalogInfoValues(custAutoID);
            if (quoteid != 0)
            {
                UserVM.CurrentQuoteID = quoteid;
                catInfoVM.ISARRCExits = _catalogInfoSrv.CheckARRCLevelExitsForTitlesByQuoteID(quoteid);
            }
            this.AssignUserVM(_catalogInfoSrv.UserVM);
            return View("../TCPViews/CatalogInformation", catInfoVM);

        }

        public ActionResult AddCatalogInfoData(FormCollection submit)
        {
            _catalogInfoSrv.UserVM = UserVM;
            int custAutoID = Convert.ToInt32(submit["custAutoID"]);
            CreateCatalogInfoModel CreateCatInfoModel = new CreateCatalogInfoModel();
            Dictionary<string, string> dctSelectedValues = new Dictionary<string, string>();

            //Adding the Selecting Items and text values to teh dictionary.
            string catalogInsertStatus = string.Empty;
            foreach (string key in submit)
            {
                if (key == "CatalogSaveStaus")
                {
                    catalogInsertStatus = "true";
                }
                else
                {
                    if (key != "custAutoID" && key != "txtbox" && key != "quoteID")
                    {
                        if (submit.GetValues(key).ToList().FirstOrDefault() != "false" && submit.GetValues(key).ToList().FirstOrDefault() != "0" && submit.GetValues(key).ToList().FirstOrDefault() != string.Empty)
                        {
                            dctSelectedValues.Add(key, submit.GetValues(key).ToList().FirstOrDefault());
                        }
                    }
                }

            }
            // Creating Customer Catalog Information with the selected Values
            _catalogInfoSrv.CreateCatalogInfo(dctSelectedValues, custAutoID);
            ////Loading the values and showing the values with the selected Data
            //CatalogInfoViewModel catInfovm = _catalogInfoSrv.getCatalogInfoValues(custUserID);
            return RedirectToAction("ViewCatalogInfo", "CatalogInfo", new { custAutoID = custAutoID, catalogInsertStatus = catalogInsertStatus, quoteid = UserVM.CurrentQuoteID });
        }



        [HttpPost]
        public JsonResult getCalculatedPriceBySubOptionIDs(int catalogSubjOptionID, int protectorSubOptionID, int shelfReadySubOptionID, int protectorsCheckedCount)
        {
            string catalogItemPrice = _catalogInfoSrv.calculatePriceForCatalog(catalogSubjOptionID, protectorSubOptionID, shelfReadySubOptionID, protectorsCheckedCount);
            return Json(catalogItemPrice);
        }

        [HttpPost]
        public JsonResult GetVersionValuesBySoftwareID(int softwarevalueID)
        {
            List<ComboBase> cmbVersionValues = _catalogInfoSrv.GetVersionListStringBySoftwareID(softwarevalueID);
            return Json(cmbVersionValues);
        }


    }
}
