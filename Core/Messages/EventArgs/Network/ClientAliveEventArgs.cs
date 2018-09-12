using System;

namespace Messages.EventArgs.Network
{
	public class ClientAliveEventArgs 
	{
		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
	}
}
