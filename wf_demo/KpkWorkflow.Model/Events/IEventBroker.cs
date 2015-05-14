namespace KpkWorkflow.Model
{
    public interface IEventBroker
    {
        void Register(IListener listener);
        void Unregister(IListener listener);
        void Publish<T>(T o) where T : IEvent;
        void Publish(IEvent o);
    }
}
