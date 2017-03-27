using System;

namespace Common.EventArgs.Network
{
	public class ScreenshotRequestEventArgs : System.EventArgs
	{
		public String HostSystemId { get; set; }
		public String ClientSystemId { get; set; }
        public Boolean Fullscreen { get; set; }
	}
}
