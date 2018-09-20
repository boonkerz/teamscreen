using System;
using LiteNetLib;
using Network.Thread;

namespace ConsoleApp
{
    class Program
    {
        
        public static HostThread thread { get { return Network.Instance.Host.Instance.Thread; } }
        
        static void Main(string[] args)
        {
            thread.Events.onPeerConnected += delegate(object sender, EventArgs eventArgs)
            {
                Console.WriteLine("Host Connected: " + thread.Manager.SystemId + " Password: " + thread.Manager.Password);
            };

            thread.Start();
            
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