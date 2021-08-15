using CommandLine;
using System;

namespace visma.pubsub.console
{
    class Program
    {
        [Verb("add", HelpText = "Add new subscriber.")]
        public class AddOptions
        {
            [Option('s', "sum", Required = false, HelpText = "Add new sum subscriber.")]
            public bool Sum { get; set; }

            [Option('p', "ping", Required = false, HelpText = "Add new sum subscriber.")]
            public bool Ping { get; set; }
        }

        [Verb("subscribe", HelpText = "Subscribe / Unsubscribe to publishers.")]
        public class UnsubscribeOptions
        {
            [Option('n', "name", Required = true, HelpText = "Subscriber's name to remove.")]
            public string Name { get; set; }

            [Option('r', "remove", HelpText = "Removes subscription by name while keeping instance alive.")]
            public bool Unsubscribe { get; set; }

            [Option('d', "delete", HelpText = "Deletes subscriber entirely by name.")]
            public bool Delete { get; set; }
        }

        [Verb("list", HelpText = "List all subscribers.")]
        public class ListOptions
        {
        }

        [Verb("publish", HelpText = "Publish int or string to subscribers.")]
        public class PublishOptions
        {
            [Option('i', "input", HelpText = "Input to publish.")]
            public string Input { get; set; }
        }

        [Verb("quit", HelpText = "Simply Quits.")]
        public class QuitOptions
        {
        }

        static void Main(string[] args)
        {
            Console.WriteLine("For list of available commands and parameters, type `--help`");
            string input;
            do
            {
                input = Console.ReadLine();

                if ("q".Equals(input) || "quit".Equals(input))
                    break;

                args = input.Split(' ');
                Parser.Default.ParseArguments<AddOptions, UnsubscribeOptions, ListOptions, PublishOptions, QuitOptions>(args)
                    .WithParsed<AddOptions>(o => Console.WriteLine("Add chosen"))
                    .WithParsed<UnsubscribeOptions>(o => Console.WriteLine("Subscribe chosen"))
                    .WithParsed<ListOptions>(o => Console.WriteLine("List chosen"))
                    .WithParsed<PublishOptions>(o => Console.WriteLine("Publish chosen"))
                    .WithParsed<QuitOptions>(o => Console.WriteLine("Quit chosen"));
            }
            while (input != null);
        }
    }
}
