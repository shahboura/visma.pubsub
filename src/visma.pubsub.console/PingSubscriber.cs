using System;

namespace visma.pubsub.console
{
    public class PingSubscriber : Subscriber<string>
    {
        public override void Action(string data)
        {
            Console.WriteLine($"Pinging back {data} from {Name}");
        }
    }
}
