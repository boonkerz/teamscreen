using System;

namespace Common.EventArgs.Network.Client
{
	public class HostCloseEventArgs : System.EventArgs
	{
		public String ClientSystemId { get; set; }
		public String HostSystemId { get; set; }
	}
}
