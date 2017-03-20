using System;
using Common.Thread;

namespace Common.Instance
{
	public class Introducer
	{
		#region Singleton Design Pattern Implementation

		private static volatile Introducer instance;
		private static readonly object syncRoot = new Object();

		public static Introducer Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRoot)
					{
						if (instance == null)
							instance = new Introducer();
					}
				}

				return instance;
			}
		}

		#endregion

		public IntroducerThread Thread { get; private set; }

		private Introducer()
		{
			Thread = new IntroducerThread();
		}

	}
}
