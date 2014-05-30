using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCMS;
using System.Web;


namespace BCMSTests
{
    [TestClass]
    public class BCMSUnitTests
    {

        [TestMethod]
        public void CurrencyTest()
        {
            double expected;
            BCMS.Models.CurrencyConverter cc = new BCMS.Models.CurrencyConverter();
            expected = cc.ConvertCurrencyToAUD("EUR", 1);
            Assert.AreEqual(1.49, expected);
            expected = cc.ConvertCurrencyToAUD("CNY", 1);
            Assert.AreEqual(0.172175, expected);
            expected = cc.ConvertCurrencyToAUD("fsd", 1);
            Assert.AreEqual(0, expected);
        }

        [TestMethod]
        public void CurrencyTypeTest()
        {
            Assert.AreEqual("AUD", BCMS.Models.CurrencyType.AUD.ToString());
            Assert.AreEqual("CNY", BCMS.Models.CurrencyType.CNY.ToString());
            Assert.AreEqual("EUR", BCMS.Models.CurrencyType.EUR.ToString());
        }

        [TestMethod]
        public void DepartmentTypeTest()
        {
            Assert.AreEqual("HigherEducation", BCMS.Models.DepartmentType.HigherEducation.ToString());
            Assert.AreEqual("State", BCMS.Models.DepartmentType.State.ToString());
            Assert.AreEqual("Logistics", BCMS.Models.DepartmentType.Logistics.ToString());
        }

        [TestMethod]
        public void SupervisorListTest()
        {
            BCMS.Models.SupervisorList sl = new BCMS.Models.SupervisorList();
            Assert.AreEqual(0, sl.ReturnSupervisors().Count);
            sl.AddToList("Bill", 500);
            Assert.AreEqual(1, sl.ReturnSupervisors().Count);
            sl.AddToList("Bill", 800);
            sl.AddToList("Jeff", 8000);
            Assert.AreEqual(2, sl.ReturnSupervisors().Count);
            BCMS.Models.Supervisor super = sl.ReturnSupervisors().Find(i => i.ReturnName() == "Bill");
            Assert.AreEqual(1300, super.ReturnAmount());
            super = sl.ReturnSupervisors().Find(i => i.ReturnName() == "Jeff");
            Assert.AreEqual(8000, super.ReturnAmount());
        }

        [TestMethod]
        public void SupervisorTest()
        {
            BCMS.Models.Supervisor super = new BCMS.Models.Supervisor("Bill", 500);
            Assert.AreEqual(500, super.ReturnAmount());
            Assert.AreEqual("Bill", super.ReturnName());
            super.SetAmount(800);
            Assert.AreEqual(800, super.ReturnAmount());
        }

        [TestMethod]
        public void ReportTest()
        {
            BlueConsultingManagementSystem.Models.Report report = new BlueConsultingManagementSystem.Models.Report();
            report.ReportPK = 999;
            Assert.AreEqual(999, report.ReportPK);
            report.ReportName = "The test report";
            Assert.AreEqual("The test report", report.ReportName);
            report.SetConsultantName("Bill");
            Assert.AreEqual("Bill", report.ConsultantName);
            report.SetSupervisorName("Jeff");
            Assert.AreEqual("Jeff",report.SupervisorName);
            report.type = BCMS.Models.DepartmentType.HigherEducation;
            Assert.AreEqual("HigherEducation", BCMS.Models.DepartmentType.HigherEducation.ToString());           
            report.SetSupervisorStatusToSubmitted();
            Assert.AreEqual("Submitted", report.SupervisorApproved);
            report.SetSupervisorStatusToApproved();
            Assert.AreEqual("Approved", report.SupervisorApproved);
            report.SetSupervisorStatusToRejected();
            Assert.AreEqual("Rejected", report.SupervisorApproved);
            report.SetStaffStatusToApproved();
            Assert.AreEqual("Approved", report.StaffApproval);
            report.SetStaffStatusToRejected();
            Assert.AreEqual("Rejected", report.StaffApproval);
            report.SetDateOfApproval();
            Assert.AreEqual(DateTime.Now.Date, report.DateOfApproval);



            //Why wont this work???

            //BlueConsultingManagementSystem.Models.Expense expense = new BlueConsultingManagementSystem.Models.Expense();
            //expense.ExpensePK = 100;
            //expense.CType = BCMS.Models.CurrencyType.AUD;
            //expense.Amount =555.5;
            //expense.Description = "gdfgfd";
            //expense.Report = report;
            //expense.Location = "dfgd";
            //report.Expenses.Add(expense);

            //Assert.AreEqual(1, report.Expenses.Count);
        }

        [TestMethod]
        public void ExpenseTest()
        {
            BlueConsultingManagementSystem.Models.Expense expense = new BlueConsultingManagementSystem.Models.Expense();
            //expense.
        }

        [TestMethod]
        public void BCMSContextTest()
        {
            //BCMS.Models.BCMSContext context = new BCMS.Models.BCMSContext();
            
        }

        public void DBLogicTests()
        {
            BCMS.Models.DBLogic DBL = new BCMS.Models.DBLogic();
        }
    }
}
