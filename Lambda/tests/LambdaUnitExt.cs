using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sparc.Hardware.CommonUnit.Test;

namespace Sparc.Lambda.Test
{
    [TestClass]
    public class LambdaUnitExt
    {
        private readonly LambdaUnit _unit;

        public LambdaUnitExt()
        {
            _unit = new LambdaUnit(ConfigurationProvider.Current.Configuration);
        }

        [TestMethod]
        public void MeasureVoltage3()
        {
            _unit.MeasuredParameters(27, 0, 0, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureVoltage3(), "equal");

            _unit.MeasuredParameters(27.81f, 0, 0, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureVoltage3(), "max");

            _unit.MeasuredParameters(26.19f, 0, 0, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureVoltage3(), "min");

            _unit.MeasuredParameters(28f, 0, 0, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureVoltage3(), "more than max");

            _unit.MeasuredParameters(26f, 0, 0, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureVoltage3(), "less than min");
        }

        [TestMethod]
        public void MeasureCurrent()
        {
            _unit.MeasuredParameters(0, 0, 6, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureCurrent(), "equal");

            _unit.MeasuredParameters(0, 0, 7, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureCurrent(), "more");
        }

        [TestMethod]
        public void MeasureVoltagMore3Less10()
        {
            _unit.MeasuredParameters(27, 0, 0, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureVoltagMore3Less10(), "less");

            _unit.MeasuredParameters(28.5f, 0, 0, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureVoltagMore3Less10(), "admission");

            _unit.MeasuredParameters(30, 0, 0, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureVoltagMore3Less10(), "more");
        }

        [TestMethod]
        public void MeasureVoltagLess3()
        {
            _unit.MeasuredParameters(26, 0, 0, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureVoltagLess3(), "less");

            _unit.MeasuredParameters(28, 0, 0, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureVoltagLess3(), "more");
        }

        [TestMethod]
        public void MeasureVoltagMore10()
        {
            _unit.MeasuredParameters(26, 0, 0, 0, 0, 0);

            Assert.IsFalse(_unit.MeasureVoltagMore10(), "less");

            _unit.MeasuredParameters(30, 0, 0, 0, 0, 0);

            Assert.IsTrue(_unit.MeasureVoltagMore10(), "more");
        }

        [TestMethod]
        public void IsSettedVoltage()
        {
            float f;
            f = 27;
            _unit.MeasuredParameters(0, f, 0, 0, 0, 0);
            Assert.IsTrue(_unit.IsSettedVoltage(), f.ToString());

            f = 27.002f;
            _unit.MeasuredParameters(0, f, 0, 0, 0, 0);
            Assert.IsTrue(_unit.IsSettedVoltage(), f.ToString());

            f = 26.999f;
            _unit.MeasuredParameters(0, f, 0, 0, 0, 0);
            Assert.IsTrue(_unit.IsSettedVoltage(), f.ToString());

            f = 26.5f;
            _unit.MeasuredParameters(0, f, 0, 0, 0, 0);
            Assert.IsFalse(_unit.IsSettedVoltage(), f.ToString());

            f = 27.1f;
            _unit.MeasuredParameters(0, f, 0, 0, 0, 0);
            Assert.IsFalse(_unit.IsSettedVoltage(), f.ToString());
        }

        [TestMethod]
        public void IsSettedCurrent()
        {
            float f;
            f = 6;
            _unit.MeasuredParameters(0, 0, 0, f, 0, 0);
            Assert.IsTrue(_unit.IsSettedCurrent(), f.ToString());

            f = 6.002f;
            _unit.MeasuredParameters(0, 0, 0, f, 0, 0);
            Assert.IsTrue(_unit.IsSettedCurrent(), f.ToString());

            f = 5.999f;
            _unit.MeasuredParameters(0, 0, 0, f, 0, 0);
            Assert.IsTrue(_unit.IsSettedCurrent(), f.ToString());

            f = 6.5f;
            _unit.MeasuredParameters(0, 0, 0, f, 0, 0);
            Assert.IsFalse(_unit.IsSettedCurrent(), f.ToString());

            f = 6.1f;
            _unit.MeasuredParameters(0, 0, 0, f, 0, 0);
            Assert.IsFalse(_unit.IsSettedCurrent(), f.ToString());
        }
    }
}
