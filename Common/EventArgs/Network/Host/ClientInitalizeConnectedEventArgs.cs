using System;

namespace Common.EventArgs.Network.Host
{
	public class ClientInitalizeConnectedEventArgs : System.EventArgs
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
		public String ClientPublicKey { get; set; }
	}
}
