using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EfawateerTests
{
    [TestClass]
    public class initialize
    {
        [TestMethod]
        public void init()
        {
            var gate = new Gateways.EfawateerGateway();

            gate.Initialize(File.ReadAllText("initialize.xml"));
        }
    }
}
