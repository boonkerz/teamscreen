using System;
using Common;
using Common.Thread;

namespace ConsoleIntroducer
{
	class ConsoleIntroducer
	{
		public static IntroducerThread Manager { get { return Common.Instance.Introducer.Instance.Thread; } }

		static void Main(string[] args)
		{
			Manager.start();

			while (true)
			{
				if (Console.KeyAvailable)
				{
					var key = Console.ReadKey(true).Key;
					if (key == ConsoleKey.Escape)
					{
						break;
					}
					if (key == ConsoleKey.A)
					{

					}
				}
			}
		}
	}
}
