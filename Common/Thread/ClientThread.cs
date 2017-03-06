using System;
using System.Threading;
using Common.Listener;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;
using Network.Messages.Connection;
using Network.Messages.System;

namespace Common
{
	public class ClientThread
	{
		
		public ClientListener ClientListener { get; set; }
		public ClientManager Manager { get; set; }

		public ClientThread()
		{
			ClientListener = new ClientListener();

			Manager = new ClientManager(ClientListener, "myapp1");
			Manager.UnsyncedEvents = true;
			ClientListener._clientManager = Manager;
			Manager.ReliableResendTime = 1000;
			Manager.PingInterval = 10000;
			Manager.DisconnectTimeout = 20000;
		}

		public void start()
		{
			if (!Manager.Start())
			{
				return;
			}
			Manager.Connect("127.0.0.1", 9050);

			Manager.sendMessage(new RequestClientIntroducerRegistrationMessage());

		}

		public void stop()
		{
			Manager.Stop();
		}

	}
}
