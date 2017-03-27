using System;
using LiteNetLib.Utils;

namespace Network.Messages.Connection.OneWay
{
	public class MouseClickMessage: BaseMessage
	{
		public enum ButtonType : int
		{
			Left = 1,
			Middle = 2,
			Right = 3
		}

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public Double X { get; set; }
		public Double Y { get; set; }
		public ButtonType Button { get; set; }

		public MouseClickMessage()
			: base((ushort)CustomMessageType.MouseClick)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(X);
			message.Put(Y);
			message.Put((int)this.Button);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			X = message.GetDouble();
			Y = message.GetDouble();
			Button = (ButtonType)message.GetInt();
		}
	}
}
