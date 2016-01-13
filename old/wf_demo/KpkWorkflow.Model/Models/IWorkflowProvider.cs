using System.Activities;
using System.Threading;
using KpkWorkflow.Model;

namespace Sparc.Kpk12.Domain
{
    public interface IWorkflowProvider
    {
        WorkflowApplication CreateApplication(Activity activity, params object[] extensions);
    }

    public class WorkflowProvider : IWorkflowProvider
    {
        private readonly SynchronizationContext _context;
        private readonly IEventBroker _eventBroker;
        private readonly ITrackingParticipant _tracking;
        private readonly IMessageAgent _agent;
        private readonly IEventAgent _eventAgent;

        public WorkflowProvider(SynchronizationContext context, IEventBroker eventBroker, ITrackingParticipant tracking, IMessageAgent agent, IEventAgent eventAgent)
        {
            _context = context;
            _eventBroker = eventBroker;
            _tracking = tracking;
            _agent = agent;
            _eventAgent = eventAgent;
        }

        public WorkflowApplication CreateApplication(Activity activity, params object[] extensions)
        {
            var workflow = new WorkflowApplication(activity)
            {
                SynchronizationContext = _context
            };
            workflow.Extensions.Add(_eventBroker);
            workflow.Extensions.Add(_tracking);
            workflow.Extensions.Add(_agent);
            workflow.Extensions.Add(_eventAgent);

            foreach (var extension in extensions)
                workflow.Extensions.Add(extension);

            return workflow;
        }
    }
}