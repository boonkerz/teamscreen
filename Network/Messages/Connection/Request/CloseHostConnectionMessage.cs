using System;
using LiteNetLib.Utils;
using Model;

namespace Network.Messages.Connection.Request
{
	public class CloseHostConnectionMessage : BaseMessage
	{
        public CloseHostConnectionMessage()
			: base((ushort)CustomMessageType.RequestCloseHostConnection)
		{
		}
	}
}
