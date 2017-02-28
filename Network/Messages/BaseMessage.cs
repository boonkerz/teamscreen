using System;
using System.Collections.Generic;
using Network.Messages.System;

namespace Network
{
	public abstract class BaseMessage : Message
	{
		
		protected BaseMessage(ushort type)
			: base(type)
		{
		}


		static BaseMessage()
		{
		}

	}
}
