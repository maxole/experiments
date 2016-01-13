using System;

namespace Lambda.GenH30
{
    /// <summary>
    /// отказ оборудования
    /// </summary>
    public class LambdaFailureException : Exception
    {
        public LambdaFailureException()
        {

        }

        public LambdaFailureException(string message) : base(message)
        {

        }

        public LambdaFailureException(string message, Exception exception)
            : base(message, exception)
        {

        }
    }
}