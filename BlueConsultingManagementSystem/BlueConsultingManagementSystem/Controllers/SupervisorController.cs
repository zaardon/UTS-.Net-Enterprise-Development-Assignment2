using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueConsultingManagementSystem.Controllers
{
    public class SupervisorController : Controller
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
        public ActionResult ViewRejectedReportInfo()
        {
            return View();
        }
        [HttpGet]
        public ActionResult ViewRejectReports()
        {
            return View();
        }







        //public BCMSModelContainer showExpenses()
        //{
        //    using (var db = new BCMSModelContainer())
        //    {
        //        //foreach (var xp in db.Expenses)
        //        //{
        //        //    Response.Write(xp.Description + " " + xp.Amount);
        //        //}
        //        return db;
        //    }
        //}
	}
}