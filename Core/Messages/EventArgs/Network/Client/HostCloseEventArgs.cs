using System;

namespace Messages.EventArgs.Network.Client
{
	public class HostCloseEventArgs 
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
	}
}
