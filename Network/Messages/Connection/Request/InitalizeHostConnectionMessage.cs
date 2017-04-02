using System;
using Network.Utils;
using Model;

namespace Network.Messages.Connection.Request
{
	public class InitalizeHostConnectionMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public String ClientPublicKey { get; set; }

		public InitalizeHostConnectionMessage()
			: base((ushort)CustomMessageType.RequestInitalizeHostConnection)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(ClientPublicKey);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			ClientPublicKey = message.GetString(250);
		}

	}
}
