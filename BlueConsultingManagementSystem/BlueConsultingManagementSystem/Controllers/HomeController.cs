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
            new Models.ConsultantModel().TogetherAdd("plz work", 13.37, "Rep it");
            ViewBag.Title = "shiiiet";
            showExpenses();
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