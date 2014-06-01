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
using System.Configuration;

namespace BCMS.Controllers
{
    public class ReportController : Controller
    {
        readonly double DEFAULT_DEPT_BUDGET = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultDepartmentBudget"]);
        readonly double DEFAULT_TOTAL_BUDGET = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultTotalBudget"]);
        private BCMSContext db = new BCMSContext();
        private DBLogic DBL = new DBLogic();
        // GET: /Report/
        public ActionResult Index()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.SupervisorApproved == "Submitted").ToList());
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
            Session["ReportID"] = null;
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
            Session["ReportID"] = null;
            ViewBag.TotalReportCost = GetReportCost(id);
            return View(report);
        }
                [Authorize(Roles = "Consultant")]
        // GET: /Report/Create
        public ActionResult Create()
        {
            
            return View();
        }

        // POST: /Report/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Consultant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportPK,ReportName,ConsultantName,type,SupervisorName,SupervisorApproved,StaffApproval,DateOfApproval")] Report report)
        {
            if (ModelState.IsValid)
            {
                DBL.AddReport(report, User.Identity.Name.ToString());
                return RedirectToAction("Details", new { id = report.ReportPK });
            }

            return View(report);
        }



        //wtf is this ?
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [Authorize(Roles="Consultant")]
        public ActionResult ConsultantSubmissions()
        {

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).ToList());
        }

        [Authorize(Roles = "Consultant")]
        public ActionResult ConsultantApprovals()
        {

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.StaffApproval == "Approved").ToList());
        }
        [Authorize(Roles = "Consultant")]
        public ActionResult ConsultantAwaiting()
        {

            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name && ( r.StaffApproval == null && r.SupervisorApproved != "Rejected")).ToList());
        }

        //for supervisor/staff
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorReports()
        {
            DepartmentType dept = DeptCheck();

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.SupervisorApproved == "Submitted").ToList());         
        }
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorRejects()
        {
            DepartmentType dept = DeptCheck();

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.StaffApproval == "Rejected").ToList());
        }
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorBudget()
        {
            DepartmentType dept = DeptCheck();
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

        private DepartmentType DeptCheck()
        {
            DepartmentType dept = DepartmentType.HigherEducation;
            if (User.IsInRole("HigherEducation"))
            {
                dept = DepartmentType.HigherEducation;
            }
            else if (User.IsInRole("Logistics"))
            {
                dept = DepartmentType.Logistics;
            }
            else if (User.IsInRole("State"))
            {
                dept = DepartmentType.State;
            }
            return dept;
        }

        [Authorize(Roles = "Staff")]
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
                    //totalCurrency = expense.ConvertedAmount + totalCurrency;
                    totalCurrency += expense.ConvertedAmount;
                    //supervisorCurrency = expense.ConvertedAmount + supervisorCurrency;
                    supervisorCurrency += expense.ConvertedAmount;
                }
                list.AddToList(report.SupervisorName, supervisorCurrency);
                supervisorCurrency = 0;
            }
            ViewBag.SpentCompanyBudget = totalCurrency;
            ViewBag.RemainingCompanyBudget = DEFAULT_TOTAL_BUDGET - totalCurrency;
            return View((object)list.ReturnSupervisors());
        }
        [Authorize(Roles = "Staff")]
        public ActionResult StaffReports()
        {
            //double defaultBudget = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultDepartmentBudget"]);
            //I think this is for that colour thing
            ViewBag.HigherBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.HigherEducation));
            ViewBag.StateBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.State));
            ViewBag.LogisticsBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.Logistics));

            return View(db.Reports.Where(r => r.StaffApproval == null).Where(r => r.SupervisorApproved == "Approved").ToList());
        }
        [Authorize(Roles = "Supervisor")]
        [HttpGet]
        public ActionResult Approve(int? id)
        {
            if (GetReportCost(id) <= (DEFAULT_DEPT_BUDGET - GetSpentBudgetForSupervisor()))
            {
                //db.Reports.Find(id).SupervisorName = User.Identity.Name;
                //db.Reports.Find(id).SupervisorApproved = "Approved";
                return RedirectToAction("ApproveCon", new {reportID = id});
            }
            else
            {
                Report report = db.Reports.Find(id);
                ViewBag.TotalForReport = GetReportCost(id);
                ViewBag.TotalBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForSupervisor());
                return View(report);
            }
        }

        private double GetReportCost(int? reportID)
        {
            double amount = 0;
            foreach(Expense exp in db.Reports.Find(reportID).Expenses)
            {
                amount = amount + exp.ConvertedAmount;
            }

            return amount;
        }
        [Authorize(Roles = "Supervisor")]
        public ActionResult ApproveCon(int? ReportID)
        {
            DBL.SupAppCon(ReportID, User.Identity.Name.ToString());
             return RedirectToAction("SupervisorReports");
        }
        [Authorize(Roles = "Supervisor")]
         public ActionResult Reject(int? id)
        {
            DBL.SupRej(id, User.Identity.Name.ToString());
            return RedirectToAction("SupervisorReports");
        }

        private double GetSpentBudgetForSupervisor()
        {
            DepartmentType dept = DeptCheck();
            double totalCurrency = 0;
            //Add a 'for this month' part to the where part
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.SupervisorApproved == "Approved" && x.StaffApproval != "Rejected")))
            {
                foreach (var expense in report.Expenses)
                {
                    //totalCurrency = expense.ConvertedAmount + totalCurrency;
                    totalCurrency += expense.ConvertedAmount;
                }
            }
            return totalCurrency;
        }

        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult StaffApproval(int? id)
        {
            if (GetReportCost(id) <= (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(db.Reports.Find(id).type)))
            {
                //db.Reports.Find(id).SupervisorName = User.Identity.Name;
                //db.Reports.Find(id).SupervisorApproved = "Approved";
                return RedirectToAction("StaffApprovalCon", new { id = id });
            }
            else
            {
                Report report = db.Reports.Find(id);
                ViewBag.TotalForReport = GetReportCost(id);
                ViewBag.TotalBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(db.Reports.Find(id).type));
                return View(report);
            }
        }

        [Authorize(Roles = "Staff")]
        private double GetSpentBudgetForStaff(DepartmentType dept)
        {
            double totalCurrency = 0;
            //Add a 'for this month' part to the where part
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.StaffApproval == "Approved")))
            {
                foreach (var expense in report.Expenses)
                {
                    //totalCurrency = expense.ConvertedAmount + totalCurrency;
                    totalCurrency += expense.ConvertedAmount;
                }
            }
            return totalCurrency;
        }

        [Authorize(Roles = "Staff")]
        public ActionResult StaffApprovalCon(int? id)
        {
            DBL.StaffAppCon(id);
            return RedirectToAction("StaffReports");
        }

        [Authorize(Roles = "Staff")]
        public ActionResult StaffReject(int? id)
        {
            DBL.StaffRej(id);
            return RedirectToAction("StaffReports");
        }

    }
}
