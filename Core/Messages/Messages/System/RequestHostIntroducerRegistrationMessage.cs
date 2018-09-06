using System;
using Messages.System;
using Model;
using LiteNetLib.Utils;

namespace Messages.System
{
	public class RequestHostIntroducerRegistrationMessage : BaseMessage
	{

		public String SystemId { get;set; }

		public RequestHostIntroducerRegistrationMessage()
			: base((ushort)CustomMessageType.RequestHostIntroducerRegistration)
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
