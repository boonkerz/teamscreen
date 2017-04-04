using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.Connection.Response
{
	public class InitalizeHostConnectionMessage : BaseMessage
	{

		public String PublicKey { get; set; }
		public bool HostFound { get; set; }

		public InitalizeHostConnectionMessage()
			: base((ushort)CustomMessageType.ResponseInitalizeHostConnection)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(PublicKey);
			message.Put(HostFound);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			PublicKey = message.GetString(250);
			HostFound = message.GetBool();
		}

	}
}
