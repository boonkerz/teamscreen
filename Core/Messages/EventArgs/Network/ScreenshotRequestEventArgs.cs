using System;

namespace Messages.EventArgs.Network
{
	public class ScreenshotRequestEventArgs 
	{
		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
        public Boolean Fullscreen { get; set; }
	}
}
