using System;
using System.Collections.Generic;
using System.Threading;
using Common.Listener;
using LiteNetLib;
using LiteNetLib.Utils;
using Model;
using Network;
using Network.Messages.System;

namespace Common
{
	public class IntroducerServer
	{
		
		private IntroducerListener _introduceListener;

		public void Run()
		{
			_introduceListener = new IntroducerListener();
			NetManager server = new NetManager(_introduceListener, 12000, "myapp1");
			server.UnsyncedEvents = true;
			//server.PingInterval = 10000;
			//server.DisconnectTimeout = 20000;
			if (!server.Start(9050))
			{
				Console.WriteLine("Server start failed");
				Console.ReadKey();
				return;
			}
			_introduceListener.Server = server;

			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
					{
						break;
					}
					if (key == ConsoleKey.A)
					{
						
					}
				}
				//server.PollEvents();
				//Thread.Sleep(15);
			}

			server.Stop();
		}
	}
}
