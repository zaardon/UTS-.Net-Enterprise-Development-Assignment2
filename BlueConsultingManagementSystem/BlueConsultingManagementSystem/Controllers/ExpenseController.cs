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
    public class ExpenseController : Controller
    {
        private BlueConsultingManagementSystemContext db;

        public ExpenseController()
        {
            db = new BlueConsultingManagementSystemContext();
        }

        //
        // GET: /Expense/

        public ActionResult Index()
        {
            return View(db.Expenses.ToList());
        }

        //
        // GET: /Expense/Details/5

        public ActionResult Details(int id)
        {
            //Expense expense = db.Expenses.Find(id);
            //if (expense == null)
            //{
            //    return HttpNotFound();
            //}

            var expense = db.Expenses.Include(x => x.Report).Where(x => x.ExpensePK == id).Single();
            return View(expense);
        }

        //
        // GET: /Expense/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Expense/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Expense expense, int reportPK)
        {
            //if (ModelState.IsValid)
            //{
            //    db.Expenses.Add(expense);
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}

            if (ModelState.IsValid)
            {
                expense.ExpensePK = 0;
                expense.Report = db.Reports.Find(reportPK);
                db.Expenses.Add(expense);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = expense.ExpensePK });
            }
            else
            {
                ViewBag.DepartmentName = staffDirectory.Departments.Find(departmentId).Name;
                SetupDropDownList(ViewBag, employee.Type);
                return View();
            }

            return View(expense);
        }

        //
        // GET: /Expense/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        //
        // POST: /Expense/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Expense expense)
        {
            if (ModelState.IsValid)
            {
                db.Entry(expense).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(expense);
        }

        //
        // GET: /Expense/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Expense expense = db.Expenses.Find(id);
            if (expense == null)
            {
                return HttpNotFound();
            }
            return View(expense);
        }

        //
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
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}