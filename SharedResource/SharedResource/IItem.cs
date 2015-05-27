using System.Threading;

namespace SharedAgent.Impl
{
    internal interface IItem
    {
        bool Set();
        bool Reset();
        bool TryReset();
    }

    internal interface IItem<out T> : IItem
    {
         T Value { get; }
    }

    internal class Item<T> : IItem<T>
    {
        public T Value { get; private set; }

        private readonly ManualResetEvent _event;

        public Item(T a)
        {
            Value = a;
            _event = new ManualResetEvent(true);
        }

        public bool Set()
        {
            return _event.Set();
        }

        public bool Reset()
        {
            return _event.Reset();
        }

        public bool TryReset()
        {
            return _event.WaitOne(0);
        }
    }
}