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
        public void TestController()
        {
            //BCMS.Controllers.ReportController rc = new BCMS.Controllers.ReportController();
            //rc.Index();
            //var result = rc.Index();
            //Assert.IsNotNull(result);

        }
    }
}
