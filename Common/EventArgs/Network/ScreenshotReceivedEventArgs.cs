using System;
using System.Drawing;

namespace Common.EventArgs.Network
{
	public class ScreenshotReceivedEventArgs : System.EventArgs
	{
		public byte[] Image { get; set; }
		public Rectangle Bounds { get; set; }
		public String SystemId { get; set; }
	}
}
