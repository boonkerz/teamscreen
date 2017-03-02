using System;
using LiteNetLib.Utils;

namespace Network.Messages.Connection
{
	public class MouseMoveMessage: BaseMessage
	{
		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public Double X { get; set; }
		public Double Y { get; set; }

		public MouseMoveMessage()
			: base((ushort)CustomMessageType.MouseMove)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(X);
			message.Put(Y);

		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			X = message.GetDouble();
			Y = message.GetDouble();
		}
	}
}
