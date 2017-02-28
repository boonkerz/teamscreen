using System;
using LiteNetLib.Utils;
using Model;

namespace Network.Messages.Connection
{
	public class ResponseHostConnectionMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public String Password { get; set; }
		public bool PasswordOk { get; set; }
		public bool HostFound { get; set; }

		public ResponseHostConnectionMessage()
			: base((ushort)CustomMessageType.ResponseHostConnection)
		{
			HostFound = true;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(Password);
			message.Put(PasswordOk);
			message.Put(HostFound);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			Password = message.GetString(100);
			PasswordOk = message.GetBool();
			HostFound = message.GetBool();
		}

	}
}
