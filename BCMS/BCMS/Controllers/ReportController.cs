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
        private readonly double DEFAULT_DEPT_BUDGET = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultDepartmentBudget"]);
        private readonly double DEFAULT_TOTAL_BUDGET = Convert.ToDouble(ConfigurationManager.AppSettings["DefaultTotalBudget"]);
        private readonly DateTime START_OF_THIS_MONTH = DateTime.Today.AddDays(1 - DateTime.Today.Day);
        private BCMSContext db = new BCMSContext();
        private DBLogic DBL = new DBLogic();

        // GET: /Report/
        public ActionResult Index()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.SupervisorApproved == "Submitted").ToList());
        }

        // GET: /Report/Details/5
        public ActionResult Details(int? ReportID)
        {
            if (ReportID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(ReportID);
            if (report == null)
            {
                return HttpNotFound();
            }
            Session["ReportID"] = null;
            ViewBag.TotalReportCost = GetReportCost(ReportID);
            return View(report);
        }

        // GET: /Report/Details/5
        public ActionResult DetailsOnly(int? ReportID)
        {
            if (ReportID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(ReportID);
            if (report == null)
            {
                return HttpNotFound();
            }
            Session["ReportID"] = null;
            ViewBag.TotalReportCost = GetReportCost(ReportID);
            return View(report);
        }
                [Authorize(Roles = "Consultant")]
        // GET: /Report/Create
        public ActionResult Create()
        {          
            return View();
        }

        // POST: /Report/Create
      // this makes  report and posts the id for the redirct 
        [Authorize(Roles = "Consultant")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ReportPK,ReportName,ConsultantName,type,SupervisorName,SupervisorApproved,StaffApproval,DateOfApproval")] Report report)
        {
            if (ModelState.IsValid)
            {
                DBL.AddReport(report, User.Identity.Name.ToString());
                return RedirectToAction("Details", new { ReportID = report.ReportPK });
            }
            return View(report);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET /REPORT/ConsultantSubmissions
        // this is just for user identified submissions
        [Authorize(Roles="Consultant")]
        public ActionResult ConsultantSubmissions()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).ToList());
        }
        // GET  /REPORT/ConsultantApprovals
        // this for consultant user identified submission that have been approved 
        [Authorize(Roles = "Consultant")]
        public ActionResult ConsultantApprovals()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.StaffApproval == "Approved").ToList());
        }

        //GET /REPORT/ConsulantAwaiting 
        // this just displays reports that the consultant submitted that is waiting to be approved.
        [Authorize(Roles = "Consultant")]
        public ActionResult ConsultantAwaiting()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name && ( r.StaffApproval == null && r.SupervisorApproved != "Rejected")).ToList());
        }
        //GET /REPORT/SupervisorReports 
        // gets  reports based on the users departments and if it's been submittied already.
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorReports()
        {
            DepartmentType dept = DeptCheck();

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.SupervisorApproved == "Submitted").ToList());         
        }
        //GET /REPORT/SupervisorRejects
        // gets reports based on department and if it's been staff rejected 
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorRejects()
        {
            DepartmentType dept = DeptCheck();

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.StaffApproval == "Rejected").ToList());
        }
        //GET /REPORT/SupervisorBudgets 
        // this gets the user department and checks 
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorBudget()
        {
            DepartmentType dept = DeptCheck();
            double totalCurrency = 0;
            // this double loop goes through all the reports with that department
            // then gets the amount out.
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.SupervisorApproved == "Approved" && x.StaffApproval != "Rejected").Where(x => x.DateOfApproval >= START_OF_THIS_MONTH || x.DateOfApproval == null)))
            {
                foreach(var expense in report.Expenses)
                {                    
                    totalCurrency = expense.ConvertedAmount + totalCurrency;
                }             
            }  
            //ship it off to the view
            ViewBag.CurrentDepartment = dept;
            ViewBag.TotalExpenses = totalCurrency;
            ViewBag.RemainingBudget = (DEFAULT_DEPT_BUDGET - totalCurrency);
            return View();
        }
        // re useable method for dept checking
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
        //GET /REPORT/StaffBudget
        // displays the staff budget for all departments on the start of the month
        [Authorize(Roles = "Staff")]
        public ActionResult StaffBudget()
        {
            SupervisorList list = new SupervisorList();
            double totalCurrency = 0;
            double supervisorCurrency = 0;
            //Add a 'for this month' part to the where part

            // goes through reports and gets the total amount of converted and adds it to the list to display on the view page
            foreach (var report in (db.Reports.Where(x => x.StaffApproval == "Approved").Where(x => x.DateOfApproval >= START_OF_THIS_MONTH)))
            {
                foreach (var expense in report.Expenses)
                {
                    totalCurrency += expense.ConvertedAmount;
                    supervisorCurrency += expense.ConvertedAmount;
                }
                list.AddToList(report.SupervisorName, supervisorCurrency);
                supervisorCurrency = 0;
            }
            ViewBag.SpentCompanyBudget = totalCurrency;
            ViewBag.RemainingCompanyBudget = DEFAULT_TOTAL_BUDGET - totalCurrency;
            // returns a custom object of a list with 2 columns simple stuff
            return View((object)list.ReturnSupervisors());
        }
        //GET /REPORT/StaffReports
        // this gets the reports for the staff to approve that haven't done so yet
        [Authorize(Roles = "Staff")]
        public ActionResult StaffReports()
        {
            ViewBag.HigherBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.HigherEducation));
            ViewBag.StateBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.State));
            ViewBag.LogisticsBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.Logistics));

            return View(db.Reports.Where(r => r.StaffApproval == null).Where(r => r.SupervisorApproved == "Approved").ToList());
        }
        //GET /REPORT/Approve/id(5 etc)
        // this displays the report to be approved for supervisors 
        [Authorize(Roles = "Supervisor")]
        [HttpGet]
        public ActionResult Approve(int? ReportID)
        {
            // if it's over budget it sends to a confirmation page to be super approved(pun intended)
            if (GetReportCost(ReportID) <= (DEFAULT_DEPT_BUDGET - GetSpentBudgetForSupervisor()))
            {
                return RedirectToAction("ApproveCon", new {reportID = ReportID});
            }
            else
            {
                Report report = db.Reports.Find(ReportID);
                ViewBag.TotalForReport = GetReportCost(ReportID);
                ViewBag.TotalBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForSupervisor());
                return View(report);
            }
        }
        // just a method for getting report cost of particular id nothing crazy 
        private double GetReportCost(int? reportID)
        {
            double amount = 0;
            foreach(Expense exp in db.Reports.Find(reportID).Expenses)
            {
                amount = amount + exp.ConvertedAmount;
            }
            return amount;
        }
        //GET /REPORT/ApproveCon/ReportID(6 etc)
        // just approves a report for supervisor + redirects 
        [Authorize(Roles = "Supervisor")]
        public ActionResult ApproveCon(int? ReportID)
        {
            DBL.SupAppCon(ReportID, User.Identity.Name.ToString());
             return RedirectToAction("SupervisorReports");
        }
        //GET /REPORT/Reject/ReportID(7 etc)
        //  just rejects a report _ redirects
        [Authorize(Roles = "Supervisor")]
         public ActionResult Reject(int? ReportID)
        {
            DBL.SupRej(ReportID, User.Identity.Name.ToString());
            return RedirectToAction("SupervisorReports");
        }
        // this returns the spent budget for a supervisor in a department
        private double GetSpentBudgetForSupervisor()
        {
            DepartmentType dept = DeptCheck();
            double totalCurrency = 0;
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.SupervisorApproved == "Approved" && x.StaffApproval != "Rejected").Where(x => x.DateOfApproval >= START_OF_THIS_MONTH || x.DateOfApproval == null)))
            {
                foreach (var expense in report.Expenses)
                {
                    totalCurrency += expense.ConvertedAmount;
                }
            }
            return totalCurrency;
        }
        //GET /REPORT/StaffApproval
        // this is similar to the supervis method 
       // you get the report id you want to approve and check if it's over budget and then process it accordingly 
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult StaffApproval(int? ReportID)
        {
            if (GetReportCost(ReportID) <= (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(db.Reports.Find(ReportID).type)))
            {
                return RedirectToAction("StaffApprovalCon", new { ReportID = ReportID });
            }
            else
            {
                Report report = db.Reports.Find(ReportID);
                ViewBag.TotalForReport = GetReportCost(ReportID);
                ViewBag.TotalBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(db.Reports.Find(ReportID).type));
                return View(report);
            }
        }
        // this returns the staff budget for a dept
        [Authorize(Roles = "Staff")]
        private double GetSpentBudgetForStaff(DepartmentType dept)
        {
            double totalCurrency = 0;
            foreach (var report in (db.Reports.Where(x => x.type == dept).Where(x => x.StaffApproval == "Approved").Where(x => x.DateOfApproval >= START_OF_THIS_MONTH)))
            {
                foreach (var expense in report.Expenses)
                {
                    totalCurrency += expense.ConvertedAmount;
                }
            }
            return totalCurrency;
        }
        //GET /REPORT/StaffApprovalCon/ReportID(278 etc)
        // this approves a report on staff  level and redirects 
        [Authorize(Roles = "Staff")]
        public ActionResult StaffApprovalCon(int? ReportID)
        {
            DBL.StaffAppCon(ReportID);
            return RedirectToAction("StaffReports");
        }
        //GET /REPORT/StaffReject/ReportID(1337 , 9001, etc)
        // this rejects on staff level and redirects 
        [Authorize(Roles = "Staff")]
        public ActionResult StaffReject(int? ReportID)
        {
            DBL.StaffRej(ReportID);
            return RedirectToAction("StaffReports");
        }
    }
}
