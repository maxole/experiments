using System.Activities;

namespace Sparc.Kpk12.Domain
{    
    public interface IWorkflowRunner
    {
        void Run(Activity activity);
        void Cancel();
        void Abort();
        ISequenceActivity CreateSequence();
    }
}
