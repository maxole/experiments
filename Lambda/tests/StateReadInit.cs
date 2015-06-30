using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;
using Sparc.Hardware.CommonUnit.Test;

namespace Sparc.Lambda.Test
{
    [TestClass]
    public class StateReadInit
    {
        private readonly ILambdaProtocol _lambdaProtocol;
        private readonly List<string> _log;
        private readonly ILambdaWorker _worker;
        private State _lastState;
        private LambdaUnit _unit;

        public StateReadInit()
        {
            _log = new List<string>();
            _lambdaProtocol = MockRepository.GenerateMock<ILambdaProtocol>();
            _lambdaProtocol.Stub(t => t.OutOff())
                           .Do(new Func<WriteResponse>(() =>
                           {
                               _log.Add("OUT 1");
                               return new WriteResponse();
                           }));
            _lambdaProtocol.Stub(t => t.Pv(Arg<float>.Is.Anything))
                           .Do(new Func<float, WriteResponse>(a =>
                           {
                               _log.Add("PV " + a);
                               return new WriteResponse();
                           }));
            _lambdaProtocol.Stub(t => t.Pc(Arg<float>.Is.Anything))
                           .Do(new Func<float, WriteResponse>(a =>
                           {
                               _log.Add("PC " + a);
                               return new WriteResponse();
                           }));

            _worker = MockRepository.GenerateMock<ILambdaWorker>();
            _worker
                .Stub(w => w.Goto(Arg<State>.Is.Anything))
                .Do(new Action<State>(state => _lastState = state));

            _unit = new LambdaUnit(ConfigurationProvider.Current.Configuration);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _lastState = null;
            _log.Clear();
        }

        [TestMethod]
        public void expect_pv_pc()
        {
            var init = new Lambda.StateReadInit(_lambdaProtocol, _unit);
            _unit.MeasuredParameters(0, 0, 0, 0, 0, 0);
            var repeat = init.Process();
            const string expected = "PV 27, PC 6";
            var actual = string.Join(", ", _log);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(repeat, "repeat");
        }

        [TestMethod]
        public void expect_pv()
        {
            var init = new Lambda.StateReadInit(_lambdaProtocol, _unit);
            _unit.MeasuredParameters(0, 0, 0, 6, 0, 0);
            var repeat = init.Process();
            const string expected = "PV 27";
            var actual = string.Join(", ", _log);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(repeat, "repeat");
        }

        [TestMethod]
        public void expect_pc()
        {
            var init = new Lambda.StateReadInit(_lambdaProtocol, _unit);
            _unit.MeasuredParameters(0, 27, 0, 0, 0, 0);
            var repeat = init.Process();
            const string expected = "PC 6";
            var actual = string.Join(", ", _log);
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(repeat, "repeat");
        }

        [TestMethod]
        public void expect_out_1()
        {
            var init = new Lambda.StateReadInit(_lambdaProtocol, _unit);
            _unit.MeasuredParameters(0, 27, 0, 6, 0, 0);
            var repeat = init.Process();
            const string expected = "OUT 1";
            var actual = string.Join(", ", _log);
            Assert.AreEqual(expected, actual);
            Assert.IsFalse(repeat, "repeat");
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void unexpected_powerOn_method()
        {
            var init = new Lambda.StateReadInit(_lambdaProtocol, _unit);
            init.PowerOn(null);
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void unexpected_read_method()
        {
            var init = new Lambda.StateReadInit(_lambdaProtocol, _unit);
            init.Read(null);
        }
    }
}
