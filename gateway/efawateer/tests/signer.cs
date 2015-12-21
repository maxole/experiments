using System;
using Gateways;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class signer
    {
        [TestMethod]
        public void TestMethod1()
        {
            var s = new EfawateerSigner("CN=XM", "test");
            s.CheckCerificate();
        }
    }
}
