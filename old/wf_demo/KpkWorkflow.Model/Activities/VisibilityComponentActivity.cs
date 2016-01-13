using System;
using System.Activities;

namespace KpkWorkflow.Model
{
    //public class VisibilityComponentActivity : NativeActivity
    //{
    //    public InArgument<Type> Type { get; set; }
    //    public InArgument<bool> Visible { get; set; }

    //    protected override void CacheMetadata(NativeActivityMetadata metadata)
    //    {
    //        metadata.RequireExtension<IEventBroker>();
    //        var type = new RuntimeArgument("Type", typeof(Type), ArgumentDirection.In);
    //        metadata.AddArgument(type);
    //        metadata.Bind(Type, type);

    //        var visible = new RuntimeArgument("Visible", typeof(bool), ArgumentDirection.In);
    //        metadata.AddArgument(visible);
    //        metadata.Bind(Visible, visible);
    //    }

    //    protected override void Execute(NativeActivityContext context)
    //    {
    //        var broker = context.GetExtension<IEventBroker>();
    //        try
    //        {
                
    //            var type = Type.Get(context);
    //            var visible = Visible.Get(context);
    //            broker.Publish(new VisibilityComponentEvent(type, visible));
    //        }
    //        catch (Exception exception)
    //        {
    //            broker.Publish(new ExceptionEvent(exception));
    //        }
    //    }
    //}
}