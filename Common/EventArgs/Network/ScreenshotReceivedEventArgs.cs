using System;

namespace Common.EventArgs.Network
{
	public class ScreenshotReceivedEventArgs : System.EventArgs
	{
		public byte[] Image { get; set; }
	}
}
