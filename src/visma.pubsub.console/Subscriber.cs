namespace visma.pubsub.console
{
    public abstract class Subscriber<T>
    {
        public string Name { get; set; }
        public bool IsWired { get; set; }
        public T Data { get; set; }

        public abstract void Action(T data);
    }
}
