using System;
using System.Diagnostics;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace Driver.Mac
{
	public class Display : Driver.BaseDriver.Display
	{

		protected int width = 0;
		protected int height = 0;

		public Display()
		{
			Configuration.Default.ImageFormatsManager.SetEncoder(ImageFormats.Jpeg, new JpegEncoder()
			{
				Quality = 5,
				IgnoreMetadata = true
			});

		}

		protected override System.Drawing.Rectangle getBounds()
		{
			return new System.Drawing.Rectangle(0,0, this.width, this.height);
		}

		protected override byte[] createScreenShot()
		{
			var img = this.OsXCapture(true);
			this.width = img.Width;
			this.height = img.Height;
			var msout = new MemoryStream();
			img.Save(msout, ImageFormats.Jpeg);
			return msout.ToArray();
		}

		private Image<Rgba32> OsXCapture(bool onlyPrimaryScreen)
		{
			var data = ExecuteCaptureProcess(
				"screencapture",
				string.Format("{0} -T0 -tjpg -S -x", onlyPrimaryScreen ? "-m" : ""));

			return data;
		}

		private Image<Rgba32> ExecuteCaptureProcess(string execModule, string parameters)
		{
			var imageFileName = Path.Combine(Path.GetTempPath(), string.Format("screenshot_{0}.jpg", Guid.NewGuid()));

			var process = Process.Start(execModule, string.Format("{0} {1}", parameters, imageFileName));
			if (process == null)
			{
				throw new InvalidOperationException(string.Format("Executable of '{0}' was not found", execModule));
			}
			process.WaitForExit();

			if (!File.Exists(imageFileName))
			{

			}

			Image<Rgba32> img = null;

			try
			{
			  	img = Image.Load(imageFileName);
			}
			catch(Exception e) {
				throw new InvalidOperationException(string.Format("Failed to capture screenshot using {0}", execModule));
			}

			File.Delete(imageFileName);


			return img;
		}
	}
}
