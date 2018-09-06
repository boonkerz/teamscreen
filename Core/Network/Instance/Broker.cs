using System;
using Network.Thread;

namespace Network.Instance
{
	public class Broker
	{
		#region Singleton Design Pattern Implementation

		private static volatile Broker instance;
		private static readonly object syncRoot = new Object();

		public static Broker Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new Broker();
					}
				}

				return instance;
			}
		}

		#endregion

		public BrokerThread Thread { get; private set; }

		private Broker()
		{
			Thread = new BrokerThread();
		}

	}
}
