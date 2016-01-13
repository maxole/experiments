namespace KpkWorkflow.Model
{
    public interface IEvent
    {
    }

    public interface IEvent<T> : IEvent where T : IPrototype
    {
        T Message { get; set; }
    }
}