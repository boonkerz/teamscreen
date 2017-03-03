using System;

namespace Common.EventArgs.Network
{
	public class MouseClickEventArgs : System.EventArgs
	{
		public enum ButtonType : int
		{
			Left = 1,
			Middle = 2,
			Right = 3
		}

		public double X { get; set; }
		public double Y { get; set; }
		public ButtonType Button { get; set; }
	}
}
