using System.Collections.Generic;
using System;
using System.IO;
using System.Messaging;
using System.Linq;

namespace MessageQueueLib
{
    public static class DocumentQueue
    {
        private const int MessageMaxSize = 3145202;

        public static void Send(string path, string name)
        {
            var bytes = File.ReadAllBytes(path);
            using (MessageQueue queue = Create())
            {
                var id = Guid.NewGuid();

                var messageBodySize = MessageMaxSize - (name.Length * sizeof(char));
                var messages = new List<FileMessage>();
                while (bytes.Length > messageBodySize)
                {
                    var body = bytes.Take(messageBodySize).ToArray();
                    messages.Add(FileMessage.Create(id, name, body));
                    bytes = bytes.Skip(messageBodySize).ToArray();
                }
                messages.Add(FileMessage.Create(id, name, bytes));

                for (int i = 0; i < messages.Count; i++)
                {
                    messages[i].Position = i;
                    messages[i].Count = messages.Count;
                    queue.Send(new Message(messages[i]));
                }
            }
        }

        public static void Listen(string path)
        {
            var messageCash = new Dictionary<Guid, List<FileMessage>>();
            using (MessageQueue queue = Create())
            {
                queue.Formatter = new XmlMessageFormatter(new Type[] { typeof(FileMessage) });
                while (true)
                {
                    Message msg = queue.Receive();
                    var message = (FileMessage)msg.Body;

                    if (!messageCash.ContainsKey(message.Id))
                    {
                        messageCash.Add(message.Id, new List<FileMessage>());
                    }

                    messageCash[message.Id].Add(message);

                    if (message.Count == messageCash[message.Id].Count)
                    {
                        var bytes = messageCash[message.Id].OrderBy(m => m.Position).SelectMany(m => m.Body).ToArray();
                        File.WriteAllBytes(Path.Combine(path, message.Name), bytes);
                    }
                }
            }
        }

        private static MessageQueue Create()
        {
            var queue = new MessageQueue
            {
                Path = @".\private$\documentqueue"
            };
            if (!MessageQueue.Exists(queue.Path))
            {
                MessageQueue.Create(queue.Path);
            }

            return queue;
        }
    }
}
