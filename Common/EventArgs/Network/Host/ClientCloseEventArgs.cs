using System;

namespace Common.EventArgs.Network.Host
{
	public class ClientCloseEventArgs : System.EventArgs
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
	}
}
