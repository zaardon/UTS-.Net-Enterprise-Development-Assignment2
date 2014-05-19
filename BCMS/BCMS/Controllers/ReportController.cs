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
    public class ReportController : Controller
    {
        private BCMSContext db = new BCMSContext();

        // GET: /Report/
        public ActionResult Index()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.SupervisorApproved == "Submitted").ToList());
            //return View(db.Reports.ToList());
        }

        // GET: /Report/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: /Report/Details/5
        public ActionResult DetailsOnly(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: /Report/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: /Report/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ReportPK,ReportName,ConsultantName,type,SupervisorApproved,StaffApproval,DateOfApproval")] Report report)
        {
            if (ModelState.IsValid)
            {
                report.ConsultantName = User.Identity.Name;
                report.SupervisorApproved = "Submitted";
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        // GET: /Report/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: /Report/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ReportPK,ReportName,ConsultantName,type,SupervisorApproved,StaffApproval,DateOfApproval")] Report report)
        {
            if (ModelState.IsValid)
            {
                db.Entry(report).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: /Report/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

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
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }


        public ActionResult ConsultantSubmissions()
        {

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).ToList());
        }

        public ActionResult ConsultantApprovals()
        {

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.StaffApproval == "Approved").ToList());
        }

        public ActionResult ConsultantAwaiting()
        {

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.SupervisorApproved == "Submitted").ToList());
        }

        //for supervisor/staff
        public ActionResult SupervisorReports()
        {
            DepartmentType dept = DepartmentType.HigherEducation;
            if(User.IsInRole("HigherEducation"))
            {
                dept = DepartmentType.HigherEducation;
            }
            else if(User.IsInRole("Logistic"))
            {
                dept = DepartmentType.Logistics;
            }
            else if(User.IsInRole("State"))
            {
                dept = DepartmentType.State;
            }

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.SupervisorApproved == "Submitted").ToList());
           
        }



        public ActionResult SupervisorRejects()
        {
            DepartmentType dept = DepartmentType.HigherEducation;
            if (User.IsInRole("HigherEducation"))
            {
                dept = DepartmentType.HigherEducation;
            }
            else if (User.IsInRole("Logistic"))
            {
                dept = DepartmentType.Logistics;
            }
            else if (User.IsInRole("State"))
            {
                dept = DepartmentType.State;
            }

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.StaffApproval == "Rejected").ToList());
        }

        public ActionResult SupervisorBudget()
        {
            //NOT WORKING YET
            DepartmentType dept = DepartmentType.HigherEducation;
            if (User.IsInRole("HigherEducation"))
            {
                dept = DepartmentType.HigherEducation;
            }
            else if (User.IsInRole("Logistic"))
            {
                dept = DepartmentType.Logistics;
            }
            else if (User.IsInRole("State"))
            {
                dept = DepartmentType.State;
            }
            double totalCurrency = 0;
            //Add a 'for this month' part to the where part
           foreach(var report in (db.Reports.Where(x => x.type == dept)))
           {
               foreach(var expense in report.Expenses)
               {
                   totalCurrency = expense.Amount + totalCurrency;
               }             
           }
           string budgetMessage = dept + " department expenses are: " + totalCurrency +"\n" + dept + " department budget remaning is:" + (10000.00 - totalCurrency);
           return View((object)budgetMessage);
        }
    

        public ActionResult StaffBudget()
        {
            //NOT DONE YET
            return View();

        }

        public ActionResult StaffReports()
        {
            //I think this is for that colour thing
            return View(db.Reports.Where(r => r.StaffApproval == "").ToList());
        }
        [HttpGet]
        public ActionResult Approve(int? id)
        {
<<<<<<< HEAD
            db.Reports.Find(id).SupervisorName = User.Identity.Name;
             db.Reports.Find(id).SupervisorApproved = "Approved";
=======
            Report report = db.Reports.Find(id);
            return View(report);
        }
        
        public ActionResult ApproveCon(int? ReportID)
        {
             db.Reports.Find(ReportID).SupervisorApproved = "Approved";
>>>>>>> 54c67d22a819ef996150cd027669bf3461f8c174
             db.SaveChanges();
             return RedirectToAction("SupervisorReports");
        }

        public void budgetcheck()
        {
            //find all reports with Department = HIGHEReducation (1)
            //get all approved(supervisor level) total cost (2)
            // if that (1) + (2) is > 10,000 (webconfig shit)
            // provide warning --- know how to do
            //DepartmentType dept = DepartmentType.HigherEducation;
            //if (User.IsInRole("HigherEducation"))
            //{
            //    dept = DepartmentType.HigherEducation;
            //}
            //else if (User.IsInRole("Logistic"))
            //{
            //    dept = DepartmentType.Logistics;
            //}
            //else if (User.IsInRole("State"))
            //{
            //    dept = DepartmentType.State;
            //}
            //db.Reports.Where(x=> x.type == dept).Sum(x=> x.Expenses.Find(e=>e.Amount))
        }
    }
}
