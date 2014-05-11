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
        public ActionResult Index()
        {
            showExpenses();
            return View();
        }
        public ActionResult CreateNew()
        {
            new Models.ConsultantModel().TogetherAdd("replace with real values", 13.37, "Rep it");
            ViewBag.Title = "shiiiet";
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