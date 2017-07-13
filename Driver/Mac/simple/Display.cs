using Common.EventArgs.Network;
using Network;
using Network.Messages.Connection;
using System;
namespace Driver.Mac.Simple
{
	public class Display : BaseDisplay
	{

		/*Gdk.Window window;

		public Display()
		{
			window = Gdk.Global.DefaultRootWindow;
		}

		public int getScreenHeight()
		{
			return window.Screen.Height;
		}

		public int getScreenWidth()
		{
			return window.Screen.Width;
		}

		public override void SendScreenshot(Boolean fullscreen)
		{
			byte[] image = new byte[] { };
            
			if (window != null)
			{
				Gdk.Pixbuf pixBuf = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, false, 8,
									   window.Screen.Width, window.Screen.Height);
				pixBuf.GetFromDrawable(window, Gdk.Colormap.System, 0, 0, 0, 0,
									   window.Screen.Width, window.Screen.Height);


				ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
				rs.Bounds = new System.Drawing.Rectangle(0, 0, this.getScreenWidth(), this.getScreenHeight());
				rs.Fullscreen = true;
				rs.HostSystemId = HostManager.SystemId;
				rs.Image = pixBuf.SaveToBuffer("jpeg");

				foreach (var ID in ConnectedClients)
				{
					rs.ClientSystemId = ID;
					HostManager.sendMessage(rs);
				}
			}*
		}*/
	}
}