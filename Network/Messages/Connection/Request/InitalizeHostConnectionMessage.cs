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
			this.Encrypt(message);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
			this.Decrypt(message);
			ClientPublicKey = message.GetString(250);
		}

	}
}
