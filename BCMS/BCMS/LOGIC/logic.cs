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
    public class logic{

        private BCMSContext db = new BCMSContext();
        public  void StaffAppCon(int? id)
        {
            db.Reports.Find(id).StaffApproval = "Approved";
            db.Reports.Find(id).DateOfApproval = DateTime.Now.Date;
            db.SaveChanges();
        }
        public void StaffRej(int? id)
        {
            db.Reports.Find(id).StaffApproval = "Rejcted";
            db.Reports.Find(id).DateOfApproval = DateTime.Now.Date;
            db.SaveChanges();
        }
        public void SupRej(int? id, string user)
        {
            db.Reports.Find(id).SupervisorName = user;
            db.Reports.Find(id).SupervisorApproved = "Rejected";
            db.SaveChanges();
        }
        public void SupAppCon(int? ReportID, string user)
        {
            db.Reports.Find(ReportID).SupervisorName = user;
            db.Reports.Find(ReportID).SupervisorApproved = "Approved";
            db.SaveChanges();
        }

        public void AddReport(Report report, string user)
        {
            report.ConsultantName = user;
            report.SupervisorApproved = "Submitted";
            db.Reports.Add(report);
            db.SaveChanges();
        }

        
    }
}