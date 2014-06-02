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
            //Saves the session to be used between page navigation
            if (Session["ReportID"] == null)
            {
                Session["ReportID"] = ReportID;                
            }
            return View();
        }
 
        // POST: /Expense/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExpensePK,Description,Location,Amount,CType,DateOfExpense")] Expense expense, HttpPostedFileBase PdfUpload )
        {
            if (ModelState.IsValid)
            {
                //Adds a PDF to the expense if the supplied PDF value is not null...
                if (PdfUpload != null)
                {
                     string filetype = PdfUpload.ContentType;
                     expense.PDFFile = new byte[PdfUpload.ContentLength];
                     PdfUpload.InputStream.Read(expense.PDFFile, 0, PdfUpload.ContentLength);
                }
                //Otherwise it creates it without one
                expense.Report = db.Reports.Find(Session["ReportID"]);
                db.Expenses.Add(expense);
                db.SaveChanges();
                Report report = db.Reports.Find(Session["ReportID"]);               
                return RedirectToAction("../Report/Details/" + Session["ReportID"]);              
            }
            return RedirectToAction("index");
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
        public FileContentResult PDFView(int? id)
        {
            return File(db.Expenses.Find(id).PDFFile, "application/pdf");
        }
    }
}
