using System;
using System.Activities;
using KpkWorkflow.Model;

namespace Sparc.Kpk12.Domain
{    
    public class WorkflowRunner : IWorkflowRunner, IListener<WorkflowAbortEvent>
    {
        private readonly IWorkflowProvider _workflowProvider;
        private readonly IEventBroker _eventBroker;
        private WorkflowApplication _workflow;

        public WorkflowRunner(IWorkflowProvider workflowProvider, IEventBroker eventBroker)
        {
            _workflowProvider = workflowProvider;
            _eventBroker = eventBroker;
        }

        public void Run(Activity activity)
        {
            _workflow = _workflowProvider.CreateApplication(activity);

            _workflow.Aborted = OnAborted;
            _workflow.Completed = OnCompleted;
            _workflow.OnUnhandledException = OnUnhandledException;
            
            _eventBroker.Publish(new WorkflowRuning());            

            _workflow.Run();
        }

        public void Cancel()
        {
            _workflow.Cancel();
        }

        public void Abort()
        {
            _workflow.Abort();
        }

        public ISequenceActivity CreateSequence()
        {
            if(_workflow == null)
                throw new NotSupportedException("workflow is null");
            return new SequenceActivity(_workflow);
        }

        private void OnCompleted(WorkflowApplicationCompletedEventArgs args)
        {
            _eventBroker.Publish(new WorkflowCompleted(args.CompletionState));
        }

        private void OnAborted(WorkflowApplicationAbortedEventArgs args)
        {
            _eventBroker.Publish(new WorkflowAborted(args.Reason));   
        }

        private UnhandledExceptionAction OnUnhandledException(WorkflowApplicationUnhandledExceptionEventArgs args)
        {
            var e = args.UnhandledException;
            _eventBroker.Publish(new WorkflowUnhandledException(e));
            return UnhandledExceptionAction.Cancel;
        }

        public void Handle(WorkflowAbortEvent e)
        {
            _workflow.Abort("user abort");
        }

        /*
static void InspectActivity(Activity root, int indent)
{
    // Inspect the activity tree using WorkflowInspectionServices.
    IEnumerator<Activity> activities =
        WorkflowInspectionServices.GetActivities(root).GetEnumerator();

    Debug.WriteLine("{0}{1}", new string(' ', indent), root.DisplayName);

    while (activities.MoveNext())
    {
        InspectActivity(activities.Current, indent + 2);
    }
}*/       
    }
}