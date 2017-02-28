using System;
namespace Network.Messages.System
{
	public enum CustomMessageType : ushort
	{
		ProxyMessage = 0,
		KeepAliveMessage,
		ProxyResponseMessage,
		RequestHostIntroducerRegistration,
		ResponseHostIntroducerRegistration,
		RequestClientIntroducerRegistration,
		ResponseClientIntroducerRegistration
	}
}
