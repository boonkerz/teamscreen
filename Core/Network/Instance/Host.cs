using System;
using Network.Thread;

namespace Network.Instance
{
	public class Host
	{
		#region Singleton Design Pattern Implementation

		private static volatile Host instance;
		private static readonly object syncRoot = new Object();

		public static Host Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new Host();
					}
				}

				return instance;
			}
		}

		#endregion

		public HostThread Thread { get; private set; }

		private Host()
		{
			Thread = new HostThread();
		}

	}
}
