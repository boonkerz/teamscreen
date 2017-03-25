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
        ResponseEmptyScreenshot,
        MouseMove,
		MouseClick,
		MouseScroll,
		KeyPress,
		KeyRelease,
		RequestCheckOnline,
		ResponseCheckOnline,
		RequestInitalizeHostConnection,
		ResponseInitalizeHostConnection
	}
}
