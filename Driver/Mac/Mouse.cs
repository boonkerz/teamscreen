using System;
using System.Diagnostics;

namespace Driver.Mac
{
	public class Mouse : Driver.Interfaces.Mouse
	{
		protected ProcessStartInfo info;

		public Mouse()
		{
			info = new ProcessStartInfo();
			info.FileName = AppDomain.CurrentDomain.BaseDirectory + "/cliclick";


			info.UseShellExecute = false;
			info.CreateNoWindow = true;

			info.RedirectStandardOutput = true;
			info.RedirectStandardError = true;

		}

		public void clickLeft(int x, int y)
		{
			info.Arguments = "c:" + x + "," + y;

			var p = Process.Start(info);
			p.WaitForExit();
		}

		public void clickMiddle(int x, int y)
		{
			info.Arguments = "mc:" + x + "," + y;

			var p = Process.Start(info);
			p.WaitForExit();
		}

		public void clickRight(int x, int y)
		{
			info.Arguments = "rc:" + x + "," + y;

			var p = Process.Start(info);
			p.WaitForExit();
		}

		public void move(int x, int y)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
		}
	}
}
