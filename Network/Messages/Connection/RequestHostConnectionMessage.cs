using System;
using LiteNetLib.Utils;
using Model;

namespace Network.Messages.Connection
{
	public class RequestHostConnectionMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public String Password { get; set; }

		public RequestHostConnectionMessage()
			: base((ushort)CustomMessageType.RequestHostConnection)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(Password);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			Password = message.GetString(100);
		}

	}
}
