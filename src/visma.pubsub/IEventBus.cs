using System;

namespace visma.pubsub
{
    public interface IEventBus
    {
        void Publish<T>(T data);
        void UnPublish<T>();
        void Subscribe<T>(object context, Action<T> action);
        void Unsubscribe(object context);
    }
}
