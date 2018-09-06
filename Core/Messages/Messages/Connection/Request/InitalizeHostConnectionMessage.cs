using System;
using LiteNetLib.Utils;

namespace Messages.Connection.Request
{
	public class InitalizeHostConnectionMessage : BaseMessage
	{

		public String ClientPublicKey { get; set; }

		public InitalizeHostConnectionMessage()
			: base((ushort)CustomMessageType.RequestInitalizeHostConnection)
		{
            EncryptedMessage = false;
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
