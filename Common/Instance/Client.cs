using System;
using Common.Thread;

namespace Common.Instance
{
	public class Client
	{
		#region Singleton Design Pattern Implementation

		private static volatile Client instance;
		private static readonly object syncRoot = new Object();

		public static Client Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new Client();
					}
				}

				return instance;
			}
		}

		#endregion

		public ClientThread Thread { get; private set; }

		private Client()
		{
			Thread = new ClientThread();
		}

	}
}
