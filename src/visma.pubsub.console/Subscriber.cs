using System;

namespace visma.pubsub.console
{
    public abstract class Subscriber
    {
        public string Name { get; set; }
        public bool IsWired { get; set; }
        public abstract Type GetMessageType();
        public abstract Delegate GetDelegate();
    }

    public abstract class Subscriber<T> : Subscriber
    {
        public override Type GetMessageType()
        {
            return typeof(T);
        }

        public override Delegate GetDelegate()
        {
            Action<T> action = Action;
            return action;
        }

        public abstract void Action (T data);
    }
}
