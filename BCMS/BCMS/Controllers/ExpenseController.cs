using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BlueConsultingManagementSystem.Models;
using BCMS.Models;

namespace BCMS.Controllers
{
    public class ExpenseController : Controller
    {
        private BCMSContext db;

        // GET: /Expense/
        public ActionResult Index()
        {
            return View(db.Expenses.ToList());
        }
        public ExpenseController()
        {
            db = new BCMSContext();
        }

        // GET: /Expense/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var expense = db.Expenses.Include(x => x.Report).Where(x => x.ExpensePK == id).Single();
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

// GET: /Expense/Create
        public ActionResult Create(int ReportID)
        {
            ViewBag.ReportName = db.Reports.Find(ReportID).ReportName;

            if (Session["ReportID"] == null)
            {
                Session["ReportID"] = ReportID;
            }
            return View();
        }
 
        // POST: /Expense/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpensePK,Description,Location,Amount,CType,DateOfExpense")] Expense expense, HttpPostedFileBase PdfUpload )
        {
            if (ModelState.IsValid)
            {
                if (PdfUpload != null)
                {

                        // file stuff
                        string filetype = PdfUpload.ContentType;
                        expense.PDFFile = new byte[PdfUpload.ContentLength];
                        PdfUpload.InputStream.Read(expense.PDFFile, 0, PdfUpload.ContentLength);
                        // more processing


                }
                expense.Report = db.Reports.Find(Session["ReportID"]);

                db.Expenses.Add(expense);
                db.SaveChanges();

                Report report = db.Reports.Find(Session["ReportID"]);
                return RedirectToAction("../Report/Details/" + Session["ReportID"]);
                
            }
            return RedirectToAction("index");
        }

        // GET: /Expense/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: /Expense/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ExpensePK,Description,Location,Amount,CType,DateOfExpense")] Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        // GET: /Expense/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        // POST: /Expense/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Expense expense = db.Expenses.Find(id);
            db.Expenses.Remove(expense);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpGet]
        public ActionResult ExpenseView(int ReportID)
        {
            return View(db.Expenses.ToList().Where(x => x.Report.ReportPK == ReportID));
        }
        public FileContentResult  PDFView(int? id)
        {
                return File(db.Expenses.Find(id).PDFFile, "application/pdf");
        }
    }
}
