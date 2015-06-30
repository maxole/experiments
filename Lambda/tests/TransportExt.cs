using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Sparc.Lambda.Test
{
    /*
    [TestClass]
    public class TransportExt
    {
        private readonly ILambdaProtocol _lambdaProtocol;

        public TransportExt()
        {
            _lambdaProtocol = MockRepository.GenerateMock<ILambdaProtocol>();
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void adr_fail()
        {
            _lambdaProtocol
                .Stub(t => t.Adr(Arg<byte>.Is.Anything))
                .Do(new Func<byte, WriteResponse>(address => new WriteResponse()));
            // action
            const byte adr = 6;
            _lambdaProtocol.Adr("idn", adr, 20);
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void out_on_fail()
        {
            _lambdaProtocol
                .Stub(t => t.OutOn())
                .Do(new Func<bool>(() => false));
            // action
            const byte adr = 6;
            _lambdaProtocol.OutOn("idn", adr, 20);
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void out_off_fail()
        {
            _lambdaProtocol
                .Stub(t => t.OutOff())
                .Do(new Func<bool>(() => false));
            // action
            const byte adr = 6;
            _lambdaProtocol.OutOff("idn", adr, 20);
        }

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void idn_fail()
        {
            _lambdaProtocol
                .Stub(t => t.Idn())
                .Do(new Func<string>(() => "NOT_SET"));
            // action
            const byte adr = 6;
            _lambdaProtocol.OutOff("idn", adr, 20);
        }
    }  
    */
}
