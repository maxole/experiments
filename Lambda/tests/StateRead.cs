using System;
using System.Collections.Generic;
using Core.Trace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Sparc.Hardware.CommonUnit.Test;

namespace Sparc.Lambda.Test
{
    [TestClass]
    public class StateRead
    {
        private readonly ILambdaProtocol _lambdaProtocol;
        private readonly List<string> _transportLog;
        private readonly ILambdaWorker _worker;
        private readonly LambdaUnit _unit;
        private readonly ILogger _logger;
        private readonly List<string> _loggerLog;

        public StateRead()
        {
            _transportLog = new List<string>();

            _lambdaProtocol = MockRepository.GenerateMock<ILambdaProtocol>();
            _lambdaProtocol.Stub(t => t.OutOn())
                .Do(new Func<WriteResponse>(() =>
                {
                    _transportLog.Add("OUT 0");
                    return new WriteResponse();
                }));

            _lambdaProtocol.Stub(t => t.Pv(Arg<float>.Is.Anything))
                      .Do(new Func<float, WriteResponse>(a =>
                      {
                          _transportLog.Add("PV " + a);
                          return new WriteResponse();
                      }));
            _lambdaProtocol.Stub(t => t.Pc(Arg<float>.Is.Anything))
                      .Do(new Func<float, WriteResponse>(a =>
                      {
                          _transportLog.Add("PC " + a);
                          return new WriteResponse();
                      }));

            _worker = MockRepository.GenerateMock<ILambdaWorker>();

            _unit = new LambdaUnit(ConfigurationProvider.Current.Configuration);

            _logger = MockRepository.GenerateMock<ILogger>();
            _logger.Stub(
                l => l.Log(Arg<LoggerEntryLevel>.Is.Anything, Arg<string>.Is.Anything, Arg<Exception>.Is.Anything,
                    Arg<IDictionary<string, object>>.Is.Anything))
                   .WhenCalled(mi => _loggerLog.Add((string) mi.Arguments[1]));

            _loggerLog = new List<string>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _transportLog.Clear();
            _loggerLog.Clear();
        }

        [TestMethod]
        public void expect_ok()
        {
            var init = new Lambda.StateRead(_logger, _lambdaProtocol, _unit);
            _unit.MeasuredParameters(27, 0, 6, 0, 0, 0);
            init.Process();

            Assert.AreEqual(
                "",
                string.Join(", ", _transportLog),
                "lambdaProtocol");

            var actual = "[] " + Lambda.StateRead.Ok;

            Assert.AreEqual(actual, string.Join(", ", _loggerLog),
                "loger");
        }

        [TestMethod]
        public void expect_measure_voltage_3()
        {
            const float measureVoltage = 27;
            const float measureCurrent = 7;
            var init = new Lambda.StateRead(_logger, _lambdaProtocol, _unit);
            _unit.MeasuredParameters(measureVoltage, 0, measureCurrent, 0, 0, 0);
            init.Process();

            Assert.AreEqual(
                "",
                string.Join(", ", _transportLog),
                "lambdaProtocol");

            var config = ConfigurationProvider.Current.Configuration.GetCustomConfig<Config>();

            var actual = string.Format("[] " + Lambda.StateRead.Info,
                config.Voltage, measureVoltage, measureCurrent);

            Assert.AreEqual(actual, string.Join(", ", _loggerLog),
                "loger");
        }

        [TestMethod]
        public void expect_measure_voltage_more_10()
        {
            const float measureVoltage = 30;
            const float measureCurrent = 7;
            var init = new Lambda.StateRead(_logger, _lambdaProtocol, _unit);
            _unit.MeasuredParameters(measureVoltage, 0, measureCurrent, 0, 0, 0);

            var exceptionWasRised = false;
            try
            {
                init.Process();
            }
            catch (LambdaFailureException)
            {
                exceptionWasRised = true;
            }

            Assert.AreEqual(
                "OUT 0",
                string.Join(", ", _transportLog),
                "lambdaProtocol");

            Assert.IsTrue(exceptionWasRised, "exception");

            //var config = ConfigurationProvider.Current.Configuration.GetCustomConfig<Config>();

            //var actual = string.Format("[] " + Lambda.StateRead.Info,
            //    config.Voltage, measureVoltage, measureCurrent);

            //Assert.AreEqual(actual, string.Join(", ", _loggerLog),
            //    "loger");
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void unexpected_powerOn_method()
        {
            var init = new Lambda.StateRead(_logger, _lambdaProtocol, _unit);
            init.PowerOn(null);
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void unexpected_read_init_method()
        {
            var init = new Lambda.StateRead(_logger, _lambdaProtocol, _unit);
            init.ReadInit(null);
        }
    }
}