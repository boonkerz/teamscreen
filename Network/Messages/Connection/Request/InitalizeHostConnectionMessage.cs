using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.Connection.Request
{
	public class InitalizeHostConnectionMessage : BaseMessage
	{

		public String ClientPublicKey { get; set; }

		public InitalizeHostConnectionMessage()
			: base((ushort)CustomMessageType.RequestInitalizeHostConnection)
		{
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(ClientPublicKey);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			ClientPublicKey = message.GetString(250);
		}

	}
}
