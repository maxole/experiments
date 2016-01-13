using System;

namespace SharedAgent.Impl
{
    public class Transient<T> : IDisposable
    {
        private readonly IItem<T> _item;

        public T Value
        {
            get { return _item.Value; }
        }

        internal Transient(IItem<T> item)
        {
            _item = item;
        }

        internal bool Reset()
        {
            return _item.Reset();
        }

        public void Dispose()
        {
            _item.Set();
        }
    }
}