using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;
using System.Net;
using System.Web.Mvc;
using BCMS.Models;
using BlueConsultingManagementSystem.Models;

namespace BCMS.Models
{
    //  this is class is for talking to the db layer instead of the controller talking directly for insertions 
    public class DBLogic{

        private BCMSContext db = new BCMSContext();
        // confirmation from staff approval just an update
        public void StaffAppCon(int? ReportID)
        {
            db.Reports.Find(ReportID).SetStaffStatusToApproved();
            db.Reports.Find(ReportID).SetDateOfApproval();
            db.SaveChanges();
        }

        // updates the report with staff rejection 
        public void StaffRej(int? ReportID)
        {
            db.Reports.Find(ReportID).SetStaffStatusToRejected();
            db.Reports.Find(ReportID).SetDateOfApproval();
            db.SaveChanges();
        }

        // supervisor reject update on report 
        public void SupRej(int? ReportID, string user)
        {
            db.Reports.Find(ReportID).SetSupervisorName(user);
            db.Reports.Find(ReportID).SetSupervisorStatusToRejected();
            db.SaveChanges();
        }
        // supervisor approval on report update
        public void SupAppCon(int? ReportID, string user)
        {
            db.Reports.Find(ReportID).SetSupervisorName(user);
            db.Reports.Find(ReportID).SetSupervisorStatusToApproved();
            db.SaveChanges();
        }
        // adding the report to the database
        public void AddReport(Report report, string user)
        {
            report.SetConsultantName(user);
            report.SetSupervisorStatusToSubmitted();
            db.Reports.Add(report);
            db.SaveChanges();
        }
    }
}