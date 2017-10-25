using Common.EventArgs.Network;
using Network;
using Network.Messages.Connection;
using System;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Diagnostics;

namespace Driver.Mac.Simple
{
	public class Display : BaseDisplay
	{

		System.Drawing.Rectangle bounds;

		public Display()
		{
			bounds = System.Windows.Forms.Screen.GetBounds(System.Drawing.Point.Empty);
		}

		public int getScreenHeight()
		{
			return bounds.Height;
		}

		public int getScreenWidth()
		{
			return bounds.Width;
		}

		public override void SendScreenshot(Boolean fullscreen)
		{

			Image im = OsXCapture(true);

			ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
			rs.Bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
			rs.Fullscreen = true;
			rs.HostSystemId = HostManager.SystemId;
			var msout = new MemoryStream();
			im.Save(msout, ImageFormat.Png);
			rs.Image = msout.ToArray();
			
			foreach (var ID in ConnectedClients)
			{
				rs.ClientSystemId = ID;
				HostManager.sendMessage(rs);
			}
		}

		private static Image OsXCapture(bool onlyPrimaryScreen)
		{
			var data = ExecuteCaptureProcess(
				"screencapture",
				string.Format("{0} -T0 -tpng -S -x", onlyPrimaryScreen ? "-m" : ""));
			
			return data;
		}

		private static Image ExecuteCaptureProcess(string execModule, string parameters)
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
				throw new InvalidOperationException(string.Format("Failed to capture screenshot using {0}", execModule));
			}

			Image img = null;

			try
			{
				img = Image.FromFile(imageFileName);
			}
			catch (Exception e) {
				throw new InvalidOperationException(e.Message);
			}
			finally
			{
				File.Delete(imageFileName);
			}

			return img;
		}
	}
}