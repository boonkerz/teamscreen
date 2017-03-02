using System;
using LiteNetLib.Utils;

namespace Network.Messages.Connection
{
	public class KeyPressMessage: BaseMessage
	{
		public KeyPressMessage()
			: base((ushort)CustomMessageType.KeyPress)
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
