using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueConsultingManagementSystem.Controllers
{
    public class SupervisorController : Controller
    {

        public ActionResult StaffAndSupervisorExpenseView()
        {
            return View();
        }
        public ActionResult StaffAndSupervisorReportView()
        {
            return View();
        }
        public ActionResult TotalExpenses()
        {
            return View();
        }
        public ActionResult ViewPDF()
        {
            return View();
        }
        public ActionResult ViewRejectedReportInfo()
        {
            return View();
        }
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