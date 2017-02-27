using System;
using System.Threading;
using Common;

namespace ConsoleClient
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Thread.Sleep(5000);
			Client client = new Client();
			client.Run();
		}
	}
}
