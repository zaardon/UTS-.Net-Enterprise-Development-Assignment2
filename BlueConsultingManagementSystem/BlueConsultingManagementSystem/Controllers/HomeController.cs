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
            return View(showExpenses());
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
        public BCMSModelContainer showExpenses()
        {
            using (var db = new BCMSModelContainer())
            {
                //foreach (var xp in db.Expenses)
                //{
                //    Response.Write(xp.Description + " " + xp.Amount);
                //}
                return db;
            }
        }
    }
}