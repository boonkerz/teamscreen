using System;
using LiteNetLib.Utils;
using Model;

namespace Messages.Connection.Response
{
	public class CloseHostConnectionMessage : BaseMessage
	{

		public CloseHostConnectionMessage()
			: base((ushort)CustomMessageType.ResponseCloseHostConnection)
		{
		}

	}
}
