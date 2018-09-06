using System;
using LiteNetLib.Utils;

namespace Messages.Connection.OneWay
{
	public class MouseMoveMessage: BaseMessage
	{
		public Double X { get; set; }
		public Double Y { get; set; }

		public MouseMoveMessage()
			: base((ushort)CustomMessageType.MouseMove)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(X);
			message.Put(Y);

		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			X = message.GetDouble();
			Y = message.GetDouble();
		}
	}
}
