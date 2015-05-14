using System;

namespace KpkWorkflow.Model
{
    public class ExceptionEvent : IEvent
    {
        public Exception Exception { get; private set; }

        public ExceptionEvent(Exception exception)
        {
            Exception = exception;
        }
    }
}