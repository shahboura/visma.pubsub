using System;

namespace visma.pubsub
{
    public interface IEventBus
    {
        void Publish<T>(T data);
        void Subscribe<T>(object context, Action<T> action);
    }
}
