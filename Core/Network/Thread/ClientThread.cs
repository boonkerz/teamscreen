using Messages.System;
using Network.Listener;
using Network.Manager;

namespace Network.Thread
{
	public class ClientThread
	{
		
		public ClientListener Events { get; set; }
		public ClientManager Manager { get; set; }
		public Utils.Config.Manager ConfigManager { get; set; }

		public ClientThread()
		{
			ConfigManager = new Utils.Config.Manager();
			Events = new ClientListener();
            Manager = new ClientManager(Events, "teamscreen");
			Events.SetManager(Manager);
        }

		public void Start()
		{
			if (!Manager.Start())
			{
				return;
			}
			Manager.Connect(ConfigManager.ClientConfig.ServerName, ConfigManager.ClientConfig.ServerPort);

			Manager.sendMessage(new RequestClientIntroducerRegistrationMessage());

		}

        public void Reconnect()
        {
            ConfigManager.Reload();
            Manager.Connect(ConfigManager.ClientConfig.ServerName, ConfigManager.ClientConfig.ServerPort);
            Manager.sendMessage(new RequestClientIntroducerRegistrationMessage());
        }

		public void Stop()
		{
			Manager.Stop();
		}

	}
}
