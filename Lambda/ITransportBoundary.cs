using System;
using System.Threading;

namespace Lambda.GenH30
{
    public interface ITransportBoundary
    {                
        IBoundaryWriter Writer();        
    }

    public interface IBoundaryWriter : IDisposable
    {
        WriteResponse Write(string command, int timeout);
    }

    public static class BoundaryWriterExt
    {
        public static WriteResponse Write(this IBoundaryWriter boundary, string request, int timeout, int repeatCount,
                                   int repeatTimeout)
        {
            int count = 1;
            WriteResponse response;
            do
            {
                response = boundary.Write(request, timeout);
                if (response.Expired)
                {
                    Thread.Sleep(repeatTimeout);
                    count++;
                }
                else
                    break;
            } while (count <= repeatCount);
            
            if (count > repeatCount)
                response.AddError(new LambdaFailureException(Properties.Resources.WriteCountException));
            if(response.Expired)
                response.AddError(new LambdaFailureException(Properties.Resources.WriteTimeOut));

            return response;
        }
    }
}