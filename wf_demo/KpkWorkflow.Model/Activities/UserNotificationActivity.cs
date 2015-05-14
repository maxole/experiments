using System;
using System.Activities;

namespace KpkWorkflow.Model
{
    //public class UserNotificationActivity : NotificationActivityBase
    //{        
    //    protected override void Execute(NativeActivityContext context)
    //    {
    //        var broker = context.GetExtension<IEventBroker>();
    //        try
    //        {                
    //            var key = Key.Get<string>(context);            
    //            broker.Publish(new UserNotificationEvent(key));
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