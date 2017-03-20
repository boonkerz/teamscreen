using System;
using LiteNetLib.Utils;
using Model;

namespace Network.Messages.Connection.Response
{
	public class InitalizeHostConnectionMessage : BaseMessage
	{

		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
		public String PublicKey { get; set; }
		public bool HostFound { get; set; }

		public InitalizeHostConnectionMessage()
			: base((ushort)CustomMessageType.ResponseInitalizeHostConnection)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(HostSystemId);
			message.Put(ClientSystemId);
			message.Put(PublicKey);
			message.Put(HostFound);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			HostSystemId = message.GetString(100);
			ClientSystemId = message.GetString(100);
			PublicKey = message.GetString(250);
			HostFound = message.GetBool();
		}

	}
}
