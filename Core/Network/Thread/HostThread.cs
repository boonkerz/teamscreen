
using System;
using Messages.EventArgs.Network;
using Messages.System;
using Network.Listener;
using Network.Manager;

namespace Network.Thread
{
	public class HostThread
	{

		public HostListener Events { get; set; }
		public HostManager Manager { get; set; }
		public Utils.Config.Manager ConfigManager { get; set; }
        protected bool _Running = false;

		public HostThread()
		{
			ConfigManager = new Utils.Config.Manager();
			Events = new HostListener();

			Manager = new HostManager(Events, "teamscreen");

			if (ConfigManager.HostConfig.Password == null || ConfigManager.HostConfig.Password == "")
			{
				Manager.Password = new Random().Next(0, 9999).ToString();
			}
			else
			{
				Manager.Password = ConfigManager.HostConfig.Password;
			}
			Events.SetManager(Manager);

			Events.OnConnected += (object sender, ConnectedEventArgs e) =>
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
			ConfigManager.Reload();
			Manager.Connect(ConfigManager.HostConfig.ServerName, ConfigManager.HostConfig.ServerPort);

			if (ConfigManager.HostConfig.SystemId == "")
			{
				Manager.sendMessage(new RequestHostIntroducerRegistrationMessage());
			}
			else
			{
				Manager.sendMessage(new RequestHostIntroducerRegistrationMessage { SystemId = ConfigManager.HostConfig.SystemId });
			}
            _Running = true;
        }

        public bool IsRunning()
        {
            return this._Running;
        }

        public void Loop()
		{
            while(_Running) {
                System.Threading.Thread.Sleep(1000);
            }
		}

		public void Stop()
		{
			Manager.Stop();
            _Running = false;
		}

        public void Reconnect()
        {
            Stop();
            Start();
        }
    }
}
