using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedAgent.Impl;

namespace SharedResource.Test
{
    [TestClass]
    public class WorkingWithTest
    {
        readonly MyObject _sharedObject = new MyObject();
        private readonly ITransientAgent _agent;

        public WorkingWithTest()
        {
            _sharedObject = new MyObject();
            _agent = new TransientAgent();
        }
        
        [TestInitialize()]
        public void Initialize()
        {                       
            _agent.Set(_sharedObject);
        }

        [TestMethod]
        public void DublicateTryGet()
        {
            Transient<MyObject> a;
            var success = _agent.TryGet(out a);

            Transient<MyObject> b;
            var fail = _agent.TryGet(out b);

            Assert.IsTrue(success);
            Assert.IsFalse(fail);
        }

        [TestMethod]
        public void GetResourceAndDisposeIt()
        {
            Transient<MyObject> a;

            _agent.TryGet(out a);

            using (a)
            {
                // do some work
            }

            var success = _agent.TryGet(out a);
            Assert.IsTrue(success);            
        }
    }
}
