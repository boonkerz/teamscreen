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

        public void ClickDownLeft(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ClickDownMiddle(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ClickDownRight(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ClickLeft(int x, int y)
		{
			info.Arguments = "c:" + x + "," + y;

			var p = Process.Start(info);
			p.WaitForExit();
		}

		public void ClickMiddle(int x, int y)
		{
			info.Arguments = "mc:" + x + "," + y;

			var p = Process.Start(info);
			p.WaitForExit();
		}

		public void ClickRight(int x, int y)
		{
			info.Arguments = "rc:" + x + "," + y;

			var p = Process.Start(info);
			p.WaitForExit();
		}

        public void ClickUpLeft(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ClickUpMiddle(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void ClickUpRight(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DoubleClickLeft(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DoubleClickMiddle(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void DoubleClickRight(int x, int y)
        {
            throw new NotImplementedException();
        }

        public void Move(int x, int y)
		{
			System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
		}
	}
}
