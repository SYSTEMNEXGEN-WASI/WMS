using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class MasterController : Controller
    {
        // GET: Master
        public ActionResult MasterSetup()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Select_VehicleDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_VehicleTypeData();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_ReceiptDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_VehRecTypeData();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_DocumentDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_DocumentTypeData();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Select_LocationDetail()
        {
            string data = "";
            bool result = false;
            data = MasterMethods.Get_LocationDetail();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_VehicleType(VehicleTypeVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_VehicleType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_VehicleReceiptType(VehReceiptTypoVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_VehReceiptType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_DocumentType(DocumentCheckList objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_DocumentType(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Insert_VehLocation(VehicleLocationVM objects)
        {
            bool result = false;

            string msg = "Failed to save record..";

            result = MasterMethods.Insert_VehLocation(objects);

            if (result)
            {
                msg = "Successfully Added";
            }

            return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
        }
    }
}