using System;
using System.Activities;

namespace KpkWorkflow.Model
{
    //public class MessageNotificationActivity : NotificationActivityBase
    //{
    //    public InArgument<Type> EventType { get; set; }        

    //    protected override void CacheMetadata(NativeActivityMetadata metadata)
    //    {
    //        base.CacheMetadata(metadata);

    //        var eventType = new RuntimeArgument("EventType", typeof(Type), ArgumentDirection.In);
    //        metadata.AddArgument(eventType);
    //        metadata.Bind(EventType, eventType);
    //    }

    //    protected override void Execute(NativeActivityContext context)
    //    {
    //        var broker = context.GetExtension<IEventBroker>();
    //        try
    //        {
    //            var key = Key.Get<string>(context);
    //            var eventType = EventType.Get(context);
    //            broker.Publish(new UserNotificationEvent2(eventType, key));
    //            var name = BookmarkName.Get(context);
    //            context.CreateBookmark(name, OnResumeBookmark);
    //        }
    //        catch (Exception exception)
    //        {
    //            broker.Publish(new ExceptionEvent(exception));
    //        }
    //    }
    //}
}