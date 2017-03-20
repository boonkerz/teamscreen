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
	public class IntroducerThread
	{

		public IntroducerListener IntroducerListener { get; set; }
		public IntroducerManager Manager { get; set; }
		public Config.Manager ConfigManager { get; set; }

		public IntroducerThread()
		{
			ConfigManager = new Config.Manager();
			IntroducerListener = new IntroducerListener();

			Manager = new IntroducerManager(IntroducerListener, "teamscreen");
			IntroducerListener.Server = Manager.getNetmanager();
		}

		public void start()
		{
            if (!Manager.Start(9050))
			{
				return;
			}

		}

		public void stop()
		{
			Manager.Stop();
		}
	}
}
