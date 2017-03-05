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
		public Config.Manager ConfigManager { get; set; }

		public HostThread()
		{
			ConfigManager = new Config.Manager();
			HostListener = new HostListener();

			Manager = new HostManager(HostListener, "myapp1");
			if (ConfigManager.HostConfig.Password == null || ConfigManager.HostConfig.Password == "")
			{
				Manager.Password = new Random().Next(0, 9999).ToString();
			}
			else
			{
				Manager.Password = ConfigManager.HostConfig.Password;
			}
			Manager.UnsyncedEvents = true;
			HostListener._hostManager = Manager;
			Manager.MergeEnabled = true;
			Manager.PingInterval = 10000;
			Manager.DisconnectTimeout = 20000;
			HostListener.OnConnected += (object sender, ConnectedEventArgs e) =>
			{
				if (ConfigManager.HostConfig.SystemId == null || ConfigManager.HostConfig.SystemId == "")
				{
					ConfigManager.HostConfig.SystemId = e.SystemId;
					ConfigManager.HostConfig.Password = Manager.Password;
					ConfigManager.saveHostConfig();
				}
			};
		}

		public void start()
		{
            if (!Manager.Start())
			{
				return;
			}
			Manager.Connect("127.0.0.1", 9050);

			if (ConfigManager.HostConfig.SystemId == "")
			{
				Manager.sendMessage(new RequestHostIntroducerRegistrationMessage());
			}
			else
			{
				Manager.sendMessage(new RequestHostIntroducerRegistrationMessage { SystemId = ConfigManager.HostConfig.SystemId });
			}

		}

		public void stop()
		{
			Manager.Stop();
		}
	}
}
