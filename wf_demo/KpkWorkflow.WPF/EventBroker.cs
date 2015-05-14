using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using KpkWorkflow.Model;

namespace KpkWorkflow.WPF
{
    public class EventBroker : IEventBroker
    {
        private readonly List<IListener> _listeners = new List<IListener>();               

        public void Register(IListener listener)
        {
            _listeners.Add(listener);
        }

        public void Unregister(IListener listener)
        {
            _listeners.Remove(listener);
        }

        public void Publish<T>(T o) where T : IEvent
        {
            Debug.WriteLine("event of type:" + typeof (T));
            foreach (var listener in _listeners.OfType<IListener<T>>().ToList())
            {                
                listener.Handle(o);
            }
        }

        public void Publish(IEvent o)
        {
            foreach (var listener in _listeners)
            {
                var type = listener.GetType();
                var founded = type.GetInterfaces()
                                  .SingleOrDefault(i => i.GetGenericArguments()
                                                         .Contains(o.GetType()));
                if (founded == null)
                    continue;
                var method = type.GetMethod("Handle");
                method.Invoke(listener, new object[] {o});
            }
        }
    }
}