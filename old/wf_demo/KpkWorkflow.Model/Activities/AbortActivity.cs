using System.Activities;
using System.Diagnostics;

namespace KpkWorkflow.Model
{
    public class AbortActivity : NativeActivity
    {
        protected override void Execute(NativeActivityContext context)
        {
            Debug.WriteLine("abort executing ...");
        }
    }
}