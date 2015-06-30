using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace Sparc.Lambda.Test
{
    [TestClass]
    public class BoundaryWriter
    {
        private const string Request = "request";
        private const int Timeout = 100;
        private const int RepeatTimeout = 1;

        [TestMethod, ExpectedException(typeof(LambdaFailureException))]
        public void write_exception()
        {
            const int repeatCount = 1;
            var boundary = MockRepository.GenerateMock<IBoundaryWriter>();
            boundary.Stub(b => b.Write(Arg<string>.Is.Anything, Arg<int>.Is.Anything))
                    .Do(new Func<string, int, WriteResponse>((s, s1) => new WriteResponse
                    {
                        Expired = true
                    }));

            var response = boundary.Write(Request, Timeout, repeatCount, RepeatTimeout);
            if (response.Error != null)
                throw response.Error;
        }

        [TestMethod]
        public void write_success_after_two_times()
        {
            const int repeatCount = 3;
            var r = new[] { true, true, false };
            var i = 0;
            var boundary = MockRepository.GenerateMock<IBoundaryWriter>();
            boundary.Stub(b => b.Write(Arg<string>.Is.Anything, Arg<int>.Is.Anything))
                    .Do(new Func<string, int, WriteResponse>((s, s1) => new WriteResponse
                    {
                        Expired = r[i++]
                    }));

            var response = boundary.Write(Request, Timeout, repeatCount, RepeatTimeout);
            if (response.Error != null)
                throw response.Error;
        }
    }
}