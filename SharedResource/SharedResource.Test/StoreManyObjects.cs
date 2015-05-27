using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharedAgent.Impl;

namespace SharedResource.Test
{
    [TestClass]
    public class StoreManyObjects
    {
        [TestMethod]
        public void StoreTwoObjectsAndGetOne()
        {
            var o1 = new MyObject();
            var o2 = new MyObject2();

            var agent = new TransientAgent();
            agent.Set(o1);
            agent.Set(o2);

            Transient<MyObject2> m;
            var b = agent.TryGet(out m);

            Assert.IsTrue(b);

            Assert.IsInstanceOfType(m.Value, typeof(MyObject2));
        }

        [TestMethod]
        public void StoreTwoObjectsAndGetTwo()
        {
            var o1 = new MyObject();
            var o2 = new MyObject2();

            var agent = new TransientAgent();
            agent.Set(o1);
            agent.Set(o2);

            Transient<MyObject2> transient2;
            var b = agent.TryGet(out transient2);

            Assert.IsTrue(b);

            Assert.IsInstanceOfType(transient2.Value, typeof(MyObject2));

            Transient<MyObject> transient1;
            var b1 = agent.TryGet(out transient1);

            Assert.IsTrue(b1);

            Assert.IsInstanceOfType(transient1.Value, typeof(MyObject));
        }

        [TestMethod]
        public void StoreTwoObjectsAndGetOneSecondTimes()
        {
            var o1 = new MyObject();
            var o2 = new MyObject2();

            var agent = new TransientAgent();
            agent.Set(o1);
            agent.Set(o2);

            Transient<MyObject2> transient2;
            var b = agent.TryGet(out transient2);
            
            b = agent.TryGet(out transient2);

            Assert.IsFalse(b);
        }

        [TestMethod]
        public void GetObjectByInterface()
        {
            IInput o = new InputInstance();

            var agent = new TransientAgent();
            agent.Set(o);

            Transient<IInput> transient;
            var b = agent.TryGet(out transient);

            Assert.IsTrue(b);

            Assert.IsInstanceOfType(transient.Value, typeof(IInput));
        }
    }
}
