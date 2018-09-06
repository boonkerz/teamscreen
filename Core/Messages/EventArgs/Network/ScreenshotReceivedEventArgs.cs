using System;
using System.Drawing;

namespace Messages.EventArgs.Network
{
	public class ScreenshotReceivedEventArgs 
	{
		public byte[] Image { get; set; }
		public Rectangle Bounds { get; set; }
		public String SystemId { get; set; }
		public Boolean Nothing { get; set; }
        public Boolean Fullscreen { get; set; }
    }
}
