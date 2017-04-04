using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.Connection.Request
{
	public class HostConnectionMessage : BaseMessage
	{
		public String Password { get; set; }
		public String SymmetricKey { get; set; }

		public HostConnectionMessage()
			: base((ushort)CustomMessageType.RequestHostConnection)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(Password);
			message.Put(SymmetricKey);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			Password = message.GetString(300);
			SymmetricKey = message.GetString(300);
		}

	}
}
