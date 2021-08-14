using System;

namespace visma.pubsub
{
    internal class Subscriber
    {
        public WeakReference Context { get; set; }

        public Delegate Action { get; set; }
    }
}