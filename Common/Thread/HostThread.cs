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

			Manager = new HostManager(HostListener, "teamscreen");
			if (ConfigManager.HostConfig.Password == null || ConfigManager.HostConfig.Password == "")
			{
				Manager.Password = new Random().Next(0, 9999).ToString();
			}
			else
			{
				Manager.Password = ConfigManager.HostConfig.Password;
			}
			HostListener._hostManager = Manager;

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

		public void Start()
		{
            if (!Manager.Start())
			{
				return;
			}
			Manager.Connect(ConfigManager.HostConfig.ServerName, ConfigManager.HostConfig.ServerPort);

			if (ConfigManager.HostConfig.SystemId == "")
			{
				Manager.sendMessage(new RequestHostIntroducerRegistrationMessage());
			}
			else
			{
				Manager.sendMessage(new RequestHostIntroducerRegistrationMessage { SystemId = ConfigManager.HostConfig.SystemId });
			}

		}

		public void Stop()
		{
			Manager.Stop();
		}

        public void Reconnect()
        {
            Manager.Connect(ConfigManager.HostConfig.ServerName, ConfigManager.HostConfig.ServerPort);

            if (ConfigManager.HostConfig.SystemId == "")
            {
                Manager.sendMessage(new RequestHostIntroducerRegistrationMessage());
            }
            else
            {
                Manager.sendMessage(new RequestHostIntroducerRegistrationMessage { SystemId = ConfigManager.HostConfig.SystemId });
            }
        }
    }
}
