using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueConsultingManagementSystem.Models
{
    public class ConsultantModel
    {

        public ConsultantModel()
        {

        }
        public Expense AddExpense(string Des, double Amo)
        {
            using (var db = new BCMSModelContainer())
            {
                Expense exp = new Expense { Description = Des, Amount = Amo };
                
                return exp;
            }
            
        }
        public Report AddReport(string RepName)
        {
            using (var db = new BCMSModelContainer())
            {
                Report rp = new Report { ReportName = RepName };
                
                return rp;
            }
            
            
        }
        public void TogetherAdd(string Description, double Amount, string ReportName)
        {
            Expense exp = AddExpense(Description, Amount);
            Report rp = AddReport(ReportName);
            using (var db = new BCMSModelContainer())
            {
                db.Expenses.Add(exp);
                db.Reports.Add(rp);
                rp.Expenses.Add(exp);
                db.SaveChanges();
            }
        }

        public List<Report> ReturnReportsOnName(string consultantName)
        {
            using(var db = new BCMSModelContainer())
            {
                List<Report> reports = new List<Report>();

                foreach(Report rp in db.Reports)
                {
                    if(rp.ConsultantName == consultantName)
                    {
                        reports.Add(rp);
                    }
                    //reports.Add(rp);
                }
                return reports;
            }        
        }
    }
}