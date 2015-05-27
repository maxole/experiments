using System;
using System.Collections.Generic;

namespace SharedAgent.Impl
{
    public interface ITransientAgent
    {
        void Set<T>(T a);
        bool TryGet<T>(out Transient<T> transient);
    }

    public class TransientAgent : ITransientAgent
    {
        private readonly Dictionary<Type, IItem> _dictionary;

        public TransientAgent()
        {
            _dictionary = new Dictionary<Type, IItem>();
        }

        public void Set<T>(T a)
        {
            var type = typeof (T);
            if (_dictionary.ContainsKey(type))
                throw new DuplicateKeyException();
            _dictionary.Add(type, new Item<T>(a));
        }

        public bool TryGet<T>(out Transient<T> transient)
        {
            transient = null;
            var value = _dictionary[typeof (T)];
            var b = value.TryReset();
            if (!b)
                return false;
            transient = new Transient<T>((IItem<T>) value);
            return transient.Reset();
        }
    }
}