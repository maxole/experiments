using System.Activities.Tracking;
using System.Diagnostics;
using Sparc.Kpk12.Domain;

namespace KpkWorkflow.Model
{
    //using System.Activities;

    //public sealed class NotifyStartEnabledActivity : NativeActivity
    //{
    //    protected override void CacheMetadata(NativeActivityMetadata metadata)
    //    {
    //        metadata.RequireExtension<IWorkflowRunner>();
    //    }

    //    protected override void Execute(NativeActivityContext context)
    //    {

    //        context.Track(new CustomTrackingRecord("name", TraceLevel.Info));

    //        var model = context.GetExtension<IWorkflowRunner>();
    //        if (model.Executed != null)
    //            model.Executed();
    //    }
    //}
}
