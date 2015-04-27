using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core;
using TPC.Core.Interfaces;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Web.Filters;

namespace TPC.Web.Controllers.TCPControllers
{
    //[TPCAuthorize(Roles = "AdminRep")]
    [AllowAnonymous]
    public class CollectionController : BaseController
    {
        private readonly IItemContainerService _itemcontainerPartialSrv;
        private readonly IUserPreferenceService _iUserPreferenceSrv;

        public CollectionController(ItemContainerService ItemContainerSrv)
        {
            _itemcontainerPartialSrv = ItemContainerSrv;
            _itemcontainerPartialSrv.UserVM = UserVM;
            _iUserPreferenceSrv = new UserPreferenceService();
        }

        public ActionResult Update()
        {
            CategoryItemGroupModel categoryItemGroupModel = new CategoryItemGroupModel();
            categoryItemGroupModel.lstFilterModel = _itemcontainerPartialSrv.getAllGroups();
            List<PackageModel> packgenames = _itemcontainerPartialSrv.DeleteExistPackage(0);
            ViewBag.PackageNames = packgenames.Select(c => new SelectListItem()
            {
                Text = c.PackageName,
                Value = c.PackageID.ToString(),

            }).ToList();
            categoryItemGroupModel.UserVM = UserVM;
            return View("../TCPViews/UpdateCollection", categoryItemGroupModel);

        }
        public ActionResult UpdateCollectionBygroupID(int groupID)
        {
            FilterModel filterModel = new FilterModel();
            filterModel = _itemcontainerPartialSrv.getCollectionDetailsByID(groupID);
            filterModel.lstpackages = _itemcontainerPartialSrv.getAllpackage(groupID);
            return PartialView("../TCPViews/Partial/UpdateExistingCollectionPartial", filterModel);
        }
        public ActionResult getitemlistByGruopId(int groupID)
        {
            List<KPLBasedCommonViewModel> lstIteList = null;
            if (groupID != 0)
            {
                lstIteList = _itemcontainerPartialSrv.getitemlistByGruopId(groupID);
            }
            ViewBag.grpID = groupID;
            ViewBag.GroupName = _itemcontainerPartialSrv.GetGroupNamebyGroupID(groupID);
            return PartialView("../TCPViews/Partial/GroupItemListPartial", lstIteList);

        }
        [AllowAnonymous]
        public ActionResult AddNewCollections(FilterModel Model, string type)
        {
            if (Model != null && type == Resources.TCPResources.UpdateText)
            {
                if (Model.GroupName != null && Model.GroupName.Trim() != string.Empty)
                {
                    ViewBag.Status = Resources.TCPResources.UpdateText;
                    _itemcontainerPartialSrv.AddNewOrUpdateCollection(Model);
                }
            }
            if (type == Resources.TCPResources.DeleteText)
            {
                _itemcontainerPartialSrv.DeleteCollection(Model.GroupID);

            }
            ModelState.Clear();
            return Update();

        }
        public string AddNewPackage(PackageModel Model)
        {
            if (Model != null && Model.PackageName != null && Model.PackageName.Trim() != string.Empty)
            {
                _itemcontainerPartialSrv.AddNewPackage(Model);
            }
            else { return ""; }
            return Model.PackageName + " Package Added";
        }
        public string updatepackage(FormCollection colection, int groupID)
        {
            var cbids = colection["cbSelect"].Split(',');
            colection.GetValue("cbSelect");
            List<string> lstpackageids = new List<string>();
            foreach (var id in cbids)
            {
                if (id != "false")
                {
                    lstpackageids.Add(id);
                }
            }
            _itemcontainerPartialSrv.updatepackage(lstpackageids, groupID);
            return "Updated Successfully";
        }

        [HttpPost]
        public JsonResult updateCollectionItems(string lstGroupids, string checkedItemids, string unchekedItemids, int groupID)
        {
            List<string> checkedIds = null, unCheckedIds = null, Groupids = null;
            if (checkedItemids != null)
            {
                checkedIds = checkedItemids.Split(',').ToList();
            }
            if (unchekedItemids != null)
            {
                unCheckedIds = unchekedItemids.Split(',').ToList();
            }
            Groupids = lstGroupids != null ? lstGroupids.Split(',').ToList() : null;

            ViewBag.GroupName = _itemcontainerPartialSrv.GetGroupNamebyGroupID(groupID);
            return Json(_itemcontainerPartialSrv.updateCollectionItems(Groupids, checkedIds, unCheckedIds, groupID));
        }



        public ActionResult DeletePackage(FormCollection collection)
        {
            string packageid = collection["PackageNames"];
            int id = Convert.ToInt32(packageid);


            List<PackageModel> packgenames = _itemcontainerPartialSrv.DeleteExistPackage(id);
            PackageModel deletedpackagename = packgenames.Where(e => e.PackageID == id).FirstOrDefault();
            packgenames.Remove(deletedpackagename);
            ViewBag.message = deletedpackagename.PackageName + " " + "Deleted Successfully";
            ViewBag.PackageNames = packgenames.Select(c => new SelectListItem()
            {
                Text = c.PackageName,
                Value = c.PackageID.ToString(),

            }).ToList();
            return PartialView("../TCPViews/Partial/DeletePackagePartial");
        }


        public string UpdateStatus()
        {
            return "Updated Successfully";
        }
    }
}
