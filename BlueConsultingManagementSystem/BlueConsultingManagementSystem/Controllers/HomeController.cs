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
            ViewBag.ConsultantReports = showExpenses("currentUser");
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
        public List<object> showExpenses(string user)
        {
            List<object> ConsultantReportsMade = new List<object>();
            using (var db = new BCMSModelContainer())
            {
                foreach (var xp in db.Reports)
                {
                    if(xp.ConsultantName == user)
                    ConsultantReportsMade.Add(xp);
                }
                return ConsultantReportsMade;
            }
        }
    }
}