using System;
using Network.Thread;

namespace ConsoleApp
{
    class Program
    {
        
        public static HostThread Manager { get { return Network.Instance.Host.Instance.Thread; } }
        
        static void Main(string[] args)
        {
            Manager.Events.onPeerConnected += delegate(object sender, EventArgs eventArgs)
            {
                Console.WriteLine("Host Connected");
            };
            
            Manager.Start();
            
            Console.WriteLine("Host started ...");

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
            }
        }
    }
}