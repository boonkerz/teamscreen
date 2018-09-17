using System;
using LiteNetLib;
using Network.Thread;

namespace TeamScreen.Broker.ConsoleApp
{
    class Program
    {
        public static BrokerThread Manager { get { return Network.Instance.Broker.Instance.Thread; } }
        
        static void Main(string[] args)
        {
			

			Manager.Events.onPeerConnected += delegate (object sender, EventArgs eventArgs)
			{
				NetPeer peer = (NetPeer)sender;
				Console.WriteLine("Peer connected: " + peer.EndPoint);
			};

			Manager.Start();


            // important!!!
            Console.TreatControlCAsInput = true;

            while (true)
            {
                Console.WriteLine("Use CTRL+C to exit");
                var input = Console.ReadKey();

                if (input.Key == ConsoleKey.C && input.Modifiers == ConsoleModifiers.Control)
                {
                    break;
                }
            }

        }
        
	}
}