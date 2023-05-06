using System;

namespace MessageQueueLib
{
    public class FileMessage
    {
        public Guid Id { get; set; }

        public int Position { get; set; }

        public int Count { get; set; }

        public string Name { get; set; }

        public byte[] Body { get; set; }

        public static FileMessage Create(Guid id, string name, byte[] bytes)
        {
            return new FileMessage
            {
                Id = id,
                Name = name,
                Body = bytes
            };
        }
    }
}
