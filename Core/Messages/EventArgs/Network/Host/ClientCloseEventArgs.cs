using System;

namespace Messages.EventArgs.Network.Host
{
	public class ClientCloseEventArgs 
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
	}
}
