using Common.EventArgs.Network;
using Driver.Windows.Screen;
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
        private ScreenCapture _ScreenCapture;
        public RawImage _LastImage = null;

        public Display()
        {
            bounds = System.Windows.Forms.Screen.GetBounds(System.Drawing.Point.Empty);
            _ScreenCapture = new Screen.ScreenCapture(80);
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
            //byte[] image = new byte[] { };

            var bmpScreenshot = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                               System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,
                               PixelFormat.Format32bppArgb);

            // Create a graphics object from the bitmap.
            var img = _ScreenCapture.GetScreen(new Size(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height));

            //var gfxScreenshot = Graphics.FromImage(img.Data);

            /*// Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X,
                                        System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);

            var stream = new MemoryStream();
            bmpScreenshot.Save(stream, ImageFormat.Png);*/

            ResponseScreenshotMessage rs = new ResponseScreenshotMessage();
            rs.Bounds = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            rs.Fullscreen = true;
            rs.HostSystemId = HostManager.SystemId;
            using (var msout = new MemoryStream())
            {
                unsafe
                {
                    fixed (byte* datb = img.Data)
                    {
                        using (Bitmap image = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width * 4, PixelFormat.Format32bppRgb, new IntPtr(datb)))
                        {
                            image.Save(msout, ImageFormat.Png);
                            rs.Image = msout.ToArray();

                        }
                    }
                }
            }

            //rs.Image = stream.ToArray();
            
            foreach(var ID in ConnectedClients)
            {
                rs.ClientSystemId = ID;
                HostManager.sendMessage(rs);
            }
        }
    }
}
