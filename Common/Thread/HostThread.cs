using System;
using System.Threading;
using System.Timers;
using Common.EventArgs.Network;
using Common.Listener;
using LiteNetLib;
using LiteNetLib.Utils;
using Network;
using Network.Messages.Connection;
using Network.Messages.System;

namespace Common.Thread
{
	public class HostThread
	{

		public HostListener HostListener { get; set; }
		public HostManager Manager { get; set; }

		public HostThread()
		{
			HostListener = new HostListener();

			Manager = new HostManager(HostListener, "myapp1");
			Manager.Password = "aA";
			Manager.UnsyncedEvents = true;
			HostListener._hostManager = Manager;
			Manager.MergeEnabled = true;
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

			Manager.sendMessage(new RequestHostIntroducerRegistrationMessage());

		}

		public void stop()
		{
			Manager.Stop();
		}		
	}
}
