using System;

namespace Messages.EventArgs.Network
{
	public class StopScreenSharingEventArgs 
	{
		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
	}
}
