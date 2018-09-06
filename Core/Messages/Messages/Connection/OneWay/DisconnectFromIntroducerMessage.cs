using System;
using LiteNetLib.Utils;
using Model;

namespace Messages.Connection.OneWay
{
	public class DisconnectFromIntroducerMessage : BaseMessage
	{
		public String SystemId { get; set; }

		public DisconnectFromIntroducerMessage()
			: base((ushort)CustomMessageType.DisconnectFromIntroducer)
		{
            EncryptedMessage = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
			message.Put(SystemId);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
            SystemId = message.GetString(100);
		}

	}
}
