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
        //This provides an expense details view that allows an consultant to add more expenses
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
        //This provides an expense details view that does not allow the user to add more expenses
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
        //This creates a report and posts the ID for the redirct 
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET /REPORT/ConsultantSubmissions
        //This provides a list of user identified consultant submissions that have been submitted (a complete history)
        [Authorize(Roles="Consultant")]
        public ActionResult ConsultantSubmissions()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).ToList());
        }
        // GET  /REPORT/ConsultantApprovals
        //This provides a list of user identified consultant submissions that have been approved
        [Authorize(Roles = "Consultant")]
        public ActionResult ConsultantApprovals()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name).Where(r => r.StaffApproval == "Approved").ToList());
        }

        //GET /REPORT/ConsulantAwaiting 
        //This provides a list of user identified consultant submissions that are pending to be reviewed
        [Authorize(Roles = "Consultant")]
        public ActionResult ConsultantAwaiting()
        {
            return View(db.Reports.Where(r => r.ConsultantName == User.Identity.Name && ( r.StaffApproval == null && r.SupervisorApproved != "Rejected")).ToList());
        }
        //GET /REPORT/SupervisorReports 
        //Provides a list of reports based on a user's department for reports that have been submitted for approval
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorReports()
        {
            DepartmentType dept = DeptCheck();

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.SupervisorApproved == "Submitted").ToList());         
        }
        //GET /REPORT/SupervisorRejects
        //Provides a list of reports based on a user's department that have been rejected by Staff Accounts
        [Authorize(Roles = "Supervisor")]
        public ActionResult SupervisorRejects()
        {
            DepartmentType dept = DeptCheck();

            return View(db.Reports.Where(r => r.type == dept).Where(r => r.StaffApproval == "Rejected").ToList());
        }
        //GET /REPORT/SupervisorBudgets 
        //Provides the budget of a selected department
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
        // This is a re-useable method for dept checking
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
        //This displays the staff budget for all departments based on the start of the month
        [Authorize(Roles = "Staff")]
        public ActionResult StaffBudget()
        {
            SupervisorList list = new SupervisorList();
            double totalCurrency = 0;
            double supervisorCurrency = 0;
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
        //This provides a list of reports that are awaiting staff approval
        [Authorize(Roles = "Staff")]
        public ActionResult StaffReports()
        {
            ViewBag.HigherBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.HigherEducation));
            ViewBag.StateBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.State));
            ViewBag.LogisticsBudgetRemaining = (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(DepartmentType.Logistics));

            return View(db.Reports.Where(r => r.StaffApproval == null).Where(r => r.SupervisorApproved == "Approved").ToList());
        }
        //GET /REPORT/Approve/id(5 etc)
        // this allows a report to be approved for supervisors 
        [Authorize(Roles = "Supervisor")]
        [HttpGet]
        public ActionResult Approve(int? id)
        {
            // if it's over budget it sends to a confirmation page to be super approved(pun intended)
            if (GetReportCost(id) <= (DEFAULT_DEPT_BUDGET - GetSpentBudgetForSupervisor()))
            {
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
        //A method for getting report cost of particular ID - no real logic 
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
        //This approves a report for a supervisor and also redirects the page
        [Authorize(Roles = "Supervisor")]
        public ActionResult ApproveCon(int? ReportID)
        {
            DBL.SupAppCon(ReportID, User.Identity.Name.ToString());
             return RedirectToAction("SupervisorReports");
        }
        //GET /REPORT/Reject/ReportID(7 etc)
        //This rejects a report for a supervisor and also redirects the page
        [Authorize(Roles = "Supervisor")]
         public ActionResult Reject(int? id)
        {
            DBL.SupRej(id, User.Identity.Name.ToString());
            return RedirectToAction("SupervisorReports");
        }
        // This returns the spent budget for a supervisor
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
        // this allows a report to be approved for staff  
        [Authorize(Roles = "Staff")]
        [HttpGet]
        public ActionResult StaffApproval(int? id)
        {
            if (GetReportCost(id) <= (DEFAULT_DEPT_BUDGET - GetSpentBudgetForStaff(db.Reports.Find(id).type)))
            {
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
        // this returns the budget for a department - according to what staff require
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
        // this approves a report on a staff level and redirects 
        [Authorize(Roles = "Staff")]
        public ActionResult StaffApprovalCon(int? id)
        {
            DBL.StaffAppCon(id);
            return RedirectToAction("StaffReports");
        }
        //GET /REPORT/StaffReject/ReportID(1337 , 9001, etc)
        // this rejects a report on a staff level and redirects 
        [Authorize(Roles = "Staff")]
        public ActionResult StaffReject(int? id)
        {
            DBL.StaffRej(id);
            return RedirectToAction("StaffReports");
        }
    }
}
