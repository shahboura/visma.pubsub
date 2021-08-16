using CommandLine;
using System;
using System.Text.RegularExpressions;

namespace visma.pubsub.console
{
    class Program
    {
        [Verb("add", HelpText = "Creates and add new subscriber.")]
        public class AddOptions
        {
            [Option('s', "sum", HelpText = "Add new sum subscriber.")]
            public bool Sum { get; set; }

            [Option('p', "ping", HelpText = "Add new sum subscriber.")]
            public bool Ping { get; set; }
        }

        [Verb("subscribe", HelpText = "Subscribe / Unsubscribe to publishers.")]
        public class SubscribeOptions
        {
            [Option('n', "name", Required = true, HelpText = "Subscriber's name to remove.")]
            public string Name { get; set; }

            [Option('a', "add", SetName = "subscription", HelpText = "Adds subscription by name.")]
            public bool Subscribe { get; set; }

            [Option('r', "remove", SetName = "subscription", HelpText = "Removes subscription by name while keeping instance alive.")]
            public bool Unsubscribe { get; set; }

            [Option('d', "delete", SetName = "subscription", HelpText = "Deletes subscriber entirely by name.")]
            public bool Delete { get; set; }
        }

        [Verb("list", HelpText = "List all subscribers.")]
        public class ListOptions
        {
        }

        [Verb("publish", HelpText = "Publish int or string to subscribers.")]
        public class PublishOptions
        {
            [Option('i', "input", Required = true, HelpText = "Input to publish.")]
            public string Input { get; set; }
        }

        [Verb("quit", HelpText = "Simply Quits.")]
        public class QuitOptions
        {
        }

        static void Main(string[] args)
        {
            IEventBus eventBus = new EventBus();
            ISubscribersHandler handler = new SubscribersHandler(eventBus);
            string input;
            Console.WriteLine("For list of available commands and parameters, type `--help`");
            var argPattern = @"(?:-+([^= \'\""]+)[= ]?)?(?:([\'\""])([^\2]+?)\2|([^- \""\']+))?";

            do
            {
                input = Console.ReadLine();

                if ("q".Equals(input) || "quit".Equals(input))
                    break;

                //args = input.Split(' ');
                var matchedArgs = Regex.Matches(input, argPattern, RegexOptions.Singleline);
                args = new string[matchedArgs.Count];
                for (int i = 0; i < matchedArgs.Count; i++)
                {
                    args[i] = matchedArgs[i].ToString().Replace("\"", string.Empty);
                }

                Parser.Default.ParseArguments<AddOptions, SubscribeOptions, ListOptions, PublishOptions, QuitOptions>(args)
                    .WithParsed<AddOptions>(o =>
                    {
                        if (o.Sum)
                            handler.AddSubscriber<SumSubscriber>();

                        if (o.Ping)
                            handler.AddSubscriber<PingSubscriber>();
                    })
                    .WithParsed<SubscribeOptions>(o =>
                    {
                        o.Name = o.Name.Trim();

                        if (o.Subscribe)
                            handler.Subscribe(o.Name);

                        if (o.Unsubscribe)
                            handler.Unsubscribe(o.Name);

                        if (o.Delete)
                            handler.DeleteSubscriber(o.Name);
                    })
                    .WithParsed<ListOptions>(o => handler.DisplaySubscribers())
                    .WithParsed<PublishOptions>(o =>
                    {
                        o.Input = o.Input.Trim();

                        if (int.TryParse(o.Input, out int output))
                        {
                            eventBus.Publish(output);
                        }
                        else
                        {
                            eventBus.Publish(o.Input);
                        }
                    })
                    .WithParsed<QuitOptions>(o => Console.WriteLine("Quit chosen"));
            }
            while (input != null);
        }
    }
}