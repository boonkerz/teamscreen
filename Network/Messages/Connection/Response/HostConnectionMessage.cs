using System;
using LiteNetLib.Utils;
using Model;

namespace Network.Messages.Connection.Response
{
	public class HostConnectionMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public bool PasswordOk { get; set; }

		public HostConnectionMessage()
			: base((ushort)CustomMessageType.ResponseHostConnection)
		{
			
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(PasswordOk);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			PasswordOk = message.GetBool();
		}

	}
}
