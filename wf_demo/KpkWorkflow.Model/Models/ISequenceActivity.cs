using System;
using System.Activities;
using KpkWorkflow.Model;

namespace Sparc.Kpk12.Domain
{
    public interface ISequenceActivity
    {
        void Next();
        void Skip();
        void Cancel();
    }

    public class SequenceActivity : ISequenceActivity
    {
        private readonly WorkflowApplication _application;

        public SequenceActivity(WorkflowApplication application)
        {
            //if(_application == null)
            //    throw new NullReferenceException("application");
            _application = application;
        }

        public void Next()
        {
            SetBookmarkState(ActivityState.Next);
        }

        public void Skip()
        {
            SetBookmarkState(ActivityState.Skip);
        }

        public void Cancel()
        {
            SetBookmarkState(ActivityState.Cancel);
        }

        private void SetBookmarkState(ActivityState state)
        {
            foreach (var bookmark in _application.GetBookmarks())
                _application.ResumeBookmark(bookmark.BookmarkName, state);
        }
    }
}