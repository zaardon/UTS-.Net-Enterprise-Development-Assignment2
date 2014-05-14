using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlueConsultingManagementSystem.Models;

namespace BlueConsultingManagementSystem.Controllers
{
    public class ReportController : Controller
    {
        private BlueConsultingManagementSystemContext db = new BCMSModelContainer();

        //
        // GET: /Report/

        public ActionResult Index()
        {
            return View(db.Reports.ToList());
        }

        //
        // GET: /Report/Details/5

        public ActionResult Details(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        //
        // GET: /Report/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Report/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Report report)
        {
            if (ModelState.IsValid)
            {
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        //
        // GET: /Report/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        //
        // POST: /Report/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        //
        // GET: /Report/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        //
        // POST: /Report/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
            
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}