using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace Driver.Windows
{
	public class Display : Driver.Interfaces.Display
	{

		Rectangle bounds;

		public Display()
		{
			bounds = Screen.GetBounds(Point.Empty);
		}

		public int getScreenHeight()
		{
			return bounds.Height;
		}

		public int getScreenWidth()
		{
			return bounds.Width;
		}

		public byte[] makeScreenshot()
		{
			byte[] image = new byte[] { };

			var bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
							   Screen.PrimaryScreen.Bounds.Height,
							   PixelFormat.Format32bppArgb);

			// Create a graphics object from the bitmap.
			var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

			// Take the screenshot from the upper left corner to the right bottom corner.
			gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
										Screen.PrimaryScreen.Bounds.Y,
										0,
										0,
										Screen.PrimaryScreen.Bounds.Size,
										CopyPixelOperation.SourceCopy);

			image = Convert.FromBase64String(bmpScreenshot.ToString());
			return image;
		}
	}
}
