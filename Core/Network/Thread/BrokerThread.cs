using Network.Listener;

namespace Network.Thread
{
	public class BrokerThread
	{

		public BrokerListener Events { get; set; }
		public IntroducerManager Manager { get; set; }
		public Utils.Config.Manager ConfigManager { get; set; }

		public BrokerThread()
		{
			ConfigManager = new Utils.Config.Manager();
			Events = new BrokerListener();

			Manager = new IntroducerManager(Events, "teamscreen");
            
			Events.Server = Manager.getNetmanager();
		}

		public void Start()
		{
            if (!Manager.Start(9050))
			{
				return;
			}

		}

		public void Stop()
		{
			Manager.Stop();
		}
	}
}
