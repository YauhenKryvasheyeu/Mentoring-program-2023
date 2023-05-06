using MessageQueueLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Messaging;

namespace MainProcessingService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DocumentQueue.Listen(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Output"));
        }
    }
}
