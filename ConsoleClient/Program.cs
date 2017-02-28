using System;
using System.Threading;
using Common;
using Common.EventArgs.Network;

namespace ConsoleClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			ClientThread Manager = Common.Instance.Client.Instance.Thread;
			Manager.ClientListener.OnConnected += delegate (object sender, ConnectedEventArgs e)
			{
				Console.WriteLine(e.SystemId);
			};
			Manager.start();

			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
					{
						Manager.stop();
					}
					if (key == ConsoleKey.A)
					{

					}
				}
				Thread.Sleep(15);
			}
		}
	}
}
