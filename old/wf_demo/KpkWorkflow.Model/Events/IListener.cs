namespace KpkWorkflow.Model
{
    public interface IListener<in T> : IListener where T : IEvent
    {
        void Handle(T e);
    }

    public interface IListener
    {
    }
}