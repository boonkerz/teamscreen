using Common.EventArgs.Network;
using Network;
using Network.Messages.Connection;
using System;
namespace Driver.Mac
{
	public class Display : Driver.Interfaces.Display
	{

		Gdk.Window window;

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

		public void RequestScreenshot(ScreenshotRequestEventArgs e, HostManager hm)
        {
            ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
            rs.HostSystemId = e.HostSystemId;
            rs.ClientSystemId = e.ClientSystemId;
            rs.Bounds = new System.Drawing.Rectangle(0, 0, this.getScreenWidth(), this.getScreenHeight());
      
            byte[] image = new byte[] { };

            if (window != null)
            {
                Gdk.Pixbuf pixBuf = new Gdk.Pixbuf(Gdk.Colorspace.Rgb, false, 8,
                                       window.Screen.Width, window.Screen.Height);
                pixBuf.GetFromDrawable(window, Gdk.Colormap.System, 0, 0, 0, 0,
                                       window.Screen.Width, window.Screen.Height);

                rs.Image = pixBuf.SaveToBuffer("jpeg");
            }

            hm.sendMessage(rs);
        }
    }
}
