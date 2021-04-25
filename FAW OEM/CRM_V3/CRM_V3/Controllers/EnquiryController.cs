using Core.CRM.ADO;
using Core.CRM.ADO.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CRM_V3.Controllers
{
	public class EnquiryController : Controller
	{
		//
		// GET: /Enquiry/


        
        public ActionResult Alert()
        {
            return View();
        }

		public ActionResult QuickEnquiry()
		{
			List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = GeneralMethods.GetLeadId();
			ViewBag.AssignTo = ddlAssignTo;

			List<SelectListItem> ddlProdCode = new List<SelectListItem>();
			ddlProdCode = GeneralMethods.GetProduct();
			ViewBag.ProdCode = ddlProdCode;

			List<SelectListItem> ddlColor = new List<SelectListItem>();
			ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
			ViewBag.Color = ddlColor;

			Session["userId"] = "001";

			return View();
		}

		public ActionResult EnquiryDetail(string leadId = "")
		{
          
			List<SelectListItem> ddleadId = new List<SelectListItem>();
			ddleadId = GeneralMethods.GetLeadId();
			ViewBag.LeadId = ddleadId;

			List<SelectListItem> ddlLeadType = new List<SelectListItem>();
			ddlLeadType = GeneralMethods.GetLeadType();
			ViewBag.LeadType = ddlLeadType;

			List<SelectListItem> ddlLeadMode = new List<SelectListItem>();
			ddlLeadMode = GeneralMethods.GetLeadMode();
			ViewBag.LeadMode = ddlLeadMode;

			List<SelectListItem> ddlLeadSrouce = new List<SelectListItem>();
			ddlLeadSrouce = GeneralMethods.GetLeadSource();
			ViewBag.LeadSource = ddlLeadSrouce;

			List<SelectListItem> ddlVehicelSegments = new List<SelectListItem>();
			ddlVehicelSegments = GeneralMethods.GetVehicleSegments();
			ViewBag.VehicleSegments = ddlVehicelSegments;

			List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
			ddlAssignTo = GeneralMethods.GetDealerEmployee();
			ViewBag.AssignTo = ddlAssignTo;

			List<SelectListItem> ddlBank = new List<SelectListItem>();
			ddlBank = GeneralMethods.GetBank();
			ViewBag.Bank = ddlBank;

			List<SelectListItem> ddlCustomers = new List<SelectListItem>();
			ddlCustomers = GeneralMethods.GetCustomers();
			ViewBag.Customers = ddlCustomers;
			
			List<SelectListItem> ddlCountry = new List<SelectListItem>();
			ddlCountry = GeneralMethods.GetCountry();
			ViewBag.Country = ddlCountry;

			List<SelectListItem> ddlCity = new List<SelectListItem>();
			ddlCity = GeneralMethods.GetCity();
			ViewBag.City = ddlCity;

			List<SelectListItem> ddlBrandCode = new List<SelectListItem>();
			ddlBrandCode = GeneralMethods.GetBrands();
			ViewBag.BrandCode = ddlBrandCode;

			List<SelectListItem> ddlProdCode = new List<SelectListItem>();
			ddlProdCode = GeneralMethods.GetProduct();
			ViewBag.ProdCode = ddlProdCode;

			List<SelectListItem> ddlColor = new List<SelectListItem>();
			ddlColor = GeneralMethods.GetColor(Session["VehicleCategory"].ToString());
			ViewBag.Color = ddlColor;

			List<SelectListItem> ddlPriority = new List<SelectListItem>();
			ddlPriority = GeneralMethods.GetStatus();
			ViewBag.Priority = ddlPriority;

			List<SelectListItem> ddlFurtherContact = new List<SelectListItem>();
			ddlFurtherContact = GeneralMethods.GetTaskType();
			ViewBag.FurtherContact = ddlFurtherContact;

            ViewBag.UrlLeadId = leadId;

			return View();
		}

		public JsonResult Insert_EnquiryMaster(EnquiryMasterVM requestModel)
		{
			bool result = false;
			string msg = "Failed to save record..";
			string leadId = "", prospectid = "";
			string[] data;
			leadId = EnquiryMethods.Insert_EnquiryMaster(requestModel);
			if(!string.IsNullOrEmpty(leadId))
			{
				data = leadId.Split(',');
				leadId = data[0].ToString();
				prospectid = data[1].ToString();
				result = true;
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg, LeadId = leadId, ProspectId = prospectid }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult Insert_Prospect(ProspectDetailVM requestModel)
		{
			bool result = false;
			string msg = "Failed to save record..";
			result = EnquiryMethods.Insert_ProspectDetail(requestModel);
			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

		[HttpPost]
		public JsonResult Insert_EnquiryDetail(List<EnquiryDetailVM> objects)
		{
			bool result = false;
			int count = 0;
			string msg = "Failed to save record..";
			foreach (var item in objects)
			{
				if(count >= 1)
				{
					result = EnquiryMethods.Insert_EnquiryDetail(item);
				}
				count++;
			}
			
			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

		public JsonResult Insert_QuickEnquiry(QuickEnquiryVM requestModel)
		{
			bool result = false;
			string msg = "Failed to save record..";
			result = EnquiryMethods.Insert_QuickEnquiry(requestModel);
			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult Select_EnquiryMaster(string EnquiryId, string DealerCode)
		{
			string data = "";
			bool result = false;
			data = EnquiryMethods.Get_EnquiryMasterData(EnquiryId, DealerCode);

			if (!string.IsNullOrEmpty(data))
			{
				result = true;
			}

			return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult Select_EnquiryDetail(string EnquiryId, string DealerCode)
		{
			string data = "";
			bool result = false;
			data = EnquiryMethods.Get_EnquiryDetailData(EnquiryId, DealerCode);

			if (!string.IsNullOrEmpty(data))
			{
				result = true;
			}

			return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
		}

		public ActionResult FollowUp()
		{
			List<SelectListItem> ddleadId = new List<SelectListItem>();
			ddleadId = GeneralMethods.GetLeadId();
			ViewBag.LeadId = ddleadId;

			List<SelectListItem> ddlLeadType = new List<SelectListItem>();
			ddlLeadType = GeneralMethods.GetLeadType();
			ViewBag.LeadType = ddlLeadType;

			List<SelectListItem> ddlFurtherContact = new List<SelectListItem>();
			ddlFurtherContact = GeneralMethods.GetTaskType();
			ViewBag.FurtherContact = ddlFurtherContact;

			List<SelectListItem> ddlStatusType = new List<SelectListItem>();
			ddlStatusType = GeneralMethods.GetStatusType();
			ViewBag.StatusType = ddlStatusType;

			List<SelectListItem> ddlLostReason = new List<SelectListItem>();
			ddlLostReason = GeneralMethods.GetLostReason();
			ViewBag.LostReason = ddlLostReason;


			return View();
		}

		[HttpPost]
		public JsonResult Insert_FollowupDetail(List<FollowUpDetailVM> objects)
		{
			bool result = false;
			//int count = 0;
			string msg = "Failed to save record..";
			foreach (var item in objects)
			{
				//if (count >= 1)
				//{
					result = EnquiryMethods.Insert_FollowupDetail(item);
				//}
				//count++;
			}

			if (result)
			{
				msg = "Successfully Added";
			}

			return Json(new { Success = result, Message = msg }, JsonRequestBehavior.AllowGet);
		}

		[HttpGet]
		public JsonResult Select_FollowUpDetail(string EnquiryId)
		{
			string data = "";
			bool result = false;
			data = EnquiryMethods.Get_FollowUpDetailData(EnquiryId);

			if (!string.IsNullOrEmpty(data))
			{
				result = true;
			}

			return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
		}

        public JsonResult Select_AlertEnquiryData()
        {
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_AlertEnquiryDetails();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Select_AlertFollowUpData()
        {
            string data = "";
            bool result = false;
            data = EnquiryMethods.Get_AlertFollowUpDetails();

            if (!string.IsNullOrEmpty(data))
            {
                result = true;
            }

            return Json(new { Success = result, Response = data }, JsonRequestBehavior.AllowGet);
        }
	}
}