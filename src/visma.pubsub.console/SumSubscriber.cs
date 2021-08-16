using System;

namespace visma.pubsub.console
{
    public class SumSubscriber : Subscriber<int>
    {
        public int Data { get; private set; }

        public override void Action(int data)
        {
            Data += data;
            Console.WriteLine($"Sum is {Data} from {Name}");
        }
    }
}