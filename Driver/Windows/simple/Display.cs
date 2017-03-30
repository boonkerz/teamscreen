using Common.EventArgs.Network;
using Network;
using Network.Messages.Connection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Driver.Windows.Simple
{
    class Display : BaseDisplay
    {
        System.Drawing.Rectangle bounds;

        public Display()
        {
            bounds = Screen.GetBounds(System.Drawing.Point.Empty);
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

            var stream = new MemoryStream();
            bmpScreenshot.Save(stream, ImageFormat.Png);

            ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
            rs.Bounds = Screen.PrimaryScreen.Bounds;
            rs.Fullscreen = true;
            rs.HostSystemId = HostManager.SystemId;
            rs.Image = stream.ToArray();
            
            foreach(var ID in ConnectedClients)
            {
                rs.ClientSystemId = ID;
                HostManager.sendMessage(rs);
            }
        }
    }
}
