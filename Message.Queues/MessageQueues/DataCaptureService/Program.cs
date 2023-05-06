using MessageQueueLib;
using System;
using System.IO;

namespace DataCaptureService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileSystemWatcher watcher = new FileSystemWatcher
            {
                Path = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "Input"),
                Filter = "*.*"
            };
            watcher.Created += new FileSystemEventHandler(FileCopied);
            watcher.EnableRaisingEvents = true;
            Console.ReadLine();
        }

        private static void FileCopied(object sender, FileSystemEventArgs e)
        {
            WaitForCopying(e.FullPath);
            DocumentQueue.Send(e.FullPath, e.Name);
        }

        private static void WaitForCopying(string path)
        {
            var count = 0;
            while (true) 
            {
                try
                {
                    using (File.Open(path, FileMode.Open, FileAccess.ReadWrite))
                    break;
                }
                catch (IOException)
                {
                    count++;
                }
            }
        }
    }
}
