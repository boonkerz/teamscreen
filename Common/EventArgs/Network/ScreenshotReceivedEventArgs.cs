using System;

namespace Common.EventArgs.Network
{
	public class ScreenshotReceivedEventArgs : System.EventArgs
	{
		public byte[] Image { get; set; }
		public int ScreenWidth { get; set; }
		public int ScreenHeight { get; set; }
		public String SystemId { get; set; }
	}
}
