using System;
using Network.Utils;
using Model;

namespace Network.Messages.System
{
	public class RequestHostDisconnectMessage : BaseMessage
	{

		public RequestHostDisconnectMessage()
			: base((ushort)CustomMessageType.RequestHostDisconnect)
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
