using System;
using System.Threading;
using Common;

namespace ConsoleHost
{
	class MainClass
	{
		public static void Main(string[] args)
		{
			Thread.Sleep(2000);
			Host host = new Host();
			host.Run();
		}
	}
}
