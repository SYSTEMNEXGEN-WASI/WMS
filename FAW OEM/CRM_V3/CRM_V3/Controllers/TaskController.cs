using Core.CRM.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRM_V3.Controllers
{
    public class TaskController : Controller
    {
        //
        // GET: /Task/
        public ActionResult TaskDetail()
        {
            List<SelectListItem> ddleadId = new List<SelectListItem>();
            ddleadId = GeneralMethods.GetLeadId();
            ViewBag.LeadId = ddleadId;

            List<SelectListItem> ddlAssignTo = new List<SelectListItem>();
            ddlAssignTo = GeneralMethods.GetDealerEmployee();
            ViewBag.AssignTo = ddlAssignTo;

            List<SelectListItem> ddlTaskId = new List<SelectListItem>();
            ddlTaskId = GeneralMethods.GetTaskId();
            ViewBag.Tasks = ddlTaskId;

            List<SelectListItem> ddlStatusType = new List<SelectListItem>();
            ddlStatusType = GeneralMethods.GetStatusType();
            ViewBag.StatusType = ddlStatusType;

            List<SelectListItem> ddlSubjects = new List<SelectListItem>();
            ddlSubjects = GeneralMethods.GetSubjects();
            ViewBag.Subjects = ddlSubjects;

            List<SelectListItem> ddlCustomers = new List<SelectListItem>();
            ddlCustomers = GeneralMethods.GetCustomers();
            ViewBag.Customers = ddlCustomers;

            List<SelectListItem> ddlRelatedTo = new List<SelectListItem>();
            ddlRelatedTo = GeneralMethods.GetLeadSourceID();
            ViewBag.RelatedTo = ddlRelatedTo;

            List<SelectListItem> ddlTaskType = new List<SelectListItem>();
            ddlTaskType = GeneralMethods.GetTaskType();
            ViewBag.TaskType = ddlTaskType;

            List<SelectListItem> ddlPriority = new List<SelectListItem>();
            ddlPriority = GeneralMethods.GetStatusType();
            ViewBag.Priority = ddlPriority;

            Session["userId"] = "001";

            return View();
        }
	}
}