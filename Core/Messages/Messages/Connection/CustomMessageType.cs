using System;

namespace Messages.Connection
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
		Key,
		RequestCheckOnline,
		ResponseCheckOnline,
		RequestInitalizeHostConnection,
		ResponseInitalizeHostConnection,
        RequestCloseHostConnection,
        ResponseCloseHostConnection,
        DisconnectFromIntroducer,
		StartScreenSharing,
		StopScreenSharing,
        ClientAlive
    }
}
