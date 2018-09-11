using Driver.Win.Mode;
using Driver.Win.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Win
{
    public class Display : Driver.BaseDriver.Display
    {

        protected int width = 0;
        protected int height = 0;

        protected Simple screenWork = new Simple();

        RawImage img = null;

        public Display()
        {
            
        }

        protected override System.Drawing.Rectangle getBounds()
        {
            return new System.Drawing.Rectangle(0,0,width,height);
        }

        protected override byte[] createScreenShot()
        {
            img = screenWork.createScreenshot(true);
            
            this.width = img.Dimensions.Width;
            this.height = img.Dimensions.Height;
            var msout = new MemoryStream();

            var bmpScreenshot = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width,
                               System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height,
                               PixelFormat.Format32bppArgb);

            var gfxScreenshot = Graphics.FromImage(bmpScreenshot);

            // Take the screenshot from the upper left corner to the right bottom corner.
            gfxScreenshot.CopyFromScreen(System.Windows.Forms.Screen.PrimaryScreen.Bounds.X,
                                        System.Windows.Forms.Screen.PrimaryScreen.Bounds.Y,
                                        0,
                                        0,
                                        System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size,
                                        CopyPixelOperation.SourceCopy);


            bmpScreenshot.Save(msout, ImageFormat.Jpeg);
            return msout.ToArray();
            
        }

        
    }
}
