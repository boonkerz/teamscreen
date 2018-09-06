using LiteNetLib.Utils;

namespace Messages.System
{
	public class RequestHostDisconnectMessage : BaseMessage
	{

		public RequestHostDisconnectMessage()
			: base((ushort)CustomMessageType.RequestHostDisconnect)
		{
            EncryptedMessage = false;
		}

	}
}
