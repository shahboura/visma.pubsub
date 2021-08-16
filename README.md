# visma.pubsub

## Running the console

This is a simple console application that allows you to send and receive messages to and from a pubsub module.

list of commands:

- `--help` displays the help message
- `{verb} --help` displays the help message for the given verb
- `add` adds a new subscriber
  - `-s, --sum` Add new sum subscriber (listens to integer topics and adds them up)
  - `-p, --ping` Add new ping subscriber (listens to string topics and echo them back)
- `subscribe` subscribe / unsubscribe to a topic or delete a subscriber
  - `-n, --name` Subscriber's name to use.
  - `-a, --add` Adds subscription by name.
  - `-r, --remove` Removes subscription by name while keeping instance alive.
  - `-d, --delete` Deletes subscriber entirely by name.
- `list` lists all subscribers
- `publish` publishes a message to a topic
  - `-i, --input` Input topic to publish
  This input can be in the form of integer or string. If parsing as integer fails, it will be parsed as string.
- `quit` quits the console
