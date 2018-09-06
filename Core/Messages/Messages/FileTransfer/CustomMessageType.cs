using System;
namespace Messages.FileTransfer
{
	public enum CustomMessageType : ushort
	{
		ProxyMessage = 1000,
		RequestListing,
		ResponseListing,
        RequestCopy,
        ResponseCopy,
        RequestReceive,
        ResponseReceive
    }
}
