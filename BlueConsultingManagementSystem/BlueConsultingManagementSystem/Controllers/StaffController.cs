using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueConsultingManagementSystem.Controllers
{
    public class StaffController : Controller
    {
        [HttpGet]
        public ActionResult StaffAndSupervisorExpenseView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult StaffAndSupervisorReportView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult TotalExpenses()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ViewPDF()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ColouredReports()
        {
            return View();
        }

	}
}