﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BCMS;
using System.Web;


namespace BCMSTests
{
    [TestClass]
    public class BCMSUnitTests
    {
        private BlueConsultingManagementSystem.Models.Report report = new BlueConsultingManagementSystem.Models.Report();
        private BlueConsultingManagementSystem.Models.Expense expense = new BlueConsultingManagementSystem.Models.Expense();
            
        [TestMethod]
        public void CurrencyConversionTest()
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
        public void SupervisorExpenseListCreationTest()
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
        public void SupervisorAndExpensesPairingTest()
        {
            BCMS.Models.Supervisor super = new BCMS.Models.Supervisor("Bill", 500);
            Assert.AreEqual(500, super.ReturnAmount());
            Assert.AreEqual("Bill", super.ReturnName());
            super.SetAmount(800);
            Assert.AreEqual(800, super.ReturnAmount());
        }

        [TestMethod]
        public void ReportConsultantSubmissionTest()
        {
            report.ReportPK = 999;
            Assert.AreEqual(999, report.ReportPK);
            report.ReportName = "The test report";
            Assert.AreEqual("The test report", report.ReportName);
            report.SetConsultantName("Bill");
            Assert.AreEqual("Bill", report.ConsultantName);
            report.type = BCMS.Models.DepartmentType.HigherEducation;
            Assert.AreEqual("HigherEducation", BCMS.Models.DepartmentType.HigherEducation.ToString());
            report.SetSupervisorStatusToSubmitted();
            Assert.AreEqual("Submitted", report.SupervisorApproved);
        }

        [TestMethod]
        public void ReportSupervisorApproveTest()
        {
            report.SetSupervisorName("Jeff");
            Assert.AreEqual("Jeff", report.SupervisorName);
            report.SetSupervisorStatusToApproved();
            Assert.AreEqual("Approved", report.SupervisorApproved);
        }

        [TestMethod]
        public void ReportSupervisorRejectTest()
        {
            report.SetSupervisorStatusToRejected();
            Assert.AreEqual("Rejected", report.SupervisorApproved);
        }

        [TestMethod]
        public void ReportStaffApproveTest()
        {
            report.SetStaffStatusToApproved();
            Assert.AreEqual("Approved", report.StaffApproval);
            report.SetDateOfApproval();
            Assert.AreEqual(DateTime.Now.Date, report.DateOfApproval);
        }

        public void ReportStaffRejectTest()
        {
            report.SetStaffStatusToRejected();
            Assert.AreEqual("Rejected", report.StaffApproval);
        }

        [TestMethod]
        public void ExpenseCreationTest()
        {
            expense.ExpensePK = 999;
            Assert.AreEqual(999, expense.ExpensePK);
            expense.CType = BCMS.Models.CurrencyType.CNY;
            Assert.AreEqual("CNY", expense.CType.ToString());
            expense.Amount = 555.5;
            Assert.AreEqual(555.5, expense.Amount);
            expense.Description = "To the shops";
            Assert.AreEqual("To the shops", expense.Description);
            expense.Location = "Sydney";
            Assert.AreEqual("Sydney", expense.Location);
            expense.Report = report;
            Assert.IsNotNull(expense.Report);
            expense.DateOfExpense = DateTime.Now.Date;
            Assert.AreEqual(DateTime.Now.Date, expense.DateOfExpense);           
        }

        [TestMethod]
        public void ExpenseConversionTest()
        {
            expense.CType = BCMS.Models.CurrencyType.CNY;
            expense.Amount = 555.5;
            double testAmount = Convert.ToDouble(expense.ConvertedAmount.ToString("#.##"));
            Assert.AreEqual(95.64, testAmount);
            Assert.AreNotSame(555.5, testAmount);
        }

        [TestMethod]
        public void ReportAddExpenseToReportTest()
        {           
            report.Expenses = new System.Collections.Generic.List<BlueConsultingManagementSystem.Models.Expense>();
            Assert.AreEqual(0, report.Expenses.Count);
            report.Expenses.Add(expense);
            Assert.AreEqual(1, report.Expenses.Count);
            Assert.IsNotNull(report.Expenses);
        }
    }
}
