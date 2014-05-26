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

            ViewBag.TotalReportCost = GetReportCost(id);
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
            ViewBag.TotalReportCost = GetReportCost(id);
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
        public ActionResult Create([Bind(Include = "ReportPK,ReportName,ConsultantName,type,SupervisorName,SupervisorApproved,StaffApproval,DateOfApproval")] Report report)
        {
            if (ModelState.IsValid)
            {
                report.ConsultantName = User.Identity.Name;
                report.SupervisorApproved = "Submitted";
                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = report.ReportPK });
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

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name && ( r.StaffApproval == null && r.SupervisorApproved != "Rejected")).ToList());
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
              foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.SupervisorApproved == "Approved" && x.StaffApproval != "Rejected")))
             {
                   foreach(var expense in report.Expenses)
                 {
                     
                     totalCurrency = expense.ConvertedAmount + totalCurrency;
                 }             
            }
            string budgetMessage = dept + " department expenses are: $" + totalCurrency +", with a remaining " + dept + " department budget of: $" + (10000.00 - totalCurrency);
           
            return View((object)budgetMessage);
        }
    

        public ActionResult StaffBudget()
        {
            SupervisorList list = new SupervisorList();
            double totalCurrency = 0;
            double supervisorCurrency = 0;
            //Add a 'for this month' part to the where part
            foreach (var report in (db.Reports.Where(x => x.StaffApproval == "Approved")))
            {
                foreach (var expense in report.Expenses)
                {
                    totalCurrency = expense.ConvertedAmount + totalCurrency;
                    supervisorCurrency = expense.ConvertedAmount + supervisorCurrency;
                }
                list.AddToList(report.SupervisorName, supervisorCurrency);
                supervisorCurrency = 0;
            }
            ViewBag.SpentCompanyBudget = totalCurrency;
            ViewBag.RemainingCompanyBudget = 30000.00 - totalCurrency;

            return View((object)list.ReturnSupervisors());

        }

        public ActionResult StaffReports()
        {
            //I think this is for that colour thing
            ViewBag.HigherBudgetRemaining = (10000 - GetSpentBudgetForStaff(DepartmentType.HigherEducation));
            ViewBag.StateBudgetRemaining = (10000 - GetSpentBudgetForStaff(DepartmentType.State));
            ViewBag.LogisticsBudgetRemaining = (10000 - GetSpentBudgetForStaff(DepartmentType.Logistics));
            return View(db.Reports.Where(r => r.StaffApproval == null).Where(r => r.SupervisorApproved == "Approved").ToList());

        }
        [HttpGet]
        public ActionResult Approve(int? id)
        {
            if (GetReportCost(id) <= (10000 - GetSpentBudgetForSupervisor()))
            {
                //db.Reports.Find(id).SupervisorName = User.Identity.Name;
                //db.Reports.Find(id).SupervisorApproved = "Approved";
                return RedirectToAction("ApproveCon", new {reportID = id});
            }
            else
            {
                Report report = db.Reports.Find(id);
                ViewBag.TotalForReport = GetReportCost(id);
                ViewBag.TotalBudgetRemaining = (10000 - GetSpentBudgetForSupervisor());
                return View(report);
            }

        }

        public double GetReportCost(int? reportID)
        {
            double amount = 0;
            foreach(Expense exp in db.Reports.Find(reportID).Expenses)
            {
                amount = amount + exp.ConvertedAmount;
            }

            return amount;
        }
        
        public ActionResult ApproveCon(int? ReportID)
        {
            db.Reports.Find(ReportID).SupervisorName = User.Identity.Name;
            db.Reports.Find(ReportID).SupervisorApproved = "Approved";
             db.SaveChanges();
             return RedirectToAction("SupervisorReports");
        }
         public ActionResult Reject(int? id)
        {
            db.Reports.Find(id).SupervisorName = User.Identity.Name;
            db.Reports.Find(id).SupervisorApproved = "Rejected";
            db.SaveChanges();
            return RedirectToAction("SupervisorReports");
        }


        public double GetSpentBudgetForSupervisor()
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
            double totalCurrency = 0;
            //Add a 'for this month' part to the where part
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.SupervisorApproved == "Approved" && x.StaffApproval != "Rejected")))
            {
                foreach (var expense in report.Expenses)
                {
                    totalCurrency = expense.ConvertedAmount + totalCurrency;
                }
            }
            return totalCurrency;
        }

        [HttpGet]
        public ActionResult StaffApproval(int? id)
        {
            if (GetReportCost(id) <= (10000 - GetSpentBudgetForStaff(db.Reports.Find(id).type)))
            {
                //db.Reports.Find(id).SupervisorName = User.Identity.Name;
                //db.Reports.Find(id).SupervisorApproved = "Approved";
                return RedirectToAction("StaffApprovalCon", new { id = id });
            }
            else
            {
                Report report = db.Reports.Find(id);
                ViewBag.TotalForReport = GetReportCost(id);
                ViewBag.TotalBudgetRemaining = (10000 - GetSpentBudgetForStaff(db.Reports.Find(id).type));
                return View(report);
            }
        }

        public double GetSpentBudgetForStaff(DepartmentType dept)
        {
            double totalCurrency = 0;
            //Add a 'for this month' part to the where part
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.StaffApproval == "Approved")))
            {
                foreach (var expense in report.Expenses)
                {
                    totalCurrency = expense.ConvertedAmount + totalCurrency;
                }
            }
            return totalCurrency;
        }
        public ActionResult StaffApprovalCon(int? id)
        {
            db.Reports.Find(id).StaffApproval = "Approved";
            db.Reports.Find(id).DateOfApproval = DateTime.Now.Date;
            db.SaveChanges();
            return RedirectToAction("StaffReports");
        }
        public ActionResult StaffReject(int? id)
        {
            db.Reports.Find(id).StaffApproval = "Rejected";
            db.Reports.Find(id).DateOfApproval = DateTime.Now.Date;
            db.SaveChanges();
            return RedirectToAction("StaffReports");
        }

    }
}
