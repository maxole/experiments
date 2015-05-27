using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedAgent.Impl;

namespace SharedResource.Test
{
    [TestClass]
    public class InsertTest
    {
        [TestMethod]
        public void SimpleAddGetObject()
        {
            var o = new MyObject();

            var agent = new TransientAgent();
            agent.Set(o);            

            Transient<MyObject> a;
            var b = agent.TryGet(out a);
            
            Assert.IsTrue(b);
            Assert.IsInstanceOfType(a.Value, typeof(MyObject));
        }

        [TestMethod, ExpectedException(typeof(DuplicateKeyException))]
        public void DublicateAddObjectExpectedException()
        {
            var o = new MyObject();

            var agent = new SharedAgent.Impl.TransientAgent();
            agent.Set(o);
            agent.Set(o);            
        }
    }
}
