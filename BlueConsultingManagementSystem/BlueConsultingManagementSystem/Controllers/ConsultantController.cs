using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueConsultingManagementSystem.Controllers
{
    public class ConsultantController : Controller
    {
        //
        // GET: /Consultant/
        [HttpGet]
        public ActionResult ViewExpense(string reportName)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ViewExpense(object expense)
        {
            return View();
        }
        [HttpGet]
        public ActionResult ViewReport(string consultantName)
        {
            return View();
        }
        [HttpPost]
        public ActionResult ViewReport(object expense)
        {
            return View();
        }
        [HttpGet]
        public ActionResult ViewPDF()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddExpenseView(object obj)
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult AddReportView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Main()
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