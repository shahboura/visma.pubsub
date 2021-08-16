using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace visma.pubsub.console
{
    public class SubscribersHandler : ISubscribersHandler
    {
        private readonly IEventBus _eventBus;
        private readonly Collection<Subscriber> _subscribers = new Collection<Subscriber>();
        private int _counter = 1;

        public SubscribersHandler(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public T AddSubscriber<T>()
            where T : Subscriber
        {
            var subscriber = Activator.CreateInstance<T>();
            subscriber.Name = $"{subscriber.GetType().Name} {_counter++}";
            _subscribers.Add(subscriber);
            Console.WriteLine($"subscriber {subscriber.Name} created");
            Subscribe(subscriber);

            return subscriber;
        }

        public void DeleteSubscriber(string name)
        {
            for (int i = _subscribers.Count - 1; i >= 0; i--)
            {
                var current = _subscribers[i];

                if (current.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    Unsubscribe(current);
                    _subscribers.RemoveAt(i);
                    Console.WriteLine($"subscriber {current.Name} deleted");
                }
            }

            GC.Collect();
        }

        public void DisplaySubscribers()
        {
            if (!_subscribers.Any())
            {
                Console.WriteLine("No subscribers registered, add a few then try again later...");
                return;
            }

            foreach (var subscriber in _subscribers)
            {
                Console.WriteLine(
                    $"subscriber with name {subscriber.Name} is {(!subscriber.IsWired ? "not" : string.Empty)} wired");
            }
        }

        public void Subscribe(string name)
        {
            var subscriber = _subscribers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (subscriber == null)
            {
                return;
            }

            Subscribe(subscriber);
        }

        private void Subscribe(Subscriber subscriber)
        {
            _eventBus.Subscribe(subscriber, subscriber.GetMessageType(), subscriber.GetDelegate());
            subscriber.IsWired = true;
            Console.WriteLine($"subscribed {subscriber.Name}");
        }

        public void Unsubscribe(string name)
        {
            var subscriber = _subscribers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (subscriber == null)
            {
                return;
            }

            Unsubscribe(subscriber);
        }

        private void Unsubscribe(Subscriber subscriber)
        {
            _eventBus.Unsubscribe(subscriber);
            subscriber.IsWired = false;
            Console.WriteLine($"unsubscribed {subscriber.Name}");
        }
    }
}