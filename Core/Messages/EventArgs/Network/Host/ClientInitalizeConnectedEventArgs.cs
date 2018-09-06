using System;

namespace Messages.EventArgs.Network.Host
{
	public class ClientInitalizeConnectedEventArgs 
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
		public String ClientPublicKey { get; set; }
	}
}
