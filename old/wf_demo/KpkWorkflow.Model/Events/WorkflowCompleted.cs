using System;
using System.Activities;

namespace KpkWorkflow.Model
{
    public class WorkflowCompleted : IEvent
    {
        public ActivityInstanceState State { get; private set; }

        public WorkflowCompleted(ActivityInstanceState state)
        {
            State = state;
        }
    }

    public class WorkflowAborted : IEvent
    {
        public Exception Reason { get; private set; }

        public WorkflowAborted(Exception reason)
        {
            Reason = reason;
        }
    }

    //UnhandledException
    public class WorkflowUnhandledException : IEvent
    {
        public Exception Error { get; private set; }

        public WorkflowUnhandledException(Exception error)
        {
            Error = error;
        }
    }
}