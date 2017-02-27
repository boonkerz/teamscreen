using System;
using Common;

namespace ConsoleIntroducer
{
	class ConsoleIntroducer
	{
		static void Main(string[] args)
		{
			IntroducerServer ic = new IntroducerServer();
			ic.Run();

		}
	}
}
