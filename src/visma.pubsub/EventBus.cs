using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace visma.pubsub
{
    public class EventBus
    {
        private readonly ConcurrentDictionary<Type, List<Subscriber>> _subscribers =
            new ConcurrentDictionary<Type, List<Subscriber>>();

        public void Publish<T>(T data)
        {
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var subscribers))
            {
                return;
            }

            subscribers.RemoveAll(s => !s.Context.IsAlive);
            subscribers.ForEach(s => ((Action<T>)s.Action)(data));
        }

        public void Subscribe<T>(object context, Action<T> action)
        {
            var type = typeof(T);
            var subscriber = new Subscriber
            {
                Context = new WeakReference(context),
                Action = action
            };

            _subscribers.AddOrUpdate(type,
                new List<Subscriber> { subscriber },
                (k, v) =>
                {
                    v.Add(subscriber); return v;
                });
        }
    }
}
