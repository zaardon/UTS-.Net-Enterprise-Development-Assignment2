﻿using System;
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
        public ActionResult ViewExpense()
        {
            return View();
        }
        public ActionResult ViewReport()
        {
            return View();
        }
        public ActionResult ViewPDF()
        {
            return View();
        }
        public ActionResult AddExpenseView()
        {
            return View();
        }
        public ActionResult AddReportView()
        {
            return View();
        }
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