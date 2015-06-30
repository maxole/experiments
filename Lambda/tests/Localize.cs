using System.Globalization;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sparc.Lambda.Test
{
    [TestClass]
    public class Localize
    {
        [TestMethod]
        public void ru_culture()
        {

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ru");

            var actual = string.Empty;
            var init = new Lambda.StateRead(null, null, null);
            try
            {
                init.PowerOn(null);
            }
            catch (LambdaFailureException exception)
            {
                actual = exception.Message;
            }

            const string expected = "Инициализация устройства выполнена ранее";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void en_culture()
        {

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en");

            var actual = string.Empty;
            var init = new Lambda.StateRead(null, null, null);
            try
            {
                init.PowerOn(null);
            }
            catch (LambdaFailureException exception)
            {
                actual = exception.Message;
            }

            const string expected = "Initializing device done previously";

            Assert.AreEqual(expected, actual);
        }
    }
}
