using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlueConsultingManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new BCMSModelContainer())
            {
                Expense exp = new Expense { Description = "hotdogs for lunch", Amount = 37.37 };
                db.Expenses.Add(exp);

                Report rp = new Report { ReportName = "the big repofgbfdrt" };
                db.Reports.Add(rp);
                rp.Expenses.Add(exp);

                try
                {
                    db.SaveChanges();
                }
                catch
                {

                }
                showExpenses();
            }
            ViewBag.Title = "it's alive";
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public void showExpenses()
        {
            using (var db = new BCMSModelContainer())
            {
                foreach (var xp in db.Expenses)
                {
                    Response.Write(xp.Description + " " + xp.Amount);
                }

            }
        }
    }
}