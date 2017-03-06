using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Driver.Windows
{
	public class Mouse : Driver.Interfaces.Mouse
	{
		protected ProcessStartInfo info;

		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
		//Mouse actions
		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

		public Mouse()
		{
			

		}

		public void clickLeft(int x, int y)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
			mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, (uint)x, (uint)y, 0, 0);
		}

		public void clickMiddle(int x, int y)
		{
			
		}

		public void clickRight(int x, int y)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
			mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, (uint)x, (uint)y, 0, 0);
		}

		public void move(int x, int y)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
		}
	}
}
