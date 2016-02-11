using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Changer2
{
    [TestClass]
    public class testing
    {
        [TestMethod]
        public void simple_builder()
        {

            var entity = new Foo();

            const float expected = 1.2f;

            var builder = new SimpleBuilder();
            builder.Setter(entity, "Pr1").Set(expected);

            Assert.AreEqual(expected, entity.Pr1);
            Assert.AreEqual(0, entity.Pr2);
        }

        [TestMethod]
        public void deep_builder()
        {

            var entity = new Foo();

            const float expected = 1.2f;

            var builder = new DeepBuilder();
            builder.Setter(entity, "Pr1").Set(expected);

            Assert.AreEqual(expected, entity.Pr1);
            Assert.AreEqual(expected + 1, entity.Pr2);
        }
    }
}