using System;
using EfawateerWcf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void BillerList()
        {
            new Class1().BillerList();
        }

        [TestMethod]
        public void AccountUpload()
        {
            new Class1().AccountUpload();
        }

        [TestMethod]
        public void TestMethod3()
        {
            new Class1().AccountInquiry();
        }

        [TestMethod]
        public void TestMethod4()
        {
            new Class1().AddCustomerBilling();
        }

        [TestMethod]
        public void TestMethod5()
        {
            new Class1().BillPayment();
        }

        [TestMethod]
        public void BillInquiry()
        {
            new Class1().BillInquiry();
        }

        [TestMethod]
        public void TestMethod7()
        {
            new Class1().PaymentInquiry();
        }

        [TestMethod]
        public void TestMethod8()
        {
            new Class1().PrepaidPayment();
        }

        [TestMethod]
        public void TestMethod9()
        {
            new Class1().PrepaidValidation();
        }
    }
}
