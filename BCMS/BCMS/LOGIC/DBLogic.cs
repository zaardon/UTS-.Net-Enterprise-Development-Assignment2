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
    public class DBLogic{

        private BCMSContext db = new BCMSContext();
        public void StaffAppCon(int? ReportID)
        {
            //db.Reports.Find(ReportID).StaffApproval = "Approved";
            //db.Reports.Find(ReportID).DateOfApproval = DateTime.Now.Date;
            db.Reports.Find(ReportID).SetStaffStatusToApproved();
            db.Reports.Find(ReportID).SetDateOfApproval();
            db.SaveChanges();
        }
        public void StaffRej(int? ReportID)
        {
            //db.Reports.Find(ReportID).StaffApproval = "Rejcted";
            //db.Reports.Find(ReportID).DateOfApproval = DateTime.Now.Date;

            db.Reports.Find(ReportID).SetStaffStatusToRejected();
            db.Reports.Find(ReportID).SetDateOfApproval();
            db.SaveChanges();
        }
        public void SupRej(int? ReportID, string user)
        {
            //db.Reports.Find(ReportID).SupervisorName = user;
            //db.Reports.Find(ReportID).SupervisorApproved = "Rejected";
            db.Reports.Find(ReportID).SetSupervisorName(user);
            db.Reports.Find(ReportID).SetSupervisorStatusToRejected();
            db.SaveChanges();
        }

        public void SupAppCon(int? ReportID, string user)
        {
            //db.Reports.Find(ReportID).SupervisorName = user;
            //db.Reports.Find(ReportID).SupervisorApproved = "Approved";
            db.Reports.Find(ReportID).SetSupervisorName(user);
            db.Reports.Find(ReportID).SetSupervisorStatusToApproved();
            db.SaveChanges();
        }
        //done
        public void AddReport(Report report, string user)
        {
            report.SetConsultantName(user);
            report.SetSupervisorStatusToSubmitted();
            db.Reports.Add(report);
            db.SaveChanges();
        }

        
    }
}