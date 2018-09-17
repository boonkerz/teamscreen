    using System;
using Network.Thread;

namespace ConsoleApp
{
    class Program
    {
        public static ClientThread Manager { get { return Network.Instance.Client.Instance.Thread; } }
        
        static void Main(string[] args)
        {
            Manager.Events.onPeerConnected += delegate(object sender, EventArgs eventArgs)
            {
                Console.WriteLine("Client Connected");
            };
            
            Manager.Start();
            
            Console.WriteLine("Client started ...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.Escape)
                    {
                        break;
                    }
                }
                System.Threading.Thread.Sleep(10);
            }
        }
    }
}