using System;

namespace Common.EventArgs.Network.Client
{
	public class HostInitalizeConnectedEventArgs : System.EventArgs
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
		public String HostPublicKey { get; set; }
	}
}
