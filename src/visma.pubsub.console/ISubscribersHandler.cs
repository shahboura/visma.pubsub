namespace visma.pubsub.console
{
    public interface ISubscribersHandler
    {
        void DisplaySubscribers();
        T AddSubscriber<T>()
            where T : Subscriber;
        void Subscribe(string name);
        void Unsubscribe(string name);
        void DeleteSubscriber(string name);
    }
}
