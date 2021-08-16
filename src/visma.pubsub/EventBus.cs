using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;

namespace visma.pubsub
{
    public class EventBus : IEventBus
    {
        private readonly ConcurrentDictionary<Type, Collection<Subscriber>> _subscribers =
            new ConcurrentDictionary<Type, Collection<Subscriber>>();

        public void Publish<T>(T data)
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var subscribers))
            {
                return;
            }

            for (int i = subscribers.Count - 1; i >= 0; i--)
            {
                var current = subscribers[i];

                if (current.Context.IsAlive)
                {
                    ((Action<T>)current.Action)(data);
                    continue;
                }

                subscribers.RemoveAt(i);
            }
        }

        public void UnPublish<T>()
        {
            _subscribers.TryRemove(typeof(T), out Collection<Subscriber> subscribers);
            subscribers.Clear();
        }

        public void Subscribe(object context, Type type, Delegate action)
        {
            var subscriber = new Subscriber
            {
                Context = new WeakReference(context),
                Action = action
            };

            _subscribers.AddOrUpdate(type,
                new Collection<Subscriber> { subscriber },
                (k, v) =>
                {
                    v.Add(subscriber); return v;
                });
        }

        public void Subscribe<T>(object context, Action<T> action)
        {
            Subscribe(context, typeof(T), action);
        }

        public void Unsubscribe(object context)
        {
            foreach (var subscribers in _subscribers.Values)
            {
                for (int i = subscribers.Count - 1; i >= 0; i--)
                {
                    var current = subscribers[i];

                    if (!current.Context.IsAlive || current.Context.Target == context)
                    {
                        subscribers.RemoveAt(i);
                    }
                }
            }
        }
    }
}