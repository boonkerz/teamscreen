using System;

namespace Messages.EventArgs.Network.Client
{
	public class HostInitalizeConnectedEventArgs 
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
		public String HostPublicKey { get; set; }
	}
}
