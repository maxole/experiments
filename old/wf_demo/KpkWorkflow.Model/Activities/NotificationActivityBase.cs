using System.Activities;

namespace KpkWorkflow.Model
{
    //public abstract class NotificationActivityBase : NativeActivity<ActivityState>
    //{
    //    public InArgument<string> Key { get; set; }
    //    [RequiredArgument]
    //    public InArgument<string> BookmarkName { get; set; }

    //    protected override void CacheMetadata(NativeActivityMetadata metadata)
    //    {
    //        metadata.RequireExtension<IEventBroker>();

    //        var key = new RuntimeArgument("Key", typeof(string), ArgumentDirection.In);
    //        metadata.AddArgument(key);
    //        metadata.Bind(this.Key, key);

    //        var bookmarkName = new RuntimeArgument("BookmarkName", typeof(string), ArgumentDirection.In);
    //        metadata.AddArgument(bookmarkName);
    //        metadata.Bind(this.BookmarkName, bookmarkName);
    //    }        

    //    protected override bool CanInduceIdle
    //    {
    //        get { return true; }
    //    }

    //    public void OnResumeBookmark(NativeActivityContext context, Bookmark bookmark, object obj)
    //    {
    //        Result.Set(context, (ActivityState)obj);
    //    }
    //}
}