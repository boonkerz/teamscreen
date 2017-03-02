using System;
namespace Network.Messages.Connection
{
	public enum CustomMessageType : ushort
	{
		ProxyMessage = 100,
		ProxyResponseMessage,
		RequestHostConnection,
		ResponseHostConnection,
		RequestScreenshot,
		ResponseScreenshot,
		MouseMove,
		MouseClick,
		MouseScroll,
		KeyPress
	}
}
