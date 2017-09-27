using System;
using Network.Utils;
using Model;
using LiteNetLib.Utils;

namespace Network.Messages.System
{
	public class RequestClientIntroducerRegistrationMessage : BaseMessage
	{
		public RequestClientIntroducerRegistrationMessage()
			: base((ushort)CustomMessageType.RequestClientIntroducerRegistration)
		{
            EncryptedMessage = false;
		}

		public override void WritePayload(NetDataWriter message)
		{
			base.WritePayload(message);
		}

		public override void ReadPayload(NetDataReader message)
		{
			base.ReadPayload(message);
		}

	}
}
