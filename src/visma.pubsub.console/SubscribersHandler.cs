using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace visma.pubsub.console
{
    public class SubscribersHandler : ISubscribersHandler
    {
        private readonly IEventBus _eventBus;
        private readonly List<Subscriber> _subscribers = new List<Subscriber>();
        private int _counter = 1;
        public IReadOnlyCollection<Subscriber> Subscribers => _subscribers.AsReadOnly();

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
            var subscriber = _subscribers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (subscriber == null)
            {
                Console.WriteLine($"No subscribers found matching name {name}");
                return;
            }

            Unsubscribe(subscriber);
            _subscribers.Remove(subscriber);
            GC.Collect();
            Console.WriteLine($"subscriber {name} deleted");
        }

        public void DisplaySubscribers()
        {
            if (!_subscribers.Any())
            {
                Console.WriteLine("No subscribers registered, add a few then try again later...");
                return;
            }

            _subscribers.ForEach(s => Console.WriteLine(
                    $"subscriber with name {s.Name} is {(!s.IsWired ? "not" : string.Empty)} wired"));
        }

        public void Subscribe(string name)
        {
            var subscriber = _subscribers.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (subscriber == null)
            {
                Console.WriteLine($"No subscribers found matching name {name}");
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
                Console.WriteLine($"No subscribers found matching name {name}");
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