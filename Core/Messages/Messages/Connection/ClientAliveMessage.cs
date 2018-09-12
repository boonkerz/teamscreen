
using System;
using LiteNetLib.Utils;
using Messages.Connection;

namespace Messages.Connection
{
	public class ClientAliveMessage : BaseMessage
	{

		public ClientAliveMessage()
			: base((ushort)CustomMessageType.ClientAlive)
		{
     	}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
        }

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
		}
        

	}
}
