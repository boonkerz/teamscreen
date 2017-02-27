using System;
using LiteNetLib;

namespace Network
{
	public class Peer
	{
		public NetEndPoint InternalAddr { get; private set; }
		public NetEndPoint ExternalAddr { get; private set; }
		public DateTime RefreshTime { get; private set; }

		public void Refresh()
		{
			RefreshTime = DateTime.Now;
		}

		public Peer(NetEndPoint internalAddr, NetEndPoint externalAddr)
		{
			Refresh();
			InternalAddr = internalAddr;
			ExternalAddr = externalAddr;
		}

	}
}
