using BCMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace BlueConsultingManagementSystem.Models
{
    public class Report
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ReportPK { get; set; }

        [Required]
        public string ReportName { get; set; }

        //[Required]
        public string ConsultantName { get; set; }

        [Required]
        public DepartmentType type { get; set; }

        //This is the name of the supervisor. fix later
        //[Required]
        public string SupervisorApproved { get; set; }

        //[Required]
        public string StaffApproval { get; set; }

        //[Required]
        public DateTime? DateOfApproval { get; set; }

        public virtual List<Expense> Expenses { get; set; }

        public virtual Report Parent { get; set; }

        [InverseProperty("Parent")]
        public virtual List<Report> Children { get; set; }

        [NotMapped]
        public string FullReport
        {
            get
            {
                return ReportName + " " + ConsultantName + " " + type + " " + SupervisorApproved + " " + SupervisorApproved + " " + StaffApproval + " " + DateOfApproval;
            }
        }

        //[NotMapped]
        //public List<Report> GenerateSupervisorReportList()
        //{
        //    List<Report> reports = new List<Report>();

        //    foreach(Report rp in Report)

        //    return reports;
        //}
    }
}

